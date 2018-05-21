open Night

[<EntryPoint>]
let main argv = 
    printfn "Press enter to log off"
    System.Console.ReadLine ()
    Control.exit LogOff
    0 // return an integer exit code
