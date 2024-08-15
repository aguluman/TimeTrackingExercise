namespace Rental.Features

open System
open Rental
open Rental.Bike
open Rental.Booking
open Shared.Types
open FsToolkit.ErrorHandling

[<RequireQualifiedAccess>]
module RentBike =
    type Data = { BikeId: BikeId; UserId: UserId }

    let execute
        (persistBookingEvent: BookingEvent -> Async<unit>)
        (queryBookingEventsOfBike: BikeId -> Async<BookingEvent list>)
        (queryBike: BikeId -> Async<Bike option>)
        (withdrawAmount: Amount -> UserId ->  Async<bool>)
        (triggerUiChanged: unit -> unit)
        (getInstant: unit -> Instant)
        bookingId
        (data: Data)
        =
        asyncResult {
            let! bookingEventsOfBike = queryBookingEventsOfBike data.BikeId
            let bookings = Booking.projectMultiple bookingEventsOfBike

            let instant = getInstant ()

            do!
                Booking.getStatusOfBike instant data.UserId bookings
                |> (fun x -> x = Bookable)
                |> Result.requireTrue RentalErrors.BikeAlreadyBooked

            let! bike =
                queryBike data.BikeId
                |> Async.map (Result.requireSome RentalErrors.BikeNotFound)

            let amount = bike.Price |> (fun (Price p) -> Amount p)

            do!
                withdrawAmount amount data.UserId
                |> Async.map(Result.requireTrue RentalErrors.UserBalanceNotSufficient)

            do!
                persistBookingEvent
                    { BookingEvent.BookingId = bookingId
                      EventId = Guid.NewGuid()
                      BikeId = data.BikeId
                      Data = Booked data.UserId
                      Instant = instant }

            do triggerUiChanged ()
        }
