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
    let showResult s = s.ToString () |> OK

    let app = choose [
            pathScan "/logoff/users/%s"
                ((Control.exitIfUserPresent (ExitMachine LogOff)) >> showResult)
            pathScan "/shutdown/users/%s"
                ((Control.exitIfUserPresent (ExitMachine ShutDown)) >> showResult)
            pathScan "/lock/users/%s"
                ((Control.exitIfUserPresent LockMachine) >> showResult)
        ]

    startWebServer defaultConfig app
    0
