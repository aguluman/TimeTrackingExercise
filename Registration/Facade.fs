﻿namespace Registration

open Registration.Fundamentals
open Registration.Operations
open Registration.User

type RegistrationFacade(services: RegistrationServices, storages: RegistrationStorages) =
    let getInstant = services.GetNodaInstant >> Instant

    member self.StartRegistration =
        StartRegistration.execute
            (User.getUser storages.UserEvents)
            storages.UserEvents.PersistEvent
            storages.OpenVerifications.Add
            services.GenerateVerificationCode
            services.SendVerificationCode
            getInstant

    member self.Hack = storages.UserEvents