namespace Starter

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
open Accounting
module Program =
    let exitCode = 0

    let CreateHostBuilder args =
        Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(fun webBuilder -> webBuilder.UseStartup<StartupBase>() |> ignore)

    [<EntryPoint>]
    let main args =
        CreateHostBuilder(args).Build().Run()

        let builder = WebApplication.CreateBuilder(args)

        let registrationAssemblyPart =
            typeof<WeatherForecast>.Assembly |> AssemblyPart

        let accountingAssemblyPart =
            typeof<AccountingId>.Assembly |> AssemblyPart

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
