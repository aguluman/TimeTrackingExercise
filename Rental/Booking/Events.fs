namespace Rental.Booking

open System
open Rental.Bike
open Shared.Types

type BookingId = BookingId of Guid
type UserIdForBooking = UserIdForBooking of Guid

type BookingEventData =
    | Booked of UserIdForBooking
    | Released

type BookingEvent =
    { BookingId: BookingId
      EventId: Guid
      BikeId: BikeId
      Data: BookingEventData
      Instant: Instant }
