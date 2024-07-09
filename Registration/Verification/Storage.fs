namespace Registration.Verification

open Registration.User.Model
open Registration.Verification.Model

type OpenVerificationStorage =
    abstract Add: OpenVerification -> Async<unit>
    abstract Remove: Email -> Async<unit>
    abstract Query: Email -> Async<OpenVerification option>

//Todo: I will add the PostgreSqlContext here
module OpenVerificationInMemoryStorage =
    let create () =
        let mutable verifications: OpenVerification list = []

        { new OpenVerificationStorage with
            member self.Add v =
                async { verifications <- verifications |> List.append [ v ] }

            member self.Remove email =
                async { verifications <- verifications |> List.filter (fun v -> v.Email <> email) }

            member self.Query email =
                async { return verifications |> List.tryFind (fun x -> x.Email = email) } }
