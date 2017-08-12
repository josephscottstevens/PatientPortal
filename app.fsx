#load "root.fsx"
#load "auth.fsx"

open Suave
open Suave.Cookie
open Suave.Filters
open Suave.Operators
open Suave.RequestErrors
open Suave.Utils
open Suave.Successful
open Root
open Auth

let app = 
  State.CookieStateStore.statefulForSession >=> choose 
    [
      Files.browseHome
      path "/"            >=> OK (basePage "/" HomePage.Home)
      path "/login"       >=> 
        choose [
          GET  >=> getLogin
          POST >=> tryLogin
        ]
      path "/logout"      >=> OK (basePage "/" Logout.Home)
      requireAuth (
        choose [
          path "/careplan"    >=> OK (basePage "/careplan" CarePlan.Home)
          path "/feedback"    >=> OK (basePage "/feedback" Feedback.Home)
          path "/medications" >=> OK (basePage "/medications" Medications.Home)
          path "/forms"       >=> OK (basePage "/forms" Forms.Home)
          path "/education"   >=> OK (basePage "/education" Education.Home)
          path "/proxies"     >=> OK (basePage "/proxies" Proxies.Home)
        ])
    ]