#r "../packages/Suave.Experimental/lib/net40/Suave.Experimental.dll"

open Suave.Html

let Home = 
  let ul = tag "ul"
  let li = tag "li"
  
  div [] [
    p [] (text "Your proxies:")
    ul [] [
      li [] (text "Stephen")
      li [] (text "James")
    ]
  ]