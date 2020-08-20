
using System;
using System.Collections.Generic;
using API_DbAccess;

namespace DTO {
    public class Mapper
    
    {
        public Mapper() {

        }
        
        public static CustomerDTO MapCustomerModelToCustomerDTO(User customer) {
            CustomerDTO dto = new CustomerDTO();
            dto.Id = customer.Id;
            dto.FirstName = customer.FirstName;
            dto.LastName = customer.LastName;
            dto.LoyaltyPoints = (int)customer.LoyaltyPoints;
            dto.Email = customer.Email;
            dto.PhoneNumber = customer.PhoneNumber;
            dto.CustomerAddress = customer.UserAddress;
            dto.Username = customer.UserName;
            dto.CustomerPassword = customer.PasswordHash;
            return dto;
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
            return newUser;
        }

        public static DressDTO MapDressModelToDressDTO(Dress dress) {
            DressDTO dto = new DressDTO();
            dto.Id = dress.Id;
            dto.DressName = dress.DressName;
            dto.Description = dress.Description;
            dto.Price = dress.Price;
            dto.Size = dress.Size;
            dto.Available = dress.Available;
            dto.DateBeginAvailable = dress.DateBeginAvailable;
            dto.DateEndAvailable = dress.DateEndAvailable;
            dto.UrlImage = dress.UrlImage;
            dto.PartnerId = dress.User.Id;
            dto.PartnerName = dress.User.UserName;
            return dto;
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
            return newDress;
        }

        public static DressOrderDTO MapDressOrderModelToDressOrderDTO(DressOrder dressOrder) {
            DressOrderDTO dto = new DressOrderDTO();
            dto.Id = dressOrder.Id;
            dto.BillingAddress = dressOrder.BillingAddress;
            dto.BillingDate = dressOrder.BillingDate;
            dto.DeliveryAddress = dressOrder.DeliveryAddress;
            dto.DeliveryDate = dressOrder.DeliveryDate;
            dto.IsValid = dressOrder.IsValid;
            dto.CustomerId = dressOrder.User.Id;
            dto.CustomerName = dressOrder.User.FirstName + dressOrder.User.LastName;
            foreach (OrderLine orderLine in dressOrder.OrderLine) {
                dto.OrderLines.Add(MapOrderLineModelToOrderLineDTO(orderLine));
            }
            return dto;
        }

        public static DressOrder MapDressOrderDtoToDressOrderModel(DressOrderDTO dressOrderDTO)
        {
            DressOrder newDressOrder = new DressOrder();
            newDressOrder.BillingAddress = dressOrderDTO.BillingAddress;
            newDressOrder.BillingDate = dressOrderDTO.BillingDate;
            newDressOrder.DeliveryAddress = dressOrderDTO.DeliveryAddress;
            newDressOrder.DeliveryDate = dressOrderDTO.DeliveryDate;
            newDressOrder.IsValid = dressOrderDTO.IsValid;
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
            PartnerDTO dto = new PartnerDTO();
            dto.Id = partner.Id;
            dto.Username = partner.UserName;
            return dto;
        }

        public static FavoriteDTO MapFavoriteModelToFavoriteDTO(Favorites favorite) {
            FavoriteDTO dto = new FavoriteDTO();
            dto.Id = favorite.Id;
            dto.DressName = favorite.Dress.DressName;
            dto.DressPrice = favorite.Dress.Price;
            dto.UrlImage = favorite.Dress.UrlImage;
            dto.Available = favorite.Dress.Available;
            return dto;
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
            OrderLineDTO dto = new OrderLineDTO();
            dto.Id = orderLine.Id;
            dto.DateBeginLocation = orderLine.DateBeginLocation;
            dto.DateEndLocation = orderLine.DateEndLocation;
            dto.FinalPrice = orderLine.FinalPrice;
            dto.DressId = orderLine.DressId;
            dto.DressOrderId = orderLine.DressOrderId;
            dto.DressName = orderLine.Dress.DressName;
            dto.IsDressAvailable = orderLine.Dress.Available;
            dto.DressUrlImage = orderLine.Dress.UrlImage;
            return dto;
        }
    }
}