using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using HotChocolate;
using Tokopodia.SyncDataService.Dtos;
using Tokopodia.Helpers;

namespace Tokopodia.SyncDataService.Http
{
  public class HttpDianterExpressDataClient : IDianterExpressDataClient
  {
    private readonly AppSettings _appSettings;
    public HttpDianterExpressDataClient([Service] IOptions<AppSettings> appSettings)
    {
      _appSettings = appSettings.Value;
    }
    public async Task<ShipmentOutput> CreateShipment(ShipmentInput input)
    {
      HttpClientHandler handler = new HttpClientHandler();
      using (var client = new HttpClient(handler, false))
      {
        client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IlNlcnZpY2VzIiwicm9sZSI6IlRva29wb2RpYSIsIm5iZiI6MTY0MzQ1NTM3NCwiZXhwIjoxNjU5MDkzNzc0LCJpYXQiOjE2NDM0NTUzNzR9.75tzAOdYRqACuZj_Xq7TLmAUVde9heaYvZMkl_ECcuk");
        var json = JsonSerializer.Serialize(input);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(_appSettings.DiantarExpressUrl + "/api/v1/Shipment/tokopodia", data);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
          throw new Exception("Failed to create shipment on DiantarExpress service");
        Console.WriteLine("==>>>>> " + content);
        ShipmentOutput responseData = JsonSerializer.Deserialize<ShipmentOutput>(content);
        Console.WriteLine(JsonSerializer.Serialize<ShipmentOutput>(responseData));
        return responseData;
      }

    }
    public async Task<CheckFeeOutput> CheckFee(CheckFeeInput input)
    {
      HttpClientHandler handler = new HttpClientHandler();
      using (var client = new HttpClient(handler, false))
      {
        var json = JsonSerializer.Serialize(input);
        client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IlNlcnZpY2VzIiwicm9sZSI6IlRva29wb2RpYSIsIm5iZiI6MTY0MzQ1NTM3NCwiZXhwIjoxNjU5MDkzNzc0LCJpYXQiOjE2NDM0NTUzNzR9.75tzAOdYRqACuZj_Xq7TLmAUVde9heaYvZMkl_ECcuk");
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(_appSettings.DiantarExpressUrl + "/api/v1/Shipment/fee", data);
        var content = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
          Console.WriteLine("==>>>>> " + content);
          CheckFeeOutput responseData = JsonSerializer.Deserialize<CheckFeeOutput>(content);
          Console.WriteLine(JsonSerializer.Serialize<CheckFeeOutput>(responseData));
          return responseData;
        }
        else
        {
          throw new Exception("Failed to check fee on DiantarExpress service");
        }
      }
    }
  }
}
