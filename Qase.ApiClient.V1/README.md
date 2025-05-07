# Qase TMS C# API V1 Client

## Installation

### NuGet CLI

```sh
Install-Package Qase.ApiClient.V1
```

### .NET CLI

```sh
dotnet package add Qase.ApiClient.V1
```

## Usage

The client uses API tokens to authenticate requests. You can view and manage your API keys on
the [API tokens page](https://app.qase.io/user/api/token).

```cs
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Qase.ApiClient.V1.Api;
using Qase.ApiClient.V1.Client;
using Qase.ApiClient.V1.Extensions;

namespace YourProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var api = host.Services.GetRequiredService<ICasesApi>();
            var apiResponse = await api.GetCasesAsync("DEVX", limit: 10);
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                foreach (var item in apiResponse.Ok().Result.Entities)
                {
                    Console.WriteLine($"Case ID: {item.Id}, Title: {item.Title}");
                }
            }
            else
            {
                Console.WriteLine($"Error: {apiResponse.StatusCode}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
          .ConfigureServices((context, services) =>
          {
              services.AddApi(options =>
              {
                  // the type of token here depends on the api security specifications
                  ApiKeyToken token = new("<your token>", ClientUtils.ApiKeyHeader.Token, "");
                  options.AddTokens(token);

                  // optionally choose the method the tokens will be provided with, default is RateLimitProvider
                  options.UseProvider<RateLimitProvider<ApiKeyToken>, ApiKeyToken>();

                  options.AddApiHttpClients();
              });
          });
    }
}
```

## Additional Documentation

For further information on the API endpoints and parameters, please refer to
the [Qase API documentation](https://developers.qase.io/).

## Requirements

- .NET Core >=2.0
- .NET Framework >=4.6.1
