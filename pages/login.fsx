#r "../SuaveHtml.dll"
open SuaveHtml

type LoginResult = Initial | Valid | NoUserFound | InvalidPassword | NoUserName | NoPassword
let getErrorMessage a =
  let msg =
    match a with
    | Initial -> ""
    | Valid -> ""
    | NoUserFound -> "No user found by that name, sorry!"
    | InvalidPassword -> "Password is invalid"
    | NoUserName -> "Username is required"
    | NoPassword -> "Password is required"
  p ["style", "color: red"] (text msg)


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
    getErrorMessage result
  ]