namespace Accounting.Features

open Accounting
open Accounting.Wallet
open Shared.Types
open FsToolkit.ErrorHandling

[<RequireQualifiedAccess>]
module QueryWallet =
    let queryByUser (getEventsByUserId: UserId -> Async<WalletEvent list>) userId =
        asyncResult {
            let! events = getEventsByUserId userId

            return!
                Wallet.project events
                |> Result.requireSome AccountingError.WalletNotFound
        }

    let queryByWalletId (getEvents: WalletId -> Async<WalletEvent list>) walletId =
        asyncResult {
            let! events = getEvents walletId

            return!
                Wallet.project events
                |> Result.requireSome AccountingError.WalletNotFound
        }