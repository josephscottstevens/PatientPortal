#r "../SuaveHtml.dll"

open SuaveHtml

let Home = 
  
  
  div [] [
    p [] (text "Your proxies:")
    ul [] [
      li [] (text "Stephen")
      li [] (text "James")
    ]
  ]