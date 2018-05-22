namespace Watchman

type Watcher =
    abstract member WatchUrl : string -> unit
    abstract member WatchedUrls : unit -> string Set

module Watcher =

    [<Struct>]
    type Message =
        | Add of Url : string
        | ViewAll of channel : AsyncReplyChannel<string Set>

    let agent = MailboxProcessor<Message>.Start(fun inbox ->
        let rec loop (urls : string Set) = async {
            let! message = inbox.Receive ()
            match message with
            | Add url ->
                return! loop (Set.add url urls)
            | ViewAll c ->
                c.Reply urls
                return! loop urls
        }

        loop Set.empty
    )

    let make () =
        { new Watcher with
            member __.WatchUrl url = agent.Post(Add url)
            member __.WatchedUrls () = agent.PostAndReply (fun c -> ViewAll c)
        }