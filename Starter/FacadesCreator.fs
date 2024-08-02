namespace Starter

open Accounting
open Accounting.AccountingStorageCreator
open Microsoft.Extensions.Configuration
open Registration


type Facades =
    { Registration: RegistrationFacade
      Accounting: AccountingFacade }

module FacadesCreator =
    let create (_configuration: IConfiguration) =
        let registrationServices =
            { RegistrationServices.GenerateVerificationCode = Fakes.generateVerificationCode
              GetNodaInstant = Services.getNodaInstant
              SendVerificationCode = Fakes.sendVerificationCode
              GetPasswordHash = Fakes.hashPassword
              CreateAuthToken = Fakes.createAuthToken }

        let registrationFacade =
            RegistrationFacade(
                registrationServices,
                (RegistrationStorageCreator.create RegistrationStorageContext.InMemory)
            )

        let accountingServices =
            { AccountingServices.GetNodaInstant = Services.getNodaInstant }

        let accountingFacade =
            AccountingFacade(accountingServices, (create AccountingStorageContext.InMemory))

        { Registration = registrationFacade
          Accounting = accountingFacade }
