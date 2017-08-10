#load "root.fsx"

open Suave
open Suave.Cookie
open Suave.Filters
open Suave.Operators
open Suave.RequestErrors
open Suave.Utils
open Suave.Successful
open Suave.Authentication
open Root


let app = 
    State.CookieStateStore.statefulForSession >=> choose 
      [
        Files.browseHome
        path "/"            >=> OK (Root.samplePage "/" Root.tempContent)
        path "/careplan"    >=> OK Root.carePlanPage
        path "/feedback"    >=> OK (Root.samplePage "/feedback" Root.tempContent)
        path "/medications" >=> OK (Root.samplePage "/medications" Root.tempContent)
        path "/forms"       >=> OK (Root.samplePage "/forms" Root.tempContent)
        path "/education"   >=> OK (Root.samplePage "/education" Root.tempContent)
        path "/proxies"     >=> OK (Root.samplePage "/proxies" Root.tempContent)
      ]