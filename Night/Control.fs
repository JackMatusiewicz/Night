namespace Night

open System
open System.Runtime.InteropServices;

type ExitMethod =
    | LogOff
    | ShutDown

module Control =

    [<DllImport("user32.dll", SetLastError = true)>]
    extern [<MarshalAs(UnmanagedType.Bool)>] bool ExitWindowsEx(uint32 uFlags, uint32 dwReason);

    let toUint (method : ExitMethod) =
        match method with
        | LogOff -> 0x00
        | ShutDown -> 0x01

    let exit (method : ExitMethod) =
        let userDefinedShutdown = (uint32 0x40000000)
        let methodUint = toUint method
        let forceMethod = methodUint ||| 0x04
        ExitWindowsEx ((uint32 forceMethod), userDefinedShutdown)

    let exitIfUserPresent (method : ExitMethod) (user : string) =
        let currentUser = Environment.UserName
        match currentUser.Contains user with
        | false -> false
        | true ->
            exit method