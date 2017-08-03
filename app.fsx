#r "./packages/Suave/lib/net40/Suave.dll"
#r "./packages/Suave.Experimental/lib/net40/Suave.Experimental.dll"
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
let li = tag "li"

let samplePage =
  html [] [
    head [] [
      title [] "Home"
      link [ "rel", "stylesheet"; "href", "https://fonts.googleapis.com/css?family=Overpass" ]
      link [ "rel", "stylesheet"; "href", "content/site.css"; "type", "text/css" ]
      meta [ "charset", "utf-8"]
      meta [ "name", "viewport"; "content", "width=device-width, initial-scale=1"]
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
            li [] [
              a "/" ["class", "nav-item"] [
                span ["class", "glyphicon glyphicon-home"] []
                p [] (text "Welcome")
              ]
            ]
            li [] [
              a "/dashboard" ["class", "nav-item"] [
                span ["class", "glyphicon glyphicon-list-alt"] []
                p [] (text "Care Plan")
              ]
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
        path "/" >=> OK samplePage
        ]