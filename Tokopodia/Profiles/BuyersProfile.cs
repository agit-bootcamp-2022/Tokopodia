using System;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;

namespace Tokopodia.Profiles
{
  public class BuyersProfile : Profile
  {
    public BuyersProfile()
    {
      CreateMap<RegisterBuyerInput, BuyerProfile>();
      CreateMap<IdentityUser, UserOutput>();
      CreateMap<BuyerProfile, BuyerOutput>();
    }
  }
}
