namespace Starter

open System.Security.Cryptography
open System.Text
open Registration.Features
open Registration.User
open Registration.User.Events
open Registration.Verification.Model

module Fakes =
    let generateVerificationCode () = VerificationCode "Xyz"

    let sendVerificationCode (VerificationCode code) (PhoneNumber phone) =
        async { printfn $"Verification Code for {phone}: {code}" }

    let hashPassword (input: string) =
        let fromBytes (data: byte array) =
            use md5 = MD5.Create()

            (StringBuilder(), md5.ComputeHash(data))
            ||> Array.fold (fun sb b -> sb.Append(b.ToString("x2")))
            |> string

        let result = fromBytes (Encoding.UTF8.GetBytes(input))
        PasswordHash result

    let createAuthToken (user: User) =
        let email = user.Email |> (fun (Email e) -> e)
        let userId = user.UserId |> (fun (UserId u) -> u.ToString())
        let result = $"thisIsAFakeToken+{email}+{userId}"

        AuthToken result
