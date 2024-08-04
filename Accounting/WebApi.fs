namespace Accounting

open System
open Accounting.Wallet
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Authorization

[<ApiController>]
[<Authorize(AuthenticationSchemes = "FakeAuthenticationScheme")>]
[<Route("accounting")>]
type AccountingController(facade: AccountingFacade) =
    inherit ControllerBase()

    [<HttpGet>]
    [<Route("wallet/{userId}")>]
    member self.GetWallet([<FromRoute>] userId: Guid) = facade.GetWallet (UserId userId)

    [<HttpPost>]
    [<Route("wallet/deposit")>]
    member self.Deposit([<FromBody>] data) = facade.Deposit data

