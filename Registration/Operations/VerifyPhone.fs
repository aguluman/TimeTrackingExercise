namespace Registration.Operations

open Registration.User.Model
open Registration.Verification.Model

module VerifyPhone =
    type Data =
        { Email: Email
          VerificationCode: VerificationCode }
