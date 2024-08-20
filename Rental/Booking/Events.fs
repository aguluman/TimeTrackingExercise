﻿namespace Rental.Booking

open System
open Rental.Bike
open Shared.Types

type BookingId = BookingId of Guid

type BookingEventData =
    | Booked of UserId
    | Released

type BookingEvent =
    { BookingId: BookingId
      EventId: Guid
      BikeId: BikeId
      Data: BookingEventData
      Instant: Instant }
