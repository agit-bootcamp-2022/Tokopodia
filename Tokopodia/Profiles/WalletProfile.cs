using AutoMapper;
using Tokopodia.Input;
using Tokopodia.Models;
using Tokopodia.Output;

namespace Tokopodia.Profiles
{
    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            CreateMap<Wallet, WalletOutput>();
            CreateMap<WalletInput, Wallet>();
        }
    }
}
