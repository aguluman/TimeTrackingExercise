namespace Registration.User

open System
open Registration.Fundamentals

module Model =
    type UserId = UserId of Guid
    type Email = Email of string
    type PhoneNumber = PhoneNumber of string
    type PasswordHash = PasswordHash of string
    type RegistrationCompletionId = RegistrationCompletionId of Guid

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
          Email: Email
          Data: UserEventData
          Instant: Instant }
