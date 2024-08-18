namespace Accounting.Wallet

open Shared.Types


type Wallet =
    { WalletId: WalletId
      UserId: UserIdForWallet
      Balance: Balance }

module Wallet =
    let project (events: WalletEvent list) =
        events
        |> List.sortBy (_.Instant)
        |> List.fold
            (fun oldState event ->
                match oldState, event.Data with
                | _, Created ->
                    // Create only if this is the first event
                    match oldState with
                    | None ->
                        // The `Balance` here is defined with `0m` to denote the initial balance
                        Some
                            { WalletId = event.WalletId
                              UserId = event.UserId
                              Balance = Balance 0m }
                    | _ -> failwith "Unexpected event: Wallet already created"
                | Some old, Withdrawn(Amount amount) ->
                    let oldBalance =
                        old.Balance
                        |> (function
                        | Balance b -> b)

                    if oldBalance >= amount then
                        // Only withdraw if the balance is enough
                        Some
                            { old with
                                Balance = Balance(oldBalance - amount) }
                    else
                        failwith "Insufficient balance"
                | Some old, Deposited(Amount amount) ->
                    let oldBalance =
                        old.Balance
                        |> (function
                        | Balance b -> b)

                    Some
                        { old with
                            Balance = Balance(oldBalance + amount) }
                | None, _ -> failwith "Unexpected event: Wallet not yet created")
            None
