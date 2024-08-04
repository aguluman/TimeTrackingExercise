namespace Accounting

open System
open Accounting.AccountingStorageCreator
open Accounting.Features
open Accounting.Wallet


type AccountingFacade(services: AccountingServices, storages: AccountingStorages, uiChanged: Event<Guid * string>) =
    let getInstant = services.GetNodaInstant >> Instant

    let triggerAccountingUiChange (UserId userId) = uiChanged.Trigger(userId, "accounting")

    member self.CreateWallet =
        CreateWallet.execute storages.WalletEvents.PersistEvent getInstant

    member self.Deposit =
        Deposit.execute
            storages.WalletEvents.QueryByUserId
            storages.WalletEvents.PersistEvent
            getInstant
            triggerAccountingUiChange

    member self.GetWallet = QueryWallet.query storages.WalletEvents.QueryByUserId

    member self.UiChanged = uiChanged.Publish
