#load "suaveHtml.fsx"
#load "../dataAccess.fsx"

open SuaveHtml
open DataAccess

let js = 
  """
  $('#example').DataTable( {
        initComplete: function () {
            this.api().columns().every( function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo( $(column.footer()).empty() )
                    .on( 'change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );
 
                        column
                            .search( val ? '^'+val+'$' : '', true, false )
                            .draw();
                    } );
 
                column.data().unique().sort().each( function ( d, j ) {
                    select.append( '<option value="'+d+'">'+d+'</option>' )
                } );
            } );
        }
    } );
  """

let data = 
  DataAccess.getUsers
  |> Seq.where (fun t -> t.NameComputed.IsSome && t.NameComputed.Value <> "")
  |> Seq.where (fun t -> t.HomePhone.IsSome)
  |> Seq.sortByDescending (fun t -> t.NameComputed)
  |> Seq.map (
    fun t -> 
      [t.NameComputed; t.HomePhone; t.HomeCity; t.HomeState; t.HomeZip; t.Gender]
      |> List.map stringToNode
      )
  |> reduceSequence 200

let Home = 
  div [] [
    script [ "type", "text/javascript" ] (rawText js)
    table ["id", "example"; "class", "display"] [
      thead [] [
         tr [] [
          th [] (text "Full Name")
          th [] (text "Phone")
          th [] (text "HomeCity")
          th [] (text "HomeState")
          th [] (text "HomeZip")
          th [] (text "Secret Password")
        ]
      ]
      tbody [] data
    ]
  ]