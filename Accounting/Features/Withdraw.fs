﻿namespace Accounting.Features

open System
open Accounting
open Accounting.Wallet
open FsToolkit.ErrorHandling

[<RequireQualifiedAccess>]
module Withdraw =
    type Data = { UserId: UserId; Amount: Amount }

    let execute
        (getEventsByUserId: UserId -> Async<WalletEvent list>)
        (persistWalletEvent: WalletEvent -> Async<unit>)
        (getInstant: unit -> Instant)
        (triggerUIChange: UserId -> unit)
        (data: Data)
        =
        asyncResult {
            let! events = getEventsByUserId data.UserId

            let! wallet =
                Wallet.project
                    events
                |> Result.requireSome
                       AccountingError.WalletNotFound

            do!
                persistWalletEvent
                    { WalletEvent.WalletId = wallet.WalletId
                      UserId = data.UserId
                      EventId = Guid.NewGuid()
                      Data = WalletEventData.Withdrawn data.Amount
                      Instant = getInstant () }

            triggerUIChange data.UserId
        }
