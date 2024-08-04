namespace Starter

open System
open Accounting
open Accounting.Features
open Accounting.Wallet
open Accounting.AccountingStorageCreator
open Registration
open Rental
open Microsoft.Extensions.Configuration

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

        let accountingFacade =
            AccountingFacade(accountingServices, (create AccountingStorageContext.InMemory), uiChangedEvent)

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

        let rentalService = { RentalServices.GetNodaInstant = Services.getNodaInstant }

        let rentalFacade =
            RentalFacade(rentalService, (RentalStorageCreator.create RentalStorageContext.InMemory))

        { Registration = registrationFacade
          Accounting = accountingFacade
          Rental = rentalFacade }
