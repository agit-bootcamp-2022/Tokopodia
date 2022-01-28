using GraphQL.Client.Abstractions;
using GraphQL.Client.Serializer.Newtonsoft;
using System.Threading.Tasks;
using Tokopodia.SyncDataService.Dtos;
using GraphQL;
using GraphQL.Client.Http;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Tokopodia.Helpers;
using System;

namespace Tokopodia.SyncDataService.GraphQLClients
{
  public class OwnerConsumer
  {
    private readonly IGraphQLClient _client;
    private AppSettings _appSettings;

    public OwnerConsumer([Service] IGraphQLClient client, [Service] IOptions<AppSettings> appSettings)
    {
      _client = client;
      _appSettings = appSettings.Value;
    }
    public async Task<WalletCustomer> GetSaldo(string token)
    {
      var query = new GraphQLRequest
      {
        Query = @"
          query GetSaldo{
            walletByCustomerIdAsync{
              id
              balance
              createdDate
              customerId
              walletMutations{
                id
                walletId
                amount
                mutationType
                createdDate
              }
            }
          }
          "
      };
      Console.WriteLine(token);
      var graphQLClient = new GraphQLHttpClient(_appSettings.UangTrans, new NewtonsoftJsonSerializer());
      graphQLClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
      var response = await graphQLClient.SendMutationAsync<WalletData>(query);
      Console.WriteLine("===>" + response.Data.walletByCustomerIdAsync[0].balance);
      return response.Data.walletByCustomerIdAsync[0];
    }

    public async Task<TransactionCreateOutput> CreateTransaction(TransactionCreate input, string token)
    {
      var query = new GraphQLRequest
      {
        Query = @"
          mutation CreateTransaction($input: TransactionCreateInput){
            createTransaction(input: $input){
              message
              transactionId
            }
          }
          ",
        Variables = new { input = input }
      };
      Console.WriteLine(token);
      var graphQLClient = new GraphQLHttpClient(_appSettings.UangTrans, new NewtonsoftJsonSerializer());
      graphQLClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
      var response = await graphQLClient.SendMutationAsync<TransactionUangTransCreateOutput>(query);
      Console.WriteLine("===>" + response.Data.createTransaction);
      return response.Data.createTransaction;
    }
  }
}