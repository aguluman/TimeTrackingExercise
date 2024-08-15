namespace Starter

open System
open Shared.Types

module Services =
    let getInstant () =
        DateTime.UtcNow 
        |> NodaTime.Instant.FromDateTimeUtc 
        |> Instant