﻿namespace Registration.Features

open System
open FsToolkit.ErrorHandling
open Registration
open Registration.User
open Registration.User.Events
open Registration.Verification.Model
open Shared.Types

[<RequireQualifiedAccess>]
module StartRegistration =
    type DataForStartRegistration =
        { Email: Email
          PhoneNumber: PhoneNumber }

    let execute
        (queryUser: Email -> Async<UserState>)
        (persistUserEvent: UserEvent -> Async<unit>)
        (addOpenVerification: OpenVerification -> Async<unit>)
        (generateVerificationCode: GenerateVerificationCode)
        (sendVerificationCode: SendVerificationCode)
        (getInstant: unit -> Instant)
        userId
        completionId
        (data: DataForStartRegistration)
        =
        asyncResult {
            let! userState = queryUser data.Email

            do!
                match userState with
                | NotExisting -> Ok()
                | _ -> Error RegistrationError.EmailAlreadyRegistered

            do!
                persistUserEvent
                    { UserEvent.UserId = userId
                      EventId = Guid.NewGuid()
                      Email = data.Email
                      Data =
                        UserEventData.RegistrationStarted
                            { RegistrationStartedData.CompletionId = completionId
                              PhoneNumber = data.PhoneNumber }
                      Instant = getInstant () }

            let verificationCode = generateVerificationCode ()

            do!
                addOpenVerification
                    { OpenVerification.Email = data.Email
                      VerificationCode = verificationCode }

            do! sendVerificationCode verificationCode data.PhoneNumber
        }
