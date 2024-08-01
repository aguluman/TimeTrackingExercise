namespace Starter

open System
open System.Security.Claims
open Microsoft.AspNetCore.Authentication
open FsToolkit.ErrorHandling
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Options

module FakeAuthentication =
    let FakeSchemeName = "FakeAuthenticationScheme"

    //This commented code block below was showing this hint.
    //ISystemClock is obsolete, use TimeProvider on AuthenticationSchemeOptions instead.
    (*type FakeAuthenticationHandler(options, logger, encoder, clock) =
        inherit AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder, clock)
    *)
    type FakeAuthenticationHandler
        (options: IOptionsMonitor<AuthenticationSchemeOptions>, logger, encoder, timeProvider: TimeProvider) =
        inherit AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)

        member this.TimeProvider = timeProvider

        override this.HandleAuthenticateAsync() =
            let inner (request: HttpRequest) =
                result {
                    let encodedTokenOption =
                        request.Headers
                        |> Seq.tryFind (fun x -> x.Key = "Authorization")
                        |> Option.map (fun x -> x.Value.Item 0)

                    return
                        match encodedTokenOption with
                        | Some encodedToken ->
                            if encodedToken.StartsWith("thisIsAFakeToken+") then
                                let parts = encodedToken.Split('+')

                                let identity =
                                    ClaimsIdentity([ Claim("Email", Array.get parts 1) ], this.Scheme.Name)

                                let principal = System.Security.Principal.GenericPrincipal(identity, null)
                                let ticket = AuthenticationTicket(principal, this.Scheme.Name)
                                AuthenticateResult.Success(ticket)
                            else
                                AuthenticateResult.Fail("token not valid")
                        | None -> AuthenticateResult.NoResult()
                }

            let result = inner this.Request

            let authResult =
                match result with
                | Ok authResult -> authResult
                | Error _ -> AuthenticateResult.NoResult()

            authResult |> Task.singleton
