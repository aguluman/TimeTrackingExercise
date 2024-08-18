namespace Registration.Features

open FsToolkit.ErrorHandling
open Registration
open Registration.User
open Registration.User.Events

type AuthToken = AuthToken of string

[<RequireQualifiedAccess>]
module CreateToken =
    type DataForCreateToken =
        {
            Email : Email
            Password : string
        }

    let execute
        (queryUser: Email -> Async<UserState>)
        (hash: string -> PasswordHash)
        (createToken: User -> AuthToken)
        (data: DataForCreateToken)
        =
        asyncResult {
            let! userState = queryUser data.Email

            let! user =
                match userState with
                | Active user -> Ok(user)
                | _ -> Error RegistrationError.UserNotFound

            let passwordHash = hash data.Password

            do!
                (user.PasswordHash = passwordHash)
                |> Result.requireTrue RegistrationError.WrongPassword

            return createToken user
        }