﻿namespace Registration

open Registration.Features
open Registration.User
open Registration.User.Events
open Registration.Verification
open Registration.Verification.Model

type RegistrationStorages =
    { UserEvents: UserEventStorage
      OpenVerifications: OpenVerificationStorage }

type SqlContext = { ConnectionString: string }

type RegistrationStorageContext =
    | InMemory
    | Sql of SqlContext

module RegistrationStorageCreator =
    let create (ctx: RegistrationStorageContext) =
        match ctx with
        | InMemory ->
            { UserEvents = UserEventInMemoryStorage.create ()
              OpenVerifications = OpenVerificationInMemoryStorage.create () }
        | _ -> failwith "Not yet implemented" //TODO: Implement any Relational Sql storage

type RegistrationServices =
    { GetNodaInstant: unit -> NodaTime.Instant
      SendVerificationCode: SendVerificationCode
      GenerateVerificationCode: GenerateVerificationCode
      GetPasswordHash: string -> PasswordHash
      CreateAuthToken: User -> AuthToken }
