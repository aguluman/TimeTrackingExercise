module Tests.Rental.Booking.StatusComputation

open Rental.Booking
open Shared.Types
open Tests
open Xunit
open Swensen.Unquote

[<Fact>]
let ``Empty list`` () =
    let result = Booking.getStatusOfBike (Example.create ()) (Example.create ()) []

    result =! Bookable

[<Fact>]
let ``Unreleased booking before`` () =
    let queryInstant = "2024-08-10 10:20"
                       |> NodaInstant.parse
                       |> Instant

    let booking =
        { Example.create<Booking> () with
              Start = "2024-08-10 10:10"
                      |> NodaInstant.parse
                      |> Instant
              End = None }

    let result = Booking.getStatusOfBike queryInstant (Example.create ()) [ booking ]

    result =! NotAvailable

[<Fact>]
let ``Unreleased booking before from same user`` () =
    let queryInstant = " 2024-08-14 11:10 " |> NodaInstant.parse |> Instant

    let booking = 
        { Example.create<Booking> () with 
              Start = " 2024-08-14 10:10 " |> NodaInstant.parse |> Instant
              End = None }

    let result = Booking.getStatusOfBike queryInstant booking.UserId [ booking ]

    result =! Releasable booking.BookingId

[<Fact>]
let ``Unreleased booking after`` () =
    let queryInstant = "2024-08-10 08:00"
                       |> NodaInstant.parse
                       |> Instant

    let booking =
        { Example.create<Booking> () with
              Start = "2024-08-10 10:00"
                      |> NodaInstant.parse
                      |> Instant
              End = None }

    let result = Booking.getStatusOfBike queryInstant (Example.create ()) [ booking ]

    result =! Bookable

[<Fact>]
let ``Released booking before`` () =
    let queryInstant = "2024-08-10 10:00"
                       |> NodaInstant.parse
                       |> Instant

    let booking =
        { Example.create<Booking> () with
              Start = "2024-08-10 08:00" |> NodaInstant.parse |> Instant
              End =
                  "2024-08-10 08:59"
                  |> NodaInstant.parse
                  |> Instant
                  |> Some }

    let result = Booking.getStatusOfBike queryInstant (Example.create ()) [ booking ]

    result =! Bookable

[<Fact>]
let ``Released booking in future`` () =
    let queryInstant = "2024-08-10 08:00"
                       |> NodaInstant.parse
                       |> Instant

    let booking =
        { Example.create<Booking> () with
              Start = "2024-08-10 08:00"
                      |> NodaInstant.parse
                      |> Instant
              End =
                  "2024-08-10 10:01"
                  |> NodaInstant.parse
                  |> Instant
                  |> Some }

    let result = Booking.getStatusOfBike queryInstant (Example.create()) [ booking ]

    result =! NotAvailable

[<Fact>]
let ``Released booking with unreleased booking. Both before`` () =
    let queryInstant = "2024-08-10 10:30"
                       |> NodaInstant.parse
                       |> Instant

    let booking1 =
        { Example.create<Booking> () with
              Start = "2024-08-10 07:30"
                      |> NodaInstant.parse
                      |> Instant
              End =
                  "2024-08-10 08:30"
                  |> NodaInstant.parse
                  |> Instant
                  |> Some }

    let booking2 =
        { booking1 with
              Start = "2024-08-10 09:00"
                      |> NodaInstant.parse
                      |> Instant
              End = None }

    let result = Booking.getStatusOfBike queryInstant (Example.create ()) [ booking1; booking2 ]

    result =! NotAvailable