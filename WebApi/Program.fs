namespace WebApi

#nowarn "20"

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Mvc.ApplicationParts
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Registration
open WebApi.Controllers

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        let registrationAssemblyPart =
            typeof<WeatherForecast>.Assembly |> AssemblyPart

        let accountingAssemblyPart =
            typeof<AccountingController>.Assembly |> AssemblyPart

        let parts =
            builder.Services
                .AddControllers()
                .PartManager
                .ApplicationParts

        parts.Add(registrationAssemblyPart)
        parts.Add(accountingAssemblyPart)

        let app = builder.Build()

        app.UseHttpsRedirection()

        app.UseAuthorization()
        app.MapControllers()

        app.Run()

        exitCode
