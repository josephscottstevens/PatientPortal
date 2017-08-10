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
let isProxy = false

let patientDisplayTopRight =
  let ifProxyLst = 
    [
      p [] (text "Patient")
      p [] (text "Sarah O'Conner")
    ]

  let lst =
    [
      p [] (text "Welcome")
      p [] (text "Kiven Homer")
    ]
  if isProxy then 
    lst @ ifProxyLst
  else 
    lst

let samplePage url =
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
        ]
        div ["class", "main-info"] [
          div [] patientDisplayTopRight
          a "/logout" ["class", "btn btn-default"] []
        ]
        nav ["class", "main-nav"] [
          Root.getNavMenu url
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
        path "/"            >=> OK (samplePage "/")
        path "/careplan"    >=> OK (samplePage "/careplan")
        path "/feedback"    >=> OK (samplePage "/feedback")
        path "/medications" >=> OK (samplePage "/medications")
        path "/forms"       >=> OK (samplePage "/forms")
        path "/education"   >=> OK (samplePage "/education")
        path "/proxies"     >=> OK (samplePage "/proxies")
        ]