#r "./packages/Suave/lib/net40/Suave.dll"
#r "./packages/Suave.Experimental/lib/net40/Suave.Experimental.dll"

namespace Suave.Helpers

[<AutoOpenAttribute>]
module t =
    open Suave
    open Suave.Cookie
    open Suave.Successful
    open Suave.RequestErrors
    open Suave.Authentication

    let requireAuth =
        authenticate Session false
           (fun () -> Choice2Of2(FORBIDDEN "please authenticate"))
           (fun _ ->  Choice2Of2(BAD_REQUEST "did you fiddle with our cipher text?"))