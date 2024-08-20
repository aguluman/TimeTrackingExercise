namespace Rental

open System
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open FsToolkit.ErrorHandling
open Rental.Booking
open Shared.Types


[<ApiController>]
[<Authorize(AuthenticationSchemes = "FakeAuthenticationScheme")>]
[<Route("rental")>]
type RentalApiController(facade: RentalFacade) =
    inherit ControllerBase()

    [<HttpGet>]
    [<Route("bikes/{userId}")>]
    member self.GetAllBikes()([<FromRoute>] userId) =
        facade.GetAllBookableBikes (UserId userId)

    [<HttpPost>]
    [<Route("rent")>]
    member self.Rent([<FromBody>] data) =
        asyncResult { do! facade.RentBike (Guid.NewGuid() |> BookingId) data }

    [<HttpPost>]
    [<Route("release")>]
    member self.Release([<FromBody>] data) =
        asyncResult { do! facade.ReleaseBike data }
