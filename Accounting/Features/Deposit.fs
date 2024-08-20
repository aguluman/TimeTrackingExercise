namespace Accounting.Features

open System
open Accounting
open Accounting.Wallet
open Shared.Types
open FsToolkit.ErrorHandling

[<RequireQualifiedAccess>]
module Deposit =
    type DataForDeposit = { UserId: UserId; Amount: Amount }

    let execute
        (getEventsByUserId: UserId -> Async<WalletEvent list>)
        (persistWalletEvent: WalletEvent -> Async<unit>)
        (getInstant: unit -> Instant)
        (triggerUiChange: WalletId -> unit)
        (data: DataForDeposit)
        =
        asyncResult {
            let! events = getEventsByUserId data.UserId

            let! wallet = Wallet.project events |> Result.requireSome AccountingError.WalletNotFound

            do!
                persistWalletEvent
                    { WalletEvent.WalletId = wallet.WalletId
                      UserId = data.UserId
                      EventId = Guid.NewGuid()
                      Data = WalletEventData.Deposited data.Amount
                      Instant = getInstant () }

            triggerUiChange wallet.WalletId
        }
