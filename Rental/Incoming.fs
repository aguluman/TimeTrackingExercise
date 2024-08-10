namespace Rental

open Rental.Bike
open Rental.Booking

type RentalStorages =
    { Bikes: BikeStorage
      BookingEvents: BookingEventStorage }

type SqlContext = { ConnectionString: string }

type RentalStorageContext =
    | InMemory
    | Mixed of BikeJsonStorage.JsonContext
    | Sql of SqlContext

module RentalStorageCreator =
    let create (ctx: RentalStorageContext) =
        match ctx with
        | Mixed json ->
            { Bikes = BikeJsonStorage.create json
              BookingEvents = BookingEventInMemoryStorage.create () }
        | _ -> failwith "not implemented" //TODO: Implement any Relational Sql storage

type RentalServices =
    { GetNodaInstant: unit -> NodaTime.Instant
      WithdrawAmount: Amount -> UserId -> Async<bool> }
