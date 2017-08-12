#r "../packages/Suave.Experimental/lib/net40/Suave.Experimental.dll"

open Suave.Html

type LoginResult = Initial | Valid | NoUserFound | InvalidPassword | NoUserName | NoPassword

let h2 = tag "h2"
let label = tag "label"

let form = tag "form"
let Home (result: LoginResult) = 

  form ["action", "/login"; "method", "post"] [
    h2 [] (text "Log in.")
    div [] [
      text "UserName" |> List.head
      input ["type", "text"; "name", "UserName"]
    ]
    div [] [
      text "Password" |> List.head
      input ["type", "text"; "name", "Password"]
    ]
    input ["type", "submit"; "value", "Submit"]
  ]