namespace Rental

open Rental.Bike

type RentalStorages = { Bikes: BikeStorage }

type SqlContext = { ConnectionString: string }

type RentalStorageContext =
    | Json of BikeJsonStorage.JsonContext
    | Sql of SqlContext

module RentalStorageCreator =
    let create (ctx: RentalStorageContext) =
        match ctx with
        | Json json -> { Bikes = BikeJsonStorage.create json }
        | _ -> failwith "not implemented" //TODO: Implement any Relational Sql storage

type RentalServices =
    { GetNodaInstant: unit -> NodaTime.Instant }
