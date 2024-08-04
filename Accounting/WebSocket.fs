namespace Accounting

open System
open System.Net.WebSockets
open System.Text
open System.Threading
open Accounting.FSharp.Control.Tasks

module WebSocket =
    let sendWebSocketMessageOnEvent (webSocket: WebSocket) (eventStream: IEvent<Guid * string>) filterGuid =
        task {
            let rec waitForEvent () =
                task {
                    printfn "waiting"
                    let! id, msg = Async.AwaitEvent eventStream
                    printfn "changed happened"

                    if id = filterGuid then
                        printfn "correct id"
                        let serverMsg = Encoding.UTF8.GetBytes msg

                        do!
                            webSocket.SendAsync(
                                ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None
                            )

                        printfn "Message Sent"

                        do! waitForEvent ()
                    else
                        printfn "wrong id"
                        do! waitForEvent ()
                }

            do! waitForEvent ()
        }
