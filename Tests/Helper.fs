﻿namespace Tests

open Shared.Types

[<RequireQualifiedAccess>]
module Instant =
    let parse s =
        s
        |> System.DateTime.Parse
        |> (fun x -> x.ToUniversalTime())
        |> NodaTime.Instant.FromDateTimeUtc
        |> Instant
