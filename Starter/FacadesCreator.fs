namespace Starter

open Accounting
open Accounting.Features
open Accounting.Wallet
open Accounting.AccountingStorageCreator
open Registration
open Rental
open Rental.Bike
open System
open Microsoft.Extensions.Configuration

type Facades =
    { Registration: RegistrationFacade
      Accounting: AccountingFacade
      Rental: RentalFacade }

module Adapters =
    let createWallet (facade: AccountingFacade) (Registration.User.Events.UserId userId) =
        facade.CreateWallet
            (Guid.NewGuid() |> WalletId)
            { CreateWallet.Data.UserId = UserId userId }

    let withdrawFromUserBalance
        (facade: AccountingFacade)
        (Amount amount)
        (Rental.Booking.UserId userId)
        =
        async {
            let accountingUserId = UserId userId
            let accountingAmount = Accounting.Wallet.Amount amount

            let! result =
                facade.Withdraw
                    { Withdraw.Data.UserId =  accountingUserId
                      Amount = accountingAmount }

            return
                match result with
                | Ok _ -> true
                | _ -> false
        }

module FacadesCreator =
    let create (_configuration: IConfiguration) =
        let uiChangedEvent = Event<Guid * string>()

        let accountingServices =
            { AccountingServices.GetNodaInstant = Services.getNodaInstant }

        let accountingChanged msg (UserId userId) = uiChangedEvent.Trigger(userId, msg)

        let accountingFacade =
            AccountingFacade(
                accountingServices,
                (create AccountingStorageContext.InMemory),
                accountingChanged )

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

        let rentalServices = {
            RentalServices.GetNodaInstant = Services.getNodaInstant
            WithdrawAmount =  Adapters.withdrawFromUserBalance accountingFacade
        }

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
