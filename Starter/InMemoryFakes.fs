namespace Starter

open Registration.User.Events
open Registration.Verification.Model

module Fakes =
    let generateVerificationCode () = VerificationCode ""

    let sendVerificationCode (VerificationCode code) (PhoneNumber phone) =
        async { printfn $"Verification Code for {phone}: {code}" }