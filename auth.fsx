#r "CSharpLibrary.dll"

#load "fsharp.fsx"

open Suave
open Suave.Operators
open Suave.Successful
open Suave.Filters
open Suave.Html
open Suave.Helpers

let x =
  let owin = CSharpLibrary.OwinAuth()
  if owin.VerifyHashedPassword("pw", "hashPw") then
      Some "Success"
  else 
      None
let app =
    choose [
        path "/" >=> OK "root"
        path "/auth" >=> authenticateUser >=> OK "authed"
        path "/deauth" >=> deAuthenticateUser >=> OK "deauthed"
        requireAuth (
            choose [
                path "/secure" >=> OK "secures"
            ]
        )
    ]

startWebServer defaultConfig app