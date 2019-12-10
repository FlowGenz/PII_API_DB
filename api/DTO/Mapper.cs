using System;
using API_DbAccess;

namespace DTO {
    public static class Mapper
    {
        public static CustomerDTO MapCustomerToDTO(Customer customer) {
            CustomerDTO dto = new CustomerDTO();
            dto.Id = customer.Id;
            dto.FirstName = customer.FirstName;
            dto.LastName = customer.LastName;
            dto.FidelityPoints = customer.FidelityPoints;
            dto.Email = customer.Email;
            dto.PhoneNumber = customer.PhoneNumber;
            dto.CustomerAddress = customer.CustomerAddress;
            dto.Username = customer.UsernameUserNavigation.Username;
            dto.CustomerPassword = customer.UsernameUserNavigation.UserPassword;
            return dto;
        }

        public static DressDTO MapDressToDTO(Dress dress) {
            DressDTO dto = new DressDTO();
            dto.Id = dress.Id;
            dto.DressName = dress.DressName;
            dto.Describe = dress.Describe;
            dto.Price = dress.Price;
            dto.Availible = dress.Availible;
            dto.DateBeginAvailable = dress.DateBeginAvailable;
            dto.DateEndAvailable = dress.DateEndAvailable;
            dto.PartnerId = dress.Partners.Id;
            dto.PartnerName = dress.Partners.UsernameUserNavigation.Username;
            return dto;
        }

        public static DressOrderDTO MapOrderToDTO(DressOrder dressOrder) {
            DressOrderDTO dto = new DressOrderDTO();
            dto.Id = dressOrder.Id;
            dto.BillingAddress = dressOrder.BillingAddress;
            dto.BillingDate = dressOrder.BillingDate;
            dto.DeliveryAddress = dressOrder.DeliveryAddress;
            dto.DeliveryDate = dressOrder.DeliveryDate;
            dto.IsValid = dressOrder.IsValid;
            dto.CustomerId = dressOrder.Customer.Id;
            dto.CustomerName = dressOrder.Customer.FirstName + " " + dressOrder.Customer.LastName;
            return dto;
        }

        public static PartnerDTO MapPartnerToDTO(Partners partner) {
            PartnerDTO dto = new PartnerDTO();
            dto.Username = partner.UsernameUser;
            return dto;
        }
    }
}