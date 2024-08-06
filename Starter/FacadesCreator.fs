namespace Starter

open System
open Accounting
open Accounting.Features
open Accounting.Wallet
open Accounting.AccountingStorageCreator
open Registration
open Rental
open Microsoft.Extensions.Configuration
open Rental.Bike

type Facades =
    { Registration: RegistrationFacade
      Accounting: AccountingFacade
      Rental: RentalFacade }

module Adapters =
    let createWallet (facade: AccountingFacade) (Registration.User.Events.UserId userId) =
        facade.CreateWallet (Guid.NewGuid() |> WalletId) { CreateWallet.Data.UserId = UserId userId }

module FacadesCreator =
    let create (_configuration: IConfiguration) =
        let uiChangedEvent = Event<Guid * string>()

        let accountingServices =
            { AccountingServices.GetNodaInstant = Services.getNodaInstant }

        let accountingChanged msg (UserId userId) = uiChangedEvent.Trigger(userId, msg)

        let accountingFacade =
            AccountingFacade(accountingServices, (create AccountingStorageContext.InMemory), accountingChanged)

        let registrationServices =
            { RegistrationServices.GenerateVerificationCode = Fakes.generateVerificationCode
              GetNodaInstant = Services.getNodaInstant
              SendVerificationCode = Fakes.sendVerificationCode
              GetPasswordHash = Fakes.hashPassword
              CreateAuthToken = Fakes.createAuthToken
              CreateWallet = Adapters.createWallet accountingFacade }


        let registrationFacade =
            RegistrationFacade(
                registrationServices,
                (RegistrationStorageCreator.create RegistrationStorageContext.InMemory)
            )

        let rentalServices = { RentalServices.GetNodaInstant = Services.getNodaInstant }

        let rentalJsonContext = { BikeJsonStorage.JsonContext.FilePath = "./bikes.json" }

        let rentalFacade =
            RentalFacade(
                rentalServices,
                (RentalStorageCreator.create (RentalStorageContext.Mixed rentalJsonContext))
            )

        uiChangedEvent,
        { Registration = registrationFacade
          Accounting = accountingFacade
          Rental = rentalFacade }
