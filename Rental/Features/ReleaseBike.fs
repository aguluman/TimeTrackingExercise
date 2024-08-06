﻿namespace Rental.Features

open System
open Rental
open Rental.Booking
open FsToolkit.ErrorHandling

module ReleaseBike =
    type Data = { BookingId: BookingId }

    let execute
        (persistBookingEvent: BookingEvent -> Async<unit>)
        (queryBookingEvents: BookingId -> Async<BookingEvent list>)
        (getInstant: unit -> Instant)
        (data: Data)
        =
        asyncResult {
            let! bookingEvents = queryBookingEvents data.BookingId

            let! booking =
                Booking.projectSingle bookingEvents
                |> Result.requireSome RentalErrors.BookingNotFound

            do! booking.End |> Result.requireNone RentalErrors.BikeAlreadyReleased

            do!
                persistBookingEvent
                    { BookingEvent.BookingId = data.BookingId
                      EventId = Guid.NewGuid()
                      BikeId = booking.BikeId
                      Data = Released
                      Instant = getInstant () }
        }
