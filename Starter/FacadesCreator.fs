﻿namespace Starter

open Microsoft.Extensions.Configuration
open Registration


type Facades = { Registration: RegistrationFacade }

module FacadesCreator =
    let create (_configuration: IConfiguration) =
            let registrationServices =
                { RegistrationServices.GenerateVerificationCode = Fakes.generateVerificationCode
                  GetNodaInstant = Services.getNodaInstant
                  SendVerificationCode = Fakes.sendVerificationCode
                  GetPasswordHash = Fakes.hashPassword
                  CreateAuthToken = Fakes.createAuthToken}

            let registrationFacade =
                RegistrationFacade(
                    registrationServices,
                    (RegistrationStorageCreator.create RegistrationStorageContext.InMemory)
                )

            { Registration = registrationFacade }