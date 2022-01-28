using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tokopodia.Input;
using Tokopodia.Output;

namespace Tokopodia.SyncDataService.Http
{
    public class HttpUangTransDataClient : IUangTransDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpUangTransDataClient(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            var accessToken = httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            httpClient.DefaultRequestHeaders.Authorization
             = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<BalanceOutput> GetSaldoUser()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_configuration["ReadSaldo"]).Result;
            var results = await response.Content.ReadAsStringAsync();

            var resultbalance = Newtonsoft.Json.JsonConvert.DeserializeObject<BalanceOutput>(results);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync Get Saldo User  Was OK !");
                return resultbalance;
            }
            else
            {
                Console.WriteLine("--> Sync Saldo User Failed");

                throw new ArgumentNullException("Wallet Tidak ditemukan");
            }
        }

        public async Task SendWalletToUangTrans(WalletForHttpInput input)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(input),
                Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_configuration["ReadSaldo"],
                httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to UangTrans Was OK !");
            }
            else
            {
                Console.WriteLine("--> Sync POST to UangTrans Failed");
            }
        }
    }
}
