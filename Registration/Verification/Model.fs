namespace Registration.Verification

open Registration.User.Model

module Model =
    type VerificationCode = VerificationCode of string

    type OpenVerification =
        { Email: Email
          VerificationCode: VerificationCode }

    type GenerateVerificationCode = Unit -> VerificationCode
    type SendVerificationCode = VerificationCode -> PhoneNumber -> Async<unit>