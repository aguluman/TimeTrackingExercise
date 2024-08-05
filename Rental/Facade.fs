namespace Rental

open Rental.Bike
type RentalFacade(_services: RentalServices, storages: RentalStorages) =
    member self.GetAllBikes = storages.Bikes.GetAll