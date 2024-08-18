﻿namespace Rental.Bike

open System
open Shared.Types

type BikeId = BikeId of Guid

type Bike =
    { BikeId: BikeId
      Name: string
      Manufacturer: string
      Price: Price
      Base64Image: string }
