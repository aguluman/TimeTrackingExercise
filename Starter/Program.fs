namespace Starter

#nowarn "20"

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Mvc.ApplicationParts
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Registration
open Accounting

module Program =
    [<EntryPoint>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)

        let registrationAssemblyPart = typeof<RegistrationFacade>.Assembly |> AssemblyPart

        let accountingAssemblyPart = typeof<AccountingId>.Assembly |> AssemblyPart

        let parts = builder.Services.AddControllers().PartManager.ApplicationParts

        parts.Add(registrationAssemblyPart)
        parts.Add(accountingAssemblyPart)

        let facades = FacadesCreator.create builder.Configuration

        builder.Services.AddSingleton<RegistrationFacade>(fun _ -> facades.Registration)
        |> ignore

        let app = builder.Build()

        if app.Environment.IsDevelopment() then
            app.UseDeveloperExceptionPage() |> ignore

        app.UseHttpsRedirection()
        app.UseRouting()
        app.UseAuthorization()
        app.MapControllers()

        app.Run()

        0
