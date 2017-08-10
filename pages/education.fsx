#r "../packages/Suave.Experimental/lib/net40/Suave.Experimental.dll"
#load "../dataAccess.fsx"

open Suave.Html
open DataAccess

let Home = 
  div ["class", "container-fluid horizontalCentered"] [
    div ["class", "row"] [
      div ["id", "MainMenu"] [
        div ["class", "list-group panel"] [
          a "../Content/brochure_basicsofalz_low.pdf" ["class", "list-group-item"] (text "Alzheimer’s Disease")
          a "../Content/tenwarnsignsofalzheimers.pdf" ["class", "list-group-item"] (text "10 Warning Signs of Alzheimer’s Disease")
          a "../Content/brochure_prevention.pdf" ["class", "list-group-item"] (text "Preventing Alzheimer's Disease")
          a "../Content/topicsheetTreatments.pdf" ["class", "list-group-item"] (text "Approved treatments for Alzheimer’s")
          a "../Content/copdAtrisk.pdf" ["class", "list-group-item"] (text "Basics Of Chronic Obstructive Pulmonary Disease")
        ]
      ]
    ]
  ]