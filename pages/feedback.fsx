#r "../packages/Suave.Experimental/lib/net40/Suave.Experimental.dll"

open Suave.Html

let h4 = tag "h4"
let textarea = tag "textarea"
let button = tag "button"
let br = tag "br" [] []
let Home = 
  div [] [
    h4 [] (text "SEND A MESSAGE TO YOUR CARE TEAM")
    p [] (text "Comment: ")
    textarea ["style", "min-width: 99% !important; height: 10em"] []
    br
    button ["class", "btn btn-default"; "style", "float: right; margin-left: 5px;"] (text "Reset")
    button ["class", "btn btn-primary"; "style", "float: right;"] (text "Submit")
  ]