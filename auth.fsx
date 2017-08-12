#r "CSharpLibrary.dll"
#r "./packages/Suave/lib/net40/Suave.dll"
#load "dataAccess.fsx"
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
open Login

let authenticateUser = authenticated Session false
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
    
let tryLogin (partialTemplate:Html.Node -> string) (a:HttpContext) =
    let usr = getKey a "UserName"
    let pw = getKey a "Password"
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

    let loginPage = partialTemplate (Login.Home result)
    OK loginPage a