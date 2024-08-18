﻿namespace Rental.Booking

open Rental.Bike
open Shared.Types

type Booking =
    { BookingId: BookingId
      BikeId: BikeId
      UserId: UserIdForBooking
      Start: Instant
      End: Instant option }

type AvailabilityStatus =
    | Bookable
    | NotAvailable
    | Releasable of BookingId

[<RequireQualifiedAccess>]
module Booking =
    let projectSingle (events: BookingEvent list) =
        events
        |> List.sortBy (fun x -> x.Instant)
        |> List.fold
            (fun bookingOption event ->
                match bookingOption, event.Data with
                | _, Booked userId ->
                    Some
                        { BookingId = event.BookingId
                          UserId = userId
                          Start = event.Instant
                          BikeId = event.BikeId
                          End = None }
                | Some booking, Released ->
                    Some
                        { booking with
                            End = Some event.Instant }
                | _, _ -> None)
            None

    let projectMultiple (events: BookingEvent list) =
        events
        |> List.groupBy (fun x -> x.BookingId)
        |> List.map (fun (_, g) -> g |> projectSingle)
        |> List.choose id

    let getStatusOfBike (instant: Instant) userId (bookingsOfBike: Booking list) =
        let unreleasedBookings =
            bookingsOfBike
            |> List.filter (fun b -> b.Start <= instant)
            |> List.filter (fun b ->
                match b.End with
                | None -> true
                | Some x -> x >= instant)

        let hasUnreleasedBookings = unreleasedBookings |> List.isEmpty |> not

        match hasUnreleasedBookings with
        | true -> 
            let lastBooking = unreleasedBookings |> List.last

            match lastBooking.UserId = userId with
            | true -> Releasable lastBooking.BookingId
            | false -> NotAvailable
        | false -> Bookable
