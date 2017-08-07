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
let nav = tag "nav"

let homeSvg = rawText """<svg class="homeSvg" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" id="Capa_1" x="0px" y="0px" width="512px" height="512px" viewBox="0 0 512 512" style="enable-background:new 0 0 512 512;" xml:space="preserve"> <g>	<path d="M512,296l-96-96V56h-64v80l-96-96L0,296v16h64v160h160v-96h64v96h160V312h64V296z"></path> </g> <g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>""" |> Seq.head

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
          ul [] [
            li ["class", "nav-item"] [
              homeSvg
              a "/"  [] (text "Welcome")
            ]
            li ["class", "nav-item"] [
              a "/careplan" [] (text "Care Plan")
            ]
            li ["class", "nav-item"] [
              a "/messages" [] (text "Messages")
            ]
            li ["class", "nav-item"] [
              a "/medications" [] (text "Medications")
            ]
            li ["class", "nav-item"] [
              a "/forms" [] (text "Forms")
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