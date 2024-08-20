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
    let createWallet (facade: AccountingFacade)  userId= 
        facade.CreateWallet
            (Guid.NewGuid() |> WalletId)
            { CreateWallet.Data.UserId = userId }

    let withdrawFromUserBalance
        (facade: AccountingFacade) amount userId  =
        async {
            let! result =
                facade.Withdraw
                    { Withdraw.DataForWithdraw.UserId = userId
                      Amount = amount }
            return 
                match result with
                | Ok _ -> true
                | _ -> false          
        }

module FacadesCreator =
    let create (_configuration: IConfiguration) =
        let uiChangedEvent = Event<string * obj>()

        let accountingServices =
            { AccountingServices.GetInstant = Services.getInstant }

        let accountingChanged msg (WalletId walletId) =
            uiChangedEvent.Trigger(walletId.ToString(), msg)

        let accountingFacade =
            AccountingFacade(
                accountingServices,
                (create AccountingStorageContext.InMemory),
                accountingChanged )

        let registrationServices =
            { RegistrationServices.GenerateVerificationCode = Fakes.generateVerificationCode
              GetInstant = Services.getInstant
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
            RentalServices.GetInstant = Services.getInstant
            WithdrawAmount =  Adapters.withdrawFromUserBalance accountingFacade
        }

        let rentalJsonContext = { BikeJsonStorage.JsonContext.FilePath = "./bikes.json" }

        let bikesUiChanged msg = uiChangedEvent.Trigger("bikes", msg)
        
        let rentalFacade =
            RentalFacade(
                rentalServices,
                (RentalStorageCreator.create (RentalStorageContext.Mixed rentalJsonContext)),
                bikesUiChanged
            )

        uiChangedEvent,
        { Registration = registrationFacade
          Accounting = accountingFacade
          Rental = rentalFacade }
