#r "CSharpLibrary.dll"
#r "./packages/Suave/lib/net40/Suave.dll"
#load "dataAccess.fsx"

open Suave
open Suave.Operators
open Suave.Successful
open Suave.Filters
open Suave.Cookie
open Suave.Successful
open Suave.RequestErrors
open Suave.Authentication
open DataAccess

let authenticateUser = authenticated Session false
let deAuthenticateUser = deauthenticate
let requireAuth =
    authenticate Session false
       (fun () -> Choice2Of2(FORBIDDEN "please authenticate"))
       (fun _ ->  Choice2Of2(BAD_REQUEST "did you fiddle with our cipher text?"))

type LoginResult = Valid | NoUserFound | InvalidPassword

let verifyPw pass hashPass =
  let owin = CSharpLibrary.OwinAuth()
  if CSharpLibrary.OwinAuth().VerifyHashedPassword(pass, hashPass) then
      Valid
  else 
      NoUserFound

let tryLogin param =
    let user, pw = param
    let aspNetUser = DataAccess.getUsersByEmail user |> Seq.tryHead

    let result =
        match aspNetUser with 
        | Some t -> verifyPw pw t.PasswordHash.Value
        | None -> NoUserFound

    match result with
    | Valid           -> Redirection.FOUND "/"
    | NoUserFound     -> Redirection.FOUND "/login/"
    | InvalidPassword -> Redirection.FOUND "/login/"