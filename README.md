# SKY API - Payments API Sample App

## Server

### Run locally:

- Download and install [.NET Core SDK](https://www.microsoft.com/net/core/)
- Open Terminal/Command Prompt and type:
```
$  git clone https://github.com/blackbaud/payments-api-sample.git
$  cd payments-api-sample/server/dotnet-blazor
```
- Duplicate **appsettings.json-sample** as **appsettings.Development.json** and fill in the missing values (all required).
```
{
    "AppSettings": {
        "AuthClientId": "<Your developer app ID>",
        "AuthClientSecret": "<Your developer app secret>",
        "AuthRedirectUri": "https://localhost:5001/auth/callback",
        "GeneralSubscriptionKey": "<Your Standard subscription key>",
        "PaymentsSubscriptionKey": "<Your Payments subscription key>"
    }
}
```
- Open Terminal/Command Prompt and type:
```
dotnet restore
```
- On a Mac, type:
```
export ASPNETCORE_ENVIRONMENT=Development && dotnet run
```
- On a PC, type:
```
set ASPNETCORE_ENVIRONMENT=Development && dotnet run
```

Visit [https://localhost:5001/](https://localhost:5001/)

## Client

### Run locally

To serve the static html client code locally, the easiest option is to use [http-server](https://www.npmjs.com/package/http-server) and [ngrok](https://ngrok.com/docs/getting-started/). `http-server` will serve the static content for access in the browser, while `ngrok` will allow use of SSL, which is required to use Blackbaud Checkout.

- Navigate into directory
```
cd client/html
```
- Run `http-server`
```
npx http-server .
```
- Run ngrok
```
ngrok http http://localhost:8080
```
- Access page at url provided by ngrok, i.e., `https://0759-98-214-92-44.ngrok-free.app`