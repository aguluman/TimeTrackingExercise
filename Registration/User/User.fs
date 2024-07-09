namespace Registration.User

open Registration.User.Model

module User =
    let projectState (events: UserEvent list) =
        events
        |> List.sortBy (fun x -> x.Instant)
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
