using System;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Tokopodia.Data.BuyerProfiles;
using Tokopodia.Helper;

namespace Tokopodia.GraphQL.Mutations
{
  [ExtendObjectType(Name = "Mutation")]
  [Obsolete]
  public class TransactionMutation
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TransactionMutation([Service] IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }
    // public async Task<TransactionOutput> AddTransaction([Service] IBuyerProfile _buyerProfile)
    // {
    //   try
    //   {
    //     var profileId = Int32.Parse(_httpContextAccessor.HttpContext.User.FindFirst("ProfileId").Value);
    //     var profileResult = await _buyerProfile.GetProfileById(profileId);
    //     if (profileResult == null)
    //       throw new Exception("Unauthorized.");
    //     // cek semu cart list yang statusnya OnCart

    //     // simpan semuanya di transaction
    //     // kalo udh ambil shipping address dari buyer id
    //     //hitung SumBillingSeller
    //     // hitung SumShippingCost
    //     // TotalBilling


    //   }
    //   catch (System.Exception ex)
    //   {
    //     throw new DataFailed(ex.Message);
    //   }

    // }
  }
}