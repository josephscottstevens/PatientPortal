#load "fsharp.fsx"

open Suave.Html
open Suave.Cookie
open Suave.Operators
open Suave.Successful
open Suave.Filters
open Suave.RequestErrors
open Suave.Authentication
open Suave.Html
open SuaveHelp

let app =
    choose [
        path "/" >=> OK "root"
        path "/auth" >=> authenticated Session false >=> OK "authed"
        path "/deauth" >=> deauthenticate >=> OK "deauthed"
        requireAuth (
            choose [
                path "/secure" >=> OK "secures"
            ]
        )
    ]

startWebServer defaultConfig app