namespace Registration

type RegistrationError =
    | EmailAlreadyRegistered
    | UserNotInVerificationProcess
    | WrongVerificationCode

