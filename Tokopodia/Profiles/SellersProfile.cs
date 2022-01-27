using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;

namespace Tokopodia.Profiles
{
    public class SellersProfile : Profile
    {
       public SellersProfile()
       {
          CreateMap<RegisterSellerInput, SellerProfile>();
          CreateMap<IdentityUser, UserOutput>();
          CreateMap<SellerProfile, SellerOutput>();
       }
    }
}
