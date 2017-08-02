#r "./packages/Suave/lib/net40/Suave.dll"
#r "./packages/Suave.Experimental/lib/net40/Suave.Experimental.dll"
// #load "root.fs"
open Suave
open Suave.Cookie
open Suave.Filters
open Suave.Operators
open Suave.Html
open Suave.RequestErrors
open Suave.Utils
open Suave.Successful
open Suave.Authentication

let ul = tag "ul"

let samplePage =
  html [] [
    head [] [
      title [] "Home"
      link [ "rel", "stylesheet"; "href", "https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css" ]
      link [ "rel", "stylesheet"; "href", "https://fonts.googleapis.com/css?family=Overpass" ]
      link [ "rel", "stylesheet"; "href", "content/Site.css"; "type", "text/css" ]
      meta [ "charset", "utf-8"]
      meta [ "name", "viewport"; "content", "width=device-width, initial-scale=1"]
      script [ "type", "text/javascript"; "src", "https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js" ] []
      script [ "type", "text/javascript"; "src", "https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js" ] []
      script [ "type", "text/javascript" ] (rawText "$().ready(function () { setup(); });" )
    ]
    body [] [
      div ["class", "wrapper"] [
        div ["class", "main-logo"] [
          img ["src", "content/logo.png"; "width", "130px"]
          p [] (text "Primary Care Clinic")
        ]
        div ["class", "main-info"] [
          p [] (text "Proxy Relationship goes here")
          img [ "src", "content/profile.jpg"; "width", "50px"; "class", "profile-small"]
          p [] (text "Patient name goes here")
          a "/logout" ["class", "btn btn-default"] []
        ]
        div ["class", "main-nav"] [
          ul [] [
            a "/" ["class", "nav-item"] [
              span ["class", "glyphicon glyphicon-home"] []
              p [] (text "Welcome")
            ]
            a "/dashboard" ["class", "nav-item"] [
              span ["class", "glyphicon glyphicon-list-alt"] []
              p [] (text "Care Plan")
            ]
          ]
        ]
        div ["class", "main-sidebar"] [
          p [] (text "Stevens, Joseph")
        ]
        div ["class", "main-content"] [
          p [] (text "Here is some test text, we want to add Incomplete forms, messages, new badges and that sort of thing here.")
        ]
      ]
    ]
  ]
  |> htmlToString

let app = 
    State.CookieStateStore.statefulForSession
    >=> choose [
        Files.browseHome
        path "/" >=> OK "Welcome aboard"
        //path "/" >=> Successful.OK samplePage
        //path "/login" >=> Auth.loginMethod
        //path "/logout" >=> Auth.logoutMethod
        //pathScan "/accept/%s" accept  
        //pathScan "/register/%s/%s/%s/%s/%s" registerMethod
        Authentication.authenticateBasic ((=) ("foo", "bar")) <|
                    choose [
                        path "/test/" >=> OK samplePage
                    ]
            // (choose [
            //         //path "/" >=> context index 
            //         path "/dashboard" >=> context dashboard
            //         path "/proxies" >=> context proxies
            //         path "/feedback" >=> context feedback
            //         pathScan "/feedback/%s" messageMethod
            //          ])
                     ]


