#r "CSharpLibrary.dll"
#r "./packages/Suave/lib/net40/Suave.dll"
#load "dataAccess.fsx"
#load "root.fsx"
#load "pages/login.fsx"

open Suave
open Suave.Operators
open Suave.Successful
open Suave.Filters
open Suave.Cookie
open Suave.Successful
open Suave.RequestErrors
open Suave.Authentication
open DataAccess
open Root
open Login

let authenticateUser (a:HttpContext) = authenticated Session false a
let deAuthenticateUser = deauthenticate
let requireAuth =
    authenticate Session false
       (fun () -> Choice2Of2(FORBIDDEN "please authenticate"))
       (fun _ ->  Choice2Of2(BAD_REQUEST "did you fiddle with our cipher text?"))

let getKey r key =
  match r.request.formData key with
    | Choice1Of2 str -> 
        if str = "" then 
            None 
        else 
            Some str
    | Choice2Of2 _ -> None

let verifyPass (pw:string option) (pwHash: string option) =
  if pw.IsNone then
    false
  elif pwHash.IsNone then
    false
  else
    CSharpLibrary.OwinAuth().VerifyHashedPassword(pw.Value, pwHash.Value)
    
let getLogin = 
    OK (insecurePage(Login.Home Login.Initial))
    
let tryLogin =
    context (fun ctx -> 
        let usr = getKey ctx "UserName"
        let pw = getKey ctx "Password"
        let aspNetUser =
            match usr with 
            | Some t -> DataAccess.getUsersByEmail t |> Seq.tryHead
            | None -> None

        let result =
            if usr.IsNone then
                NoUserName
            elif pw.IsNone then
                NoPassword
            elif aspNetUser.IsNone then
                NoUserFound
            elif not (verifyPass pw aspNetUser.Value.PasswordHash) then
                InvalidPassword
            else
                Valid
        
        let loginPage = insecurePage (Login.Home result)
        if result = Valid then
            authenticateUser >=> Redirection.FOUND "/"
        else         
            deAuthenticateUser >=> OK (insecurePage(Login.Home result))
    )