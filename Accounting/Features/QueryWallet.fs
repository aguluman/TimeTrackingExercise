namespace Accounting.Features

open Accounting
open Accounting.Wallet
open FsToolkit.ErrorHandling

[<RequireQualifiedAccess>]
module QueryWallet =
    let query (getEventsByUserId: UserId -> Async<WalletEvent list>) userId =
        asyncResult {
            let! events = getEventsByUserId userId

            return!
                Wallet.project events
                |> Result.requireSome AccountingError.WalletNotFound
        }