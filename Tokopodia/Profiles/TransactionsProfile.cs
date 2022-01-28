using System;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;

namespace Tokopodia.Profiles
{
  public class TransactionsProfile : Profile
  {
    public TransactionsProfile()
    {
      CreateMap<Transaction, TransactionOutput>();
      CreateMap<Cart, CartOutput>()
        .ForMember(dest => dest.BuyerName, opt => opt.MapFrom(src => $"{src.Buyer.FirstName} {src.Buyer.LastName}"))
        .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.Seller.ShopName))
        .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
    }
  }
}
