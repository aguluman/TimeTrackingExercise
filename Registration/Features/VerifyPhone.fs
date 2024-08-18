namespace Registration.Features

open Registration
open Registration.User
open Registration.User.Events
open Registration.Verification.Model
open FsToolkit.ErrorHandling

module VerifyPhone =
    type DataForVerifyPhone =
        { Email: Email
          VerificationCode: VerificationCode }


    let execute
        (queryUser: Email -> Async<UserState>)
        (queryOpenVerification: Email -> Async<OpenVerification option>)
        (removeOpenVerification: Email -> Async<unit>)
        (data: DataForVerifyPhone)
        =
        asyncResult {
            let! userState = queryUser data.Email

            let! user =
                match userState with
                | InVerification user -> Ok(user)
                | _ -> Error RegistrationError.UserNotInVerificationProcess

            let! openVerification =
                user.Email
                |> queryOpenVerification
                |> Async.map (Result.requireSome RegistrationError.UserNotInVerificationProcess)

            do!
                (openVerification.VerificationCode = data.VerificationCode)
                |> Result.requireTrue RegistrationError.WrongVerificationCode

            do! removeOpenVerification user.Email

            return user.CompletionId
        }