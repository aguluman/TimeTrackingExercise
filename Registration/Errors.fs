namespace Registration

type RegistrationError =
    | UsernameAlreadyTaken
    | UserNotInVerificationProcess
    | WrongVerificationCode

