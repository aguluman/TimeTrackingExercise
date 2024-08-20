namespace Registration.User

open Registration.User.Events
open Shared.Types

type User =
    { UserId: UserId
      Email: Email
      PasswordHash: PasswordHash
      PhoneNumber: PhoneNumber
      FirstName: string
      LastName: string }

type VerifyingUser =
    { UserId: UserId
      Email: Email
      PhoneNumber: PhoneNumber
      CompletionId: RegistrationCompletionId }

type UserState =
    | NotExisting
    | InVerification of VerifyingUser
    | Active of User
    | Deactivated of User

module User =
    let projectState (events: UserEvent list) =
        events
        |> List.sortBy (_.Instant)
        |> List.fold
            (fun oldState event ->
                match oldState, event.Data with
                | _, RegistrationStarted eventData ->
                    UserState.InVerification
                        { VerifyingUser.UserId = event.UserId
                          Email = event.Email
                          PhoneNumber = eventData.PhoneNumber
                          CompletionId = eventData.CompletionId }
                | InVerification old, RegistrationCompleted eventData ->
                    UserState.Active
                        { User.UserId = event.UserId
                          Email = event.Email
                          PasswordHash = eventData.PasswordHash
                          PhoneNumber = old.PhoneNumber
                          FirstName = eventData.FirstName
                          LastName = eventData.LastName }
                | Active user, UserDeactivated -> UserState.Deactivated user
                | old, _ -> old)
            UserState.NotExisting

    let getUser (storage: UserEventStorage) email =
        async {
            let! events = storage.QueryByEmail email
            return projectState events
        }
