﻿namespace Accounting

open System

type AccountingId = AccountingId of Guid
type AccountingError =
    | WalletNotFound
    | UserBalanceNotSufficient