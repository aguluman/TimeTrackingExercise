namespace Registration.Operations

open Registration.User.Events

[<RequireQualifiedAccess>]
module CompleteRegistration =
    type Data =
        { CompletionId: RegistrationCompletionId
          FirstName: string
          LastName: string }
