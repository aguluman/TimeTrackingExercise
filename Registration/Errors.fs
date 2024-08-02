namespace Registration

type RegistrationError =
    | EmailAlreadyRegistered
    | UserNotInVerificationProcess
    | WrongVerificationCode
    | WrongCompletionId
    | UserNotFound
    | WrongPassword
    | ExternalError
