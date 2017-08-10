#r "packages/SQLProvider/lib/FSharp.Data.SqlProvider.dll"
open FSharp.Data.Sql
let [<Literal>] ConnectionString = """Data Source=localhost;Initial Catalog=NavcareDB_backup;Integrated Security=True;"""
type Sql = SqlDataProvider<ConnectionString = ConnectionString,
                           DatabaseVendor = Common.DatabaseProviderTypes.MSSQLSERVER,
                           UseOptionTypes = true>
let ctx = Sql.GetDataContext()

let getUsers =
    query {
        for user in ctx.Dbo.AspNetUsers do
        select user
    }

let getUserById id = 
    getUsers 
    |> Seq.where (fun t -> t.Id = id)
    |> Seq.head

let getUsersByEmail email =
    getUsers
    |> Seq.where(fun t -> t.Email.IsSome)
    |> Seq.where (fun t -> t.Email.Value = email)

let getPatients =
    query {
        for p in ctx.Ptn.Patients do
        select p
    }

let getPatientsByUserId userId =
    getPatients
    |> Seq.where (fun t -> t.UserId = userId)

let getPatientsById id =
    getPatients
    |> Seq.where (fun t -> t.Id = id)

let getPatientsByEmail email =
    getPatients
    |> Seq.where (fun t -> t.Email.IsSome)
    |> Seq.where (fun t -> t.Email.Value = email)

let getProxyUserIdByEmail email =
    let patientId = 
        query {
            for user in ctx.Dbo.AspNetUsers do
            join proxy in ctx.Ptn.Proxies on (user.Id = proxy.UserId.Value)
            where (user.Email.IsSome)
            where (proxy.UserId.IsSome)
            where (user.Email.Value = email)
            select proxy.PatientId
        }
        |> Seq.tryHead
    let patient = 
        match patientId with
        | Some t -> getPatientsById t |> Seq.tryHead
        | None -> None

    match patient with
    | Some t -> Some patient.Value.UserId
    | None -> None

let getCarePlans patientId =
    query {
        for p in ctx.Cls.CarePlanFiles do
        where (p.PatientId = patientId)
        select p
    }

let isFacilityPortalEnabled userId =
    let isEnabledOption =
        query {
            for hcoUser in ctx.Hco.HcoUsers do
            join hco in ctx.Hco.Hcos on (hcoUser.HcoId = hco.Id)
            where (hcoUser.UserId.IsSome)
            where (hcoUser.UserId.Value = userId)
            select hco.PortalEnabled
        }
        |> Seq.tryHead
    if isEnabledOption.IsSome then
        isEnabledOption.Value
    else
        false