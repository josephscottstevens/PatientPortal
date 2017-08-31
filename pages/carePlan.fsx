// #r "../SuaveHtml.dll"
// #load "../dataAccess.fsx"

// open DataAccess
// open SuaveHtml
// let Home = 
//   let carePlans = DataAccess.getCarePlans 1

//   let fm month year =
//     let monthStr = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName month
//     let yearStr = string year
//     a "www.google.com" ["class", "list-group-item"] (text (monthStr + " " + yearStr + " CarePlan"))
    
//   let content = 
//     carePlans
//     |> Seq.map (fun t -> fm t.Month t.Year)
//     |> Seq.toList
    
//   div ["class", "container-fluid horizontalCentered"] [
//     div ["class", "row"] [
//       div ["class", "list-group panel"] content
//     ]
//   ]