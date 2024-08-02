namespace Registration.Features

open System
open Registration
open Registration.Fundamentals
open Registration.User
open Registration.User.Events
open FsToolkit.ErrorHandling

[<RequireQualifiedAccess>]
module CompleteRegistration =
    type Data =
        { Email: Email
          CompletionId: RegistrationCompletionId
          FirstName: string
          LastName: string
          Password: string }

    let execute
        (queryUser: Email -> Async<UserState>)
        (persistUserEvent: UserEvent -> Async<unit>)
        (hash: string -> PasswordHash)
        (getInstant: unit -> Instant)
        (data: Data)
        =
        asyncResult {
            let! userState = queryUser data.Email

            let! user =
                match userState with
                | InVerification user -> Ok(user)
                | _ -> Error RegistrationError.UserNotFound

            do!
                (user.CompletionId = data.CompletionId)
                |> Result.requireTrue RegistrationError.WrongCompletionId

            let passwordHash = hash data.Password

            do!
                persistUserEvent
                    { UserEvent.UserId = user.UserId
                      EventId = Guid.NewGuid()
                      Email = user.Email
                      Data =
                        UserEventData.RegistrationCompleted
                            { RegistrationCompletedData.FirstName = data.FirstName
                              LastName = data.LastName
                              PasswordHash = passwordHash }
                      Instant = getInstant () }
        }
