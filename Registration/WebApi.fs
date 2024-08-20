namespace Registration

open System
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open FsToolkit.ErrorHandling
open Registration.User.Events
open Shared.Types
open Swashbuckle.AspNetCore.Annotations

/// <summary>
/// Controller for handling user registration.
/// </summary>
[<ApiController>]
[<AllowAnonymous>]
[<Route("registration")>]
type RegistrationApiController(facade: RegistrationFacade) =
    inherit ControllerBase()

    /// <summary>
    /// Starts the registration process.
    /// </summary>
    /// <param name="data">The User Registration data.</param>
    [<HttpPost>]
    [<Route("start")>]
    [<SwaggerOperation(Summary = "Starts the registration process")>]
    [<SwaggerResponse(StatusCodes.Status200OK, "Registration started successfully")>]
    [<SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data")>]
    [<SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")>]
    member self.Start([<FromBody>] data) =
        asyncResult {
            do!
                facade.StartRegistration
                    (Guid.NewGuid() |> UserId)
                    (Guid.NewGuid() |> RegistrationCompletionId) data
        }

    /// <summary>
    /// Verifies the phone number.
    /// </summary>
    /// <param name="data">The phone verification data.</param>
    [<HttpGet>]
    [<Route("verify")>]
    [<SwaggerOperation(Summary = "Verifies the phone number")>]
    [<SwaggerResponse(StatusCodes.Status200OK, "Phone number verified successfully")>]
    [<SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data")>]
    [<SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")>]
    member self.Verify([<FromBody>] data) =
        asyncResult { return! facade.VerifyPhone data }

    /// <summary>
    /// Completes the registration process.
    /// </summary>
    /// <param name="data">The completion data.</param>
    [<HttpPost>]
    [<Route("complete")>]
    [<SwaggerOperation(Summary = "Completes the registration process")>]
    [<SwaggerResponse(StatusCodes.Status200OK, "Registration completed successfully")>]
    [<SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data")>]
    [<SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")>]
    member self.Start([<FromBody>] data) =
        asyncResult { do! facade.CompleteRegistration data }

    /// <summary>
    /// Begins the login process.
    /// </summary>
    /// <param name="data">The login data.</param>
    [<HttpPost>]
    [<Route("login")>]
    [<SwaggerOperation(Summary = "Begins the login process")>]
    [<SwaggerResponse(StatusCodes.Status200OK, "Login successful")>]
    [<SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data")>]
    [<SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")>]
    member self.Start([<FromBody>] data) =
        asyncResult { return! facade.CreateToken data }

    /// <summary>
    /// Fetches initials from Emails.
    /// </summary>
    /// <param name="initialsFromEmail">The initials from email.</param>
    [<HttpGet>]
    [<Route("initials/{initialsFromEmail}")>]
    [<SwaggerOperation(Summary = "Fetches initials from Emails")>]
    [<SwaggerResponse(StatusCodes.Status200OK, "Initials fetched successfully")>]
    [<SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid email")>]
    [<SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")>]
    member self.GetUser([<FromRoute>] initialsFromEmail) =
        facade.GetInitials(Email initialsFromEmail)
