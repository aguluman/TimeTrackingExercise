namespace Accounting

open Accounting.AccountingStorageCreator
open Accounting.Features
open Accounting.Wallet


type AccountingFacade(services: AccountingServices, storages: AccountingStorages, uiChanged: string -> UserId -> unit) =
    let getInstant = services.GetNodaInstant >> Instant

    member self.CreateWallet =
        CreateWallet.execute storages.WalletEvents.PersistEvent getInstant

    member self.Deposit =
        Deposit.execute
            storages.WalletEvents.QueryByUserId
            storages.WalletEvents.PersistEvent
            getInstant
            (uiChanged "accounting")

    member self.GetWallet = QueryWallet.query storages.WalletEvents.QueryByUserId
