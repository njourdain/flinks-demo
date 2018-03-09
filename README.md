# Flinks API challenge

## Introduction

This small web api uses the flinks.io sandbox to query the accounts of dummy customer f791f187-afad-4f43-b8ca-04a8995fa660 and create an accounts summary with the following information :

- LoginId: the uuid related to that specific user
- RequestId: the uuid related to this specific API request
- Holder informations: name and email
- The account number for all the Operations account
- The balance of all USD accounts
- The transaction ID of the largest credit transaction in the last 90 days

## How to build

First, clone this git repository. Then, in a terminal session, use the following commands :

- cd yourFlinksRepositoryFolder
- dotnet build Flinks.sln
- cd Flinks.Api/bin/Debug/netcoreapp2.0
- dotnet Flinks.Api.dll

In another terminal session, use the following command :

- curl http://localhost:5000/api/summary

## USD accounts balance

The USD accounts balances are rounded and return as an integer. To get them as numbers, checkout branch number-instead-of-int.