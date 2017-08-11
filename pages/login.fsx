#r "../packages/Suave.Experimental/lib/net40/Suave.Experimental.dll"

open Suave.Html

let h2 = tag "h2"
let label = tag "label"

let Home = 

  div [] [
    h2 [] (text "Log in.")
    div ["class", "row"] [
      div ["class", "col-md-8"] [
        div ["class", "form-group"] [
          label ["class", "col-md-2 control-label"] (text "Email")
          div ["class", "col-md-10"] [
            input ["class", "input-validation-error form-control"; "id", "Email"; "type", "text"]
            span ["class", "field-validation-error text-danger"; "id", "Email_validationMessage"; "style", "display: none" ] (text "The Email field is required.")
          ]
        ]
        div ["class", "form-group"] [
          label ["class", "col-md-2 control-label"] (text "Password")
          div ["class", "col-md-10"] [
            input ["class", "input-validation-error form-control"; "id", "Password"; "type", "text"]
            span ["class", "field-validation-error text-danger"; "id", "Password_validationMessage"; "style", "display: none" ] (text "The Password field is required.")
          ]
        ]
        div ["class", "form-group"] [
          input ["type", "submit"; "value", "Log in"]
        ]
      ]
    ]
  ]