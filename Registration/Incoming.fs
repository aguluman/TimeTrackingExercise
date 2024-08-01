namespace Registration

open Registration.User
open Registration.User.Events
open Registration.Verification
open Registration.Verification.Model

type RegistrationStorages =
    { UserEvents: UserEventStorage
      OpenVerifications: OpenVerificationStorage }

type PostgreSqlContext = { ConnectionString: string }

type RegistrationStorageContext =
    | InMemory
    | PostgreSql of PostgreSqlContext

module RegistrationStorageCreator =
    let create (ctx: RegistrationStorageContext) =
        match ctx with
        | InMemory ->
            { UserEvents = UserEventInMemoryStorage.create ()
              OpenVerifications = OpenVerificationInMemoryStorage.create () }
        | _ -> failwith "Not yet implemented" //TODO: Implement PostgreSql storage

type RegistrationServices =
    { GetNodaInstant: unit -> NodaTime.Instant
      SendVerificationCode: SendVerificationCode
      GenerateVerificationCode: GenerateVerificationCode
      GetPasswordHash: string -> PasswordHash }
