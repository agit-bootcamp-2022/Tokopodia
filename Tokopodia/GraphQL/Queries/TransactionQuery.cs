using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Tokopodia.Output;
using Tokopodia.Data.Transactions;
using Tokopodia.Data.Products;
using Tokopodia.Data.SellerProfiles;
using Tokopodia.Data.BuyerProfiles;
using AutoMapper;
using System.Collections.Generic;
using Tokopodia.Helper;
using Microsoft.AspNetCore.Http;
using Tokopodia.Models;
namespace Tokopodia.GraphQL.Queries
{
  [ExtendObjectType(Name = "Query")]
  [Obsolete]
  public class TransactionQuery
  {
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TransactionQuery([Service] IMapper mapper, [Service] IHttpContextAccessor httpContextAccessor)
    {
      _mapper = mapper;
      _httpContextAccessor = httpContextAccessor;
    }

    [Authorize(Roles = new[] { "Courier", "Seller", "Buyer" })]
    public async Task<TransactionOutput> GetTransactionById(int transactionId,
                                                            [Service] ITransaction _transaction,
                                                            [Service] IBuyerProfile _buyer,
                                                            [Service] ISellerProfile _seller,
                                                            [Service] IProduct _product)
    {
      try
      {
        var transactionResult = await _transaction.GetById(transactionId);
        if (transactionResult == null)
          throw new Exception("Data not found");
        var transOutput = _mapper.Map<TransactionOutput>(transactionResult);
        foreach (var cart in transactionResult.Carts)
        {
          Console.WriteLine("name: " + cart.Product.Name);
        }
        transOutput.CartsOutput = _mapper.Map<IEnumerable<CartOutput>>(transactionResult.Carts);
        return transOutput;
      }
      catch (System.Exception ex)
      {
        throw new DataFailed(ex.Message);
      }

    }

    [Authorize(Roles = new[] { "Buyer" })]
    public async Task<TransactionOutput> GetTransaction(int transactionId,
                                                            [Service] ITransaction _transaction,
                                                            [Service] IBuyerProfile _buyer,
                                                            [Service] ISellerProfile _seller,
                                                            [Service] IProduct _product)
    {
      try
      {
        var transactionResult = await _transaction.GetById(transactionId);
        if (transactionResult == null)
          throw new Exception("Data not found");
        var transOutput = _mapper.Map<TransactionOutput>(transactionResult);
        foreach (var cart in transactionResult.Carts)
        {
          Console.WriteLine("name: " + cart.Product.Name);
        }
        transOutput.CartsOutput = _mapper.Map<IEnumerable<CartOutput>>(transactionResult.Carts);
        return transOutput;
      }
      catch (System.Exception ex)
      {
        throw new DataFailed(ex.Message);
      }

    }

    [Authorize(Roles = new[] { "Seller", "Buyer" })]
    public async Task<IEnumerable<TransactionOutput>> GetTransactionByProfileId(
                                                            [Service] ITransaction _transaction,
                                                            [Service] IBuyerProfile _buyer,
                                                            [Service] ISellerProfile _seller,
                                                            [Service] IProduct _product)
    {
      try
      {
        var userId = _httpContextAccessor.HttpContext.User.FindFirst("ProfileId").Value;
        var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
        if (userId == null)
        {
          throw new Exception("Unauthorized.");
        }
        var transactionResult = await _transaction.GetByProfileId(Int16.Parse(userId), role);
        if (transactionResult == null)
          throw new Exception("Data not found");

        var transOutputs = _mapper.Map<IEnumerable<TransactionOutput>>(transactionResult);

        return transOutputs;
      }
      catch (System.Exception ex)
      {
        throw new DataFailed(ex.Message);
      }

    }
  }
}