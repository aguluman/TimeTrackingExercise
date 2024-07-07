[<RequireQualifiedAccess>]
module Registration.Operations.StartRegistration

open FsToolkit.ErrorHandling
open Registration

type Data =
    { Email: Email
      PhoneNumber: PhoneNumber }

let execute (queryUser: Email -> UserState) data = asyncResult { () }
