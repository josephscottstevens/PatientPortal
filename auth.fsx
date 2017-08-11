// #r "CSharpLibrary.dll"
#r "./packages/Suave/lib/net40/Suave.dll"

open Suave
open Suave.Operators
open Suave.Successful
open Suave.Filters
open Suave.Cookie
open Suave.Successful
open Suave.RequestErrors
open Suave.Authentication

let authenticateUser = authenticated Session false
let deAuthenticateUser = deauthenticate
let requireAuth =
    authenticate Session false
       (fun () -> Choice2Of2(FORBIDDEN "please authenticate"))
       (fun _ ->  Choice2Of2(BAD_REQUEST "did you fiddle with our cipher text?"))

let tryLogin param =
    let a, b = param
    if a = "foo" && b = "bar" then
        authenticated Session false >=> Redirection.FOUND "/"
    else
        Redirection.FOUND "/login/"

// let x =
//   let owin = CSharpLibrary.OwinAuth()
//   if owin.VerifyHashedPassword("pw", "hashPw") then
//       Some "Success"
//   else 
//       None