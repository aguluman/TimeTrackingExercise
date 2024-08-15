module Tests.Rental.InstantFacts

open Tests
open Xunit
open Swensen.Unquote

[<Fact>]
let ``is later`` () =
    let a = "2024-08-15 06:00:01" |> Instant.parse
    let b = "2024-08-15 06:00:00" |> Instant.parse

    a > b =! true
    a < b =! false
    a >= b =! true

[<Fact>]
let ``is earlier`` () =
    let a = "2024-08-15 06:00:00" |> Instant.parse
    let b = "2024-08-15 06:00:01" |> Instant.parse

    a < b =! true
    a > b =! false
    a <= b =! true

[<Fact>]
let ``is equal`` () =
    let a = "2024-08-15 06:00:00" |> Instant.parse
    let b = "2024-08-15 06:00:00" |> Instant.parse

    a = b =! true
