using System;
using API_DbAccess;

namespace DTO {
    public class Mapper
    
    {
        public Mapper() {

        }
        
        public CustomerDTO MapCustomerToDTO(User customer) {
            CustomerDTO dto = new CustomerDTO();
            dto.Id = customer.Id;
            dto.FirstName = customer.FirstName;
            dto.LastName = customer.LastName;
            dto.LoyaltyPoints = (int)customer.LoyaltyPoints;
            dto.Email = customer.Email;
            dto.PhoneNumber = customer.PhoneNumber;
            dto.CustomerAddress = customer.UserAddress;
            dto.Username = customer.Username;
            dto.CustomerPassword = customer.UserPassword;
            return dto;
        }

        public DressDTO MapDressToDTO(Dress dress) {
            DressDTO dto = new DressDTO();
            dto.Id = dress.Id;
            dto.DressName = dress.DressName;
            dto.Describe = dress.Describe;
            dto.Price = dress.Price;
            dto.Available = dress.Available;
            dto.DateBeginAvailable = dress.DateBeginAvailable;
            dto.DateEndAvailable = dress.DateEndAvailable;
            dto.PartnerId = dress.User.Id;
            dto.PartnerName = dress.User.Username;
            return dto;
        }

        public DressOrderDTO MapOrderToDTO(DressOrder dressOrder) {
            DressOrderDTO dto = new DressOrderDTO();
            dto.Id = dressOrder.Id;
            dto.BillingAddress = dressOrder.BillingAddress;
            dto.BillingDate = dressOrder.BillingDate;
            dto.DeliveryAddress = dressOrder.DeliveryAddress;
            dto.DeliveryDate = dressOrder.DeliveryDate;
            dto.IsValid = dressOrder.IsValid;
            dto.CustomerId = dressOrder.User.Id;
            dto.CustomerName = dressOrder.User.FirstName + " " + dressOrder.User.LastName;
            return dto;
        }

        public PartnerDTO MapPartnerToDTO(User partner) {
            PartnerDTO dto = new PartnerDTO();
            dto.Id = partner.Id;
            dto.Username = partner.Username;
            return dto;
        }

        public FavoriteDTO MapFavoriteToDTO(Favorites favorite) {
            FavoriteDTO dto = new FavoriteDTO();
            dto.Id = favorite.Id;
            dto.DressName = favorite.Dress.DressName;
            dto.DressPrice = favorite.Dress.Price;
            dto.UrlImage = favorite.Dress.UrlImage;
            return dto;
        }
    }
}