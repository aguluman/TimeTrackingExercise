namespace Accounting

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Authorization

[<ApiController>]
[<Authorize(AuthenticationSchemes = "FakeAuthenticationScheme")>]
[<Route("accounting")>]
type AccountingController() =
    inherit ControllerBase()

    [<HttpGet>]
    [<Route("test")>]
    member self.Get() = [| 1; 2; 3 |]
