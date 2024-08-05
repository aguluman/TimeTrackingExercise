﻿namespace Rental

open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open FsToolkit.ErrorHandling

[<ApiController>]
[<Authorize(AuthenticationSchemes = "FakeAuthenticationScheme")>]
[<Route("rental")>]
type RegistrationApiController(facade: RentalFacade) =
    inherit ControllerBase()

    [<HttpGet>]
    [<Route("bikes")>]
    member self.GetAllBikes() = facade.GetAllBikes()
