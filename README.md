# SKY API - Payments API Sample App

## Overview

This sample application demonstrates integrating with Blackbaud Payments API via several examples, both backend and front end. 

## Server

### .NET Web API

#### Run locally:

- Download and install [.NET Core SDK](https://www.microsoft.com/net/core/)
- Open Terminal/Command Prompt and type:
```
$  git clone https://github.com/blackbaud/payments-api-sample.git
$  cd payments-api-sample/server/dotnet
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

## Client - Admin

### Angular

The backend API depends on a front-end application to provide user interactions for login flows and other tasks. This angular application provides a simple starting point for that. If you haven't already, you can get your local environment set up by following [these docs](https://angular.dev/tutorials/first-app#local-development-environment).

Once you are set up, run the app locally by running these commands:

```
npm i
ng serve
```

The output of `ng serve` will give you a link to navigate to the running app in your browser. 

**Note**: The API assumes the application is running on port 4200 (the Angular default).

## Client - Public

### Angular

#### Run locally

The client side public-facing application integrated with the new Blackbaud Checkout is built in angular. This is the payment page that consumers will visit to make a donation or purchase. If you haven't already, you can get your local environment set up by following [these docs](https://angular.dev/tutorials/first-app#local-development-environment).

Once you are set up, run the app locally by running these commands:

```
npm i
ng serve
```

The output of `ng serve` will give you a link to navigate to the running app in your browser.

### HTML
#### Run locally

To serve the static html client code locally, the easiest option is to use [http-server](https://www.npmjs.com/package/http-server). `http-server` will serve the static content for access in the browser.

- Navigate into directory
```
cd client/html
```
- Run `http-server`
```
npx http-server .
```