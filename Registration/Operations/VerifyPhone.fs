module Registration.Operations.VerifyPhone

open Registration
open Registration.Verification

type Data =
    { Email: Email
      VerificationCode: VerificationCode }

