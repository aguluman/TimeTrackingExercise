namespace Registration.Features

open Registration.User
open Registration.User.Events


[<RequireQualifiedAccess>]
module QueryInitials =
    let query (getUserEvents: Email -> Async<UserEvent list>) initialsFromEmail =
        async {
            let! userEvents = getUserEvents initialsFromEmail
            let userState = User.projectState userEvents

            return
                match userState with
                | Active user ->
                    let firstNameFirstLetter = user.FirstName.Substring(0, 1)
                    let lastNameFirstLetter = user.LastName.Substring(0, 1)
                    Some $"{firstNameFirstLetter}{lastNameFirstLetter}"
                | _ -> None

        }