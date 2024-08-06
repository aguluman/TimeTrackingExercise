namespace Rental.Booking

open System
open Rental
open Rental.Bike

type BookingId = BookingId of Guid
type UserId = UserId of Guid

type BookingEventData =
    | Booked of UserId
    | Released

type BookingEvent =
    { BookingId: BookingId
      EventId: Guid
      BikeId: BikeId
      Data: BookingEventData
      Instant: Instant }
