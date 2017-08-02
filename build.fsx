#r "./packages/FAKE/tools/FakeLib.dll"
open Fake

let runServer () =
    fireAndForget (fun startInfo ->
        startInfo.WorkingDirectory <- "suaveExample"
        startInfo.FileName <- FSIHelper.fsiPath
        startInfo.Arguments <- "--define:RELOAD app.fsx")

use watcher = !! "suaveExample/*.fsx" |> WatchChanges (fun changes ->
  tracefn "%A" changes
  killAllCreatedProcesses()
  runServer()
)
runServer()