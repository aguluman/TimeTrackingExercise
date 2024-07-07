[<RequireQualifiedAccess>]
module Registration.Operations.CompleteRegistration

open Registration

type Data =
    { CompletionId: RegistrationCompletionId
      FirstName: string
      LastName: string }
