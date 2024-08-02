namespace Starter

#nowarn "20"

open System.Text.Json.Serialization
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Mvc.ApplicationParts
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Registration
open Accounting
open Starter.FakeAuthentication

module Program =
    [<EntryPoint>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)

        builder.Services
            .AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, FakeAuthenticationHandler>(FakeSchemeName, (fun _ -> ()))
        |> ignore

        builder.Services.AddAuthorization |> ignore

        let registrationAssemblyPart = typeof<RegistrationFacade>.Assembly |> AssemblyPart
        let accountingAssemblyPart = typeof<AccountingId>.Assembly |> AssemblyPart

        let parts =
            builder.Services
                .AddControllers()
                .AddJsonOptions(fun options -> options.JsonSerializerOptions.Converters.Add(JsonFSharpConverter()))
                .PartManager.ApplicationParts

        parts.Add(registrationAssemblyPart)
        parts.Add(accountingAssemblyPart)

        let facades = FacadesCreator.create builder.Configuration

        builder.Services
            .AddSingleton<RegistrationFacade>(fun _ -> facades.Registration)
            .AddSingleton<AccountingFacade>(fun _ -> facades.Accounting)
        |> ignore

        let app = builder.Build()

        if app.Environment.IsDevelopment() then
            app.UseDeveloperExceptionPage() |> ignore

        app.UseHttpsRedirection()
        app.UseRouting()
        app.UseAuthentication()
        app.UseAuthorization()

        app.MapControllers()

        app.Run()

        0
