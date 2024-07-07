namespace Registration.Verification

open Registration

type VerificationCode = VerificationCode of string

type OpenVerification =
    { Email: Email
      VerificationCode: VerificationCode }
