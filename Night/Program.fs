open Night
open Suave
open Suave.Web
open Suave.Http
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.Cookie

[<EntryPoint>]
let main _ =
    let app = choose [
            pathScan "/logoff/users/%s" ((Control.exitIfUserPresent LogOff) >> (fun s -> OK <| s.ToString()))
            pathScan "/shutdown/users/%s" ((Control.exitIfUserPresent ShutDown) >> (fun s -> OK <| s.ToString()))
        ]
    startWebServer defaultConfig app
    0
