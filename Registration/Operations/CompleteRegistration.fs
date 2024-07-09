namespace Registration.Operations

open Registration.User.Model

[<RequireQualifiedAccess>]
module CompleteRegistration =
    type Data =
        { CompletionId: RegistrationCompletionId
          FirstName: string
          LastName: string }
