#r "CSharpLibrary.dll"
#r "./packages/Suave/lib/net40/Suave.dll"
#r "./packages/Suave.Experimental/lib/net40/Suave.Experimental.dll"
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
open Suave.Html
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
let nav = tag "nav"

let insecurePage content =
  html [] [
    head [] [
      title [] "Home"
      link [ "rel", "stylesheet"; "href", "https://fonts.googleapis.com/css?family=Overpass" ]
      link [ "rel", "stylesheet"; "href", "content/site.css"; "type", "text/css" ]
      link [ "rel", "stylesheet"; "href", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"; ]
      script [ "src", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"] []
      meta [ "charset", "utf-8"]
      meta [ "name", "viewport"; "content", "width=device-width, initial-scale=1"]
    ]
    body [] [
      div ["class", "wrapper"] [
        div ["class", "main-logo"] [
          img ["src", "content/logo.svg"; "width", "130px"]
        ]
        div ["class", "main-info"] []
        nav ["class", "main-nav"] []
        div ["class", "main-sidebar"] []
        div ["class", "main-content"] [
          content
        ]
      ]
    ]
  ]
  |> htmlToString
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