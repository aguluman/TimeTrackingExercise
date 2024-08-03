namespace Accounting

open System
open System.Net.WebSockets
open System.Text
open System.Threading
open Accounting.FSharp.Control.Tasks

module WebSocket =
    let sendWebSocketMessageOnEvent (webSocket: WebSocket) eventStream =
        task {
            let rec waitForEvent () =
                task {
                    printfn "Waiting"
                    let! _ = Async.AwaitEvent eventStream
                    printfn "Changed happened"
                    let serverMsg = Encoding.UTF8.GetBytes "change"

                    do!
                        webSocket.SendAsync(
                            ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None
                        )

                    printfn "Message Sent"

                    do! waitForEvent ()
                }

            do! waitForEvent ()
        }
