#load "root.fsx"

open Suave
open Suave.Cookie
open Suave.Filters
open Suave.Operators
open Suave.Html
open Suave.RequestErrors
open Suave.Utils
open Suave.Successful
open Suave.Authentication
open Root
let nav = tag "nav"

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
          img ["src", "content/logo.svg"; "width", "130px"]
          p [] (text "Primary Care Clinic")
        ]
        div ["class", "main-info"] [
          p [] (text "Proxy Relationship goes here")
          img [ "src", "content/profile.jpg"; "width", "50px"; "class", "profile-small"]
          p [] (text "Patient name goes here")
          a "/logout" ["class", "btn btn-default"] []
        ]
        nav ["class", "main-nav"] [
          Root.getNavMenu "/"
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