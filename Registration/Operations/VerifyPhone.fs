namespace Registration.Operations

open Registration.User.Events
open Registration.Verification.Model

module VerifyPhone =
    type Data =
        { Email: Email
          VerificationCode: VerificationCode }
