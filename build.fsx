// --------------------------------------------------------------------------------------
// A simple FAKE build script that:
//  1) Hosts Suave server locally & reloads web part that is defined in 'app.fsx'
//  2) Deploys the web application to Azure web sites when called with 'build deploy'
// 
// Source: https://github.com/tpetricek/suave-xplat-gettingstarted/blob/master/build.fsx
// Copied From: https://gist.github.com/mikesigs/72875bc7408af9202349#file-build-fsx-L65
// --------------------------------------------------------------------------------------
#r "packages/FSharp.Compiler.Service/lib/net45/FSharp.Compiler.Service.dll"
#r "packages/Suave/lib/net40/Suave.dll"
#r "packages/FAKE/tools/FakeLib.dll"
#load "app.fsx"
open Fake
open System
open System.IO
open Suave
open Suave.Http
open Suave.Web
open Suave.Operators
open Suave.Tcp
open Suave.Sockets
open Suave.Sockets.Control
open Suave.WebSocket
open Microsoft.FSharp.Compiler.Interactive.Shell

// --------------------------------------------------------------------------------------
// The following uses FileSystemWatcher to look for changes in 'app.fsx'. When
// the files changes, we run `#load "app.fsx"` using the F# Interactive service
// and then get the `App.app` value (top-level value defined using `let app = ...`).
// The loaded WebPart is then hosted at localhost:8083
// --------------------------------------------------------------------------------------

let sbOut = Text.StringBuilder()
let sbErr = Text.StringBuilder()

let fsiSession =
    let inStream = new StringReader("")
    let outStream = new StringWriter(sbOut)
    let errStream = new StringWriter(sbErr)
    let fsiConfig = FsiEvaluationSession.GetDefaultConfiguration()
    let argv = Array.append [|"/fake/fsi.exe"; "--quiet"; "--noninteractive"; "-d:DO_NOT_START_SERVER"|] [||]
    FsiEvaluationSession.Create (fsiConfig, argv, inStream, outStream, errStream)

let refreshEvent = new Event<_>()

let reportFsiError (e:exn) =
    traceError "Reloading app.fsx script failed."
    traceError (sprintf "Message: %s\nError: %s" e.Message (sbErr.ToString().Trim()))
    sbErr.Clear() |> ignore

let byteResponse (str:string) =
    str
    |> System.Text.Encoding.ASCII.GetBytes
    |> ByteSegment

let socketHandler (webSocket : WebSocket) =
  fun cx -> socket {
    while true do
      let! refreshed =
        Control.Async.AwaitEvent(refreshEvent.Publish)
        |> Suave.Sockets.SocketOp.ofAsync
        
      do! webSocket.send Text (byteResponse "refreshed") true
  }
let reloadScript _ =
    try
        traceImportant "Reloading..."
        let appFsx = __SOURCE_DIRECTORY__  + "\\app.fsx"
        fsiSession.EvalInteraction (sprintf "#load @\"%s\"" appFsx)
        fsiSession.EvalInteraction ("open App")
        match fsiSession.EvalExpression("app") with
        | Some app -> Some(app.ReflectionValue :?> WebPart)
        | None -> failwith "Couldn't get 'app' value"
    with e -> reportFsiError e; None
    
// --------------------------------------------------------------------------------------
// Suave server that redirects all request to currently loaded version
// --------------------------------------------------------------------------------------

let currentApp = ref (fun _ -> async { return None })

let serverConfig =
    { defaultConfig with
        homeFolder = Some __SOURCE_DIRECTORY__
        serverKey = System.Convert.FromBase64String "ABk4PEzueMTzt4oMbWRThJtDkeTGwHsg2wipDykS7N0="
        bindings = [ HttpBinding.create Protocol.HTTP Net.IPAddress.Loopback 8083us ]
    }
    

let reloadAppServer _ =
    reloadScript() |> Option.iter (fun app ->
        currentApp.Value <- app
        refreshEvent.Trigger()
        traceImportant "New version of app.fsx loaded!")

let startSuave() =
    // Start Suave on localhost
    let appOriginal ctx = currentApp.Value ctx
    let app = 
        choose [
            appOriginal
            Filters.path "/websocket" >=> handShake socketHandler
        ]
    let _, server = startWebServerAsync serverConfig app
    reloadAppServer()
    Async.Start(server)

let openBrowser() =
    // Open web browser with the loaded file
    System.Diagnostics.Process.Start("http://localhost:8083") |> ignore

let watch() =
    // Watch for changes & reload when files change
    use watcher = 
        !! (__SOURCE_DIRECTORY__ + "\\*.*")
        -- "\\*.git"
        -- "\\*.notes"
        -- "\\*.js"  // I use chrome to hot-reload js files
        -- "\\*.css" // I use chrome to hot-reload css files
        |> WatchChanges (fun t -> 
            let x = Seq.tryHead t
            if Seq.length t > 1 then
                traceImportant("!!Multiple files changed!!" + " at :" + System.DateTime.Now.ToShortTimeString())
            if x.IsSome then
                traceImportant ("File Changed: " + x.Value.Name + " at :" + System.DateTime.Now.ToShortTimeString())
            else
                traceImportant ("No file changed" + " at :" + System.DateTime.Now.ToShortTimeString())
            reloadAppServer())
    traceImportant "Waiting for app.fsx edits. Press any key to stop."
    System.Console.ReadLine() |> ignore

Target "run" (fun _ ->
    startSuave()
    openBrowser()
    watch()
)

Target "Default" DoNothing

RunTargetOrDefault "run"