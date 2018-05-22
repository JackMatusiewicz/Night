namespace Night

open System
open System.Runtime.InteropServices;

[<Struct>]
type ExitMethod =
    | LogOff
    | ShutDown

[<Struct>]
type ControlMethod =
    | LockMachine
    | ExitMachine of ExitMethod : ExitMethod

module Control =

    [<DllImport("user32.dll", SetLastError = true)>]
    extern [<MarshalAs(UnmanagedType.Bool)>] bool ExitWindowsEx(uint32 uFlags, uint32 dwReason);

    [<DllImport("user32.dll", SetLastError = true)>]
    extern [<MarshalAs(UnmanagedType.Bool)>] bool LockWorkStation();

    let toUint (method : ExitMethod) =
        match method with
        | LogOff -> 0x00
        | ShutDown -> 0x01

    let exit (method : ExitMethod) =
        let userDefinedShutdown = (uint32 0x40000000)
        let methodUint = toUint method
        let forceMethod = methodUint ||| 0x04
        ExitWindowsEx ((uint32 forceMethod), userDefinedShutdown)

    let exitIfUserPresent (method : ControlMethod) (user : string) =
        let currentUser = Environment.UserName
        match currentUser.Contains user with
        | false -> false
        | true ->
            match method with
            | LockMachine -> LockWorkStation ()
            | ExitMachine method -> exit method