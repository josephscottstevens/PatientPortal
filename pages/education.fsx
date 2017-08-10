#r "../packages/Suave.Experimental/lib/net40/Suave.Experimental.dll"
#load "../dataAccess.fsx"

open Suave.Html
open DataAccess

let Home = 
  let getPdf url str =
    a url ["class", "list-group-item"; "target", "blank"] (text str)

  div ["class", "container-fluid horizontalCentered"] [
    div ["class", "row"] [
      div ["id", "MainMenu"] [
        div ["class", "list-group panel"] [
          getPdf "../Content/brochureBasicsofalz_low.pdf" "Alzheimer’s Disease"
          getPdf "../Content/tenwarnsignsofalzheimers.pdf" "10 Warning Signs of Alzheimer’s Disease"
          getPdf "../Content/brochure_prevention.pdf" "Preventing Alzheimer's Disease"
          getPdf "../Content/topicsheetTreatments.pdf"  "Approved treatments for Alzheimer’s"
          getPdf "../Content/copdAtrisk.pdf" "Basics Of Chronic Obstructive Pulmonary Disease"
        ]
      ]
    ]
  ]