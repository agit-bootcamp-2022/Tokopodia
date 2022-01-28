using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using System;
using System.Threading.Tasks;
using Tokopodia.Output;
using Tokopodia.Data.Transactions;
using AutoMapper;
using System.Collections.Generic;
using Tokopodia.Helper;

namespace Tokopodia.GraphQL.Queries
{
  [ExtendObjectType(Name = "Query")]
  [Obsolete]
  public class TransactionQuery
  {
    private readonly IMapper _mapper;

    public TransactionQuery([Service] IMapper mapper)
    {
      _mapper = mapper;
    }

    [Authorize(Roles = new[] { "Courier", "Seller", "Buyer" })]
    public async Task<TransactionOutput> GetTransactionById(int transactionId, [Service] ITransaction _transaction)
    {
      try
      {
        var transactionResult = await _transaction.GetById(transactionId);
        if (transactionResult == null)
          throw new DataNotFound();
        var transOutput = _mapper.Map<TransactionOutput>(transactionResult);
        transOutput.CartsOutput = _mapper.Map<IEnumerable<CartOutput>>(transactionResult.Carts);
        return transOutput;
      }
      catch (System.Exception ex)
      {
        throw new DataFailed(ex.Message);
      }

    }
  }
}