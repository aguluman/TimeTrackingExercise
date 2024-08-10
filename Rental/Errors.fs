namespace Rental

type RentalErrors =
   | BikeNotFound
   | BookingNotFound
   | BikeAlreadyReleased
   | BikeAlreadyBooked
   | UserWalletNotFound
   | UserBalanceNotSufficient
   | CouldNotWithdrawMoney