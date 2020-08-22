using API_DbAccess;

namespace DTO
{
    public class Mapper
    
    {
        public Mapper() {

        }
        
        public static CustomerDTO MapCustomerModelToCustomerDTO(User customer) {
            CustomerDTO customerDTO = new CustomerDTO();
            customerDTO.Id = customer.Id;
            customerDTO.FirstName = customer.FirstName;
            customerDTO.LastName = customer.LastName;
            customerDTO.LoyaltyPoints = (int)customer.LoyaltyPoints;
            customerDTO.Email = customer.Email;
            customerDTO.PhoneNumber = customer.PhoneNumber;
            customerDTO.CustomerAddress = customer.UserAddress;
            customerDTO.Username = customer.UserName;
            customerDTO.CustomerPassword = customer.PasswordHash;
            customerDTO.RowVersion = customer.RowVersion;
            return customerDTO;
        }

        public static User MapCustomerDtoToCustomerModel(CustomerDTO customerDTO) {
            User newUser = new User();
            newUser.UserName = customerDTO.Username;
            newUser.UserAddress = customerDTO.CustomerAddress;
            newUser.LoyaltyPoints = customerDTO.LoyaltyPoints;
            newUser.LastName = customerDTO.LastName;
            newUser.FirstName = customerDTO.FirstName;
            newUser.Email = customerDTO.Email;
            newUser.PhoneNumber = customerDTO.PhoneNumber;
            newUser.RowVersion = customerDTO.RowVersion;
            return newUser;
        }

        public static DressDTO MapDressModelToDressDTO(Dress dress) {
            DressDTO dressDTO = new DressDTO();
            dressDTO.Id = dress.Id;
            dressDTO.DressName = dress.DressName;
            dressDTO.Description = dress.Description;
            dressDTO.Price = dress.Price;
            dressDTO.Size = dress.Size;
            dressDTO.Available = dress.Available;
            dressDTO.DateBeginAvailable = dress.DateBeginAvailable;
            dressDTO.DateEndAvailable = dress.DateEndAvailable;
            dressDTO.UrlImage = dress.UrlImage;
            dressDTO.PartnerId = dress.User.Id;
            dressDTO.PartnerName = dress.User.UserName;
            dressDTO.RowVersion = dress.RowVersion;
            return dressDTO;
        }

        public static Dress MapDressDtoToDressModel(DressDTO dressDTO, User partner) {
            Dress newDress = new Dress();
            newDress.DressName = dressDTO.DressName;
            newDress.Description = dressDTO.Description;
            newDress.Price = dressDTO.Price;
            newDress.Size = dressDTO.Size;
            newDress.Available = dressDTO.Available;
            newDress.DateBeginAvailable = dressDTO.DateBeginAvailable;
            newDress.DateEndAvailable = dressDTO.DateEndAvailable;
            newDress.UrlImage = dressDTO.UrlImage;
            newDress.User = partner;
            partner.Dress.Add(newDress);
            newDress.RowVersion = dressDTO.RowVersion;
            return newDress;
        }

        public static DressOrderDTO MapDressOrderModelToDressOrderDTO(DressOrder dressOrder) {
            DressOrderDTO dressOrderDTO = new DressOrderDTO();
            dressOrderDTO.Id = dressOrder.Id;
            dressOrderDTO.BillingAddress = dressOrder.BillingAddress;
            dressOrderDTO.BillingDate = dressOrder.BillingDate;
            dressOrderDTO.DeliveryAddress = dressOrder.DeliveryAddress;
            dressOrderDTO.DeliveryDate = dressOrder.DeliveryDate;
            dressOrderDTO.IsValid = dressOrder.IsValid;
            dressOrderDTO.CustomerId = dressOrder.User.Id;
            dressOrderDTO.CustomerName = dressOrder.User.FirstName + dressOrder.User.LastName;
            foreach (OrderLine orderLine in dressOrder.OrderLine) {
                dressOrderDTO.OrderLines.Add(MapOrderLineModelToOrderLineDTO(orderLine));
            }
            dressOrderDTO.RowVersion = dressOrder.RowVersion;
            return dressOrderDTO;
        }

        public static DressOrder MapDressOrderDtoToDressOrderModel(DressOrderDTO dressOrderDTO)
        {
            DressOrder newDressOrder = new DressOrder();
            newDressOrder.BillingAddress = dressOrderDTO.BillingAddress;
            newDressOrder.BillingDate = dressOrderDTO.BillingDate;
            newDressOrder.DeliveryAddress = dressOrderDTO.DeliveryAddress;
            newDressOrder.DeliveryDate = dressOrderDTO.DeliveryDate;
            newDressOrder.IsValid = dressOrderDTO.IsValid;
            newDressOrder.RowVersion = dressOrderDTO.RowVersion;
            return newDressOrder;
        }

        public static OrderLine MapOrderLineDtoToOrderLineModel(OrderLineDTO orderLine, Dress dress, DressOrder dressOrder) {
            OrderLine newOrderLine = new OrderLine();
            newOrderLine.DateBeginLocation = orderLine.DateBeginLocation;
            newOrderLine.DateEndLocation = orderLine.DateEndLocation;
            newOrderLine.Dress = dress;
            newOrderLine.DressOrder = dressOrder;
            newOrderLine.DressOrderId = dressOrder.Id;
            newOrderLine.FinalPrice = orderLine.FinalPrice;
            return newOrderLine;
        }

        public static PartnerDTO MapPartnerModelToPartnerDTO(User partner) {
            PartnerDTO partnerDTO = new PartnerDTO();
            partnerDTO.Id = partner.Id;
            partnerDTO.Username = partner.UserName;
            return partnerDTO;
        }

        public static FavoriteDTO MapFavoriteModelToFavoriteDTO(Favorites favorite) {
            FavoriteDTO favoriteDTO = new FavoriteDTO();
            favoriteDTO.Id = favorite.Id;
            favoriteDTO.DressName = favorite.Dress.DressName;
            favoriteDTO.DressPrice = favorite.Dress.Price;
            favoriteDTO.UrlImage = favorite.Dress.UrlImage;
            favoriteDTO.Available = favorite.Dress.Available;
            return favoriteDTO;
        }

        public static Favorites MapFavoriteDtoToFavoriteModel(FavoriteDTO favoriteDTO, User customer, Dress dress) {
            Favorites newFavorite = new Favorites();
            newFavorite.DressId = favoriteDTO.DressId;
            newFavorite.UserId = favoriteDTO.CustomerId;
            newFavorite.User = customer;
            newFavorite.Dress = dress;
            dress.Favorites.Add(newFavorite);
            return newFavorite;
        }

        public static OrderLineDTO MapOrderLineModelToOrderLineDTO(OrderLine orderLine) {
            OrderLineDTO orderLineDTO = new OrderLineDTO();
            orderLineDTO.Id = orderLine.Id;
            orderLineDTO.DateBeginLocation = orderLine.DateBeginLocation;
            orderLineDTO.DateEndLocation = orderLine.DateEndLocation;
            orderLineDTO.FinalPrice = orderLine.FinalPrice;
            orderLineDTO.DressId = orderLine.DressId;
            orderLineDTO.DressOrderId = orderLine.DressOrderId;
            orderLineDTO.DressName = orderLine.Dress.DressName;
            orderLineDTO.IsDressAvailable = orderLine.Dress.Available;
            orderLineDTO.DressUrlImage = orderLine.Dress.UrlImage;
            return orderLineDTO;
        }
    }
}