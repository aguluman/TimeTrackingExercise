namespace Registration.User

open Registration.User.Model

type UserEventStorage =
    abstract PersistEvent: UserEvent -> Async<unit>
    abstract QueryByEmail: Email -> Async<UserEvent list>

//Todo: I will add the PostgreSqlContext here
module UserEventInMemoryStorage =
    let create () =
        let mutable events: UserEvent list = []

        { new UserEventStorage with
            member self.PersistEvent event =
                async { events <- events |> List.append [ event ] }

            member self.QueryByEmail email =
                async { return events |> List.filter (fun e -> e.Email = email) } }
