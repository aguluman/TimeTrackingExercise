namespace Accounting.Wallet

open System
open Shared.Types

type WalletId = WalletId of Guid
type UserIdForWallet = UserIdForWallet of Guid

type WalletEventData =
    | Created
    | Withdrawn of Amount
    | Deposited of Amount

type WalletEvent =
    { WalletId: WalletId
      UserId: UserIdForWallet
      EventId: Guid
      Data: WalletEventData
      Instant: Instant }
