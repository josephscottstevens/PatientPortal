#load "suaveHtml.fsx"

open SuaveHtml

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