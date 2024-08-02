namespace Accounting

type AccountingStorages = { WalletEvents: WalletEventStorage }

type SqlContext = { ConnectionString: string }

type AccountingStorageContext =
    | InMemory
    | Sql of SqlContext

module AccountingStorageCreator =
    let create (ctx: AccountingStorageContext) =
        match ctx with
        | InMemory -> { WalletEvents = WalletEventInMemoryStorage.create () }
        | _ -> failwith "not implemented yet" //TODO: Implement any Relational Sql storage

    type AccountingServices = {
        GetNodaInstant: unit -> NodaTime.Instant
    }