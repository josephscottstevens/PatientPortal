#r "packages/SQLProvider/lib/FSharp.Data.SqlProvider.dll"
#r "packages/iTextSharp/lib/itextsharp.dll"
open FSharp.Data.Sql
open System.IO
open iTextSharp.text.pdf
open iTextSharp.text
type Sql = SqlDataProvider<ConnectionString = """Data Source=localhost;Initial Catalog=NavcareDB_backup;Integrated Security=True;""",
                            DatabaseVendor = Common.DatabaseProviderTypes.MSSQLSERVER, 
                            UseOptionTypes = true>

let ctx = Sql.GetDataContext()

let format (str:string) =
    let doc = new Document()
    let ms = new MemoryStream()
    PdfWriter.GetInstance(doc, ms) |> ignore
    doc.Open()
    doc.Add(new Paragraph(str)) |> ignore
    doc.Close()
    ms.ToArray()

let updateDb param =
    let carePlanFile, patientId, createdAt = param

    let x = ctx.Cls.CarePlanFiles.Create()
    x.CarePlanFile <- carePlanFile
    x.CreateDate <- createdAt
    x.PatientId <- patientId
    x.Month <- createdAt.Month
    x.Year <- createdAt.Year
    ctx.SubmitUpdates()

let testCarePlan = 
    query {
        for t in ctx.Cls.ClinicalSummaries do
        sortBy t.PatientId
        where (t.CarePlan.IsSome)
        select (format t.CarePlan.Value, t.PatientId, t.CreatedAt)
    }
    |> Seq.map updateDb
    |> Seq.toList

//DROP TABLE cls.CarePlanFiles
//GO
//CREATE TABLE cls.CarePlanFiles
//(
//	[PatientId] int not null,
//	[Year] int not null,
//	[Month] int not null,
//	[CarePlanFile] VARBINARY(MAX) not null,
//	[CreateDate] DateTime not null,
//	Primary Key ([PatientId], [Year], [Month])
//)
//GO