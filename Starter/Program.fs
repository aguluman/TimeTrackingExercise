namespace Starter

#nowarn "20"

open System.Text.Json.Serialization
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Mvc.ApplicationParts
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.OpenApi.Models
open Registration
open Accounting
open Rental
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
        builder.Services.AddCors(fun options ->
            options.AddPolicy("AllowSpecificOrigin", fun builder ->
                builder.WithOrigins("")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials() |> ignore
            )
        ) |> ignore

        let registrationAssemblyPart = typeof<RegistrationFacade>.Assembly |> AssemblyPart
        let accountingAssemblyPart = typeof<AccountingId>.Assembly |> AssemblyPart

        let parts =
            builder.Services
                .AddControllers()
                .AddJsonOptions(fun options -> options.JsonSerializerOptions.Converters.Add(JsonFSharpConverter()))
                .PartManager.ApplicationParts

        parts.Add(registrationAssemblyPart)
        parts.Add(accountingAssemblyPart)

        let uiChangedEvent, facades = FacadesCreator.create builder.Configuration

        builder.Services
            .AddSingleton<Event<string * obj>>(fun _ -> uiChangedEvent)
            .AddSingleton<RegistrationFacade>(fun _ -> facades.Registration)
            .AddSingleton<AccountingFacade>(fun _ -> facades.Accounting)
            .AddSingleton<RentalFacade>(fun _ -> facades.Rental)
        |> ignore

        // Add Swagger services
        builder.Services.AddSwaggerGen(fun c ->
            c.SwaggerDoc("v1", OpenApiInfo(Title = "My Bike-Rental API", Version = "v1"))
        )
        let eventStream =
            builder.Services.BuildServiceProvider().GetService<Event<string * obj>>()

        let app = builder.Build()

        if app.Environment.IsDevelopment() then
            app.UseDeveloperExceptionPage() |> ignore

        app.UseHttpsRedirection()
        app.UseRouting()
        app.UseAuthentication()
        app.UseAuthorization()
        app.UseWebSockets()
        app.MapControllers()
        app.Use(WebSocket.wsMiddleware eventStream.Publish)
        app.UseSwagger()

        // Enable middleware to serve swagger-ui,
        // specifying the Swagger JSON endpoint.
        app.UseSwaggerUI(fun c ->
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1")
            c.RoutePrefix <- ""
        )
        app.Run()

        0
