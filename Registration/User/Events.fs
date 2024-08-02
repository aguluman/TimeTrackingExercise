namespace Registration.User

open System
open Registration.Fundamentals

module Events =
    type UserId = UserId of Guid
    type Email = Email of string
    type PhoneNumber = PhoneNumber of string
    type PasswordHash = PasswordHash of string
    type RegistrationCompletionId = RegistrationCompletionId of Guid

    type RegistrationStartedData =
        { PhoneNumber: PhoneNumber
          CompletionId: RegistrationCompletionId }

    type RegistrationCompletedData =
        { PasswordHash: PasswordHash
          FirstName: string
          LastName: string }

    type UserEventData =
        | RegistrationStarted of RegistrationStartedData
        | RegistrationCompleted of RegistrationCompletedData
        | UserDeactivated

    type UserEvent =
        { UserId: UserId
          EventId: Guid
          Email: Email
          Data: UserEventData
          Instant: Instant }
