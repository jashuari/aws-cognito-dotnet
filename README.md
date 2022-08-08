# **AWS Cognito Use Cases**

Register and Authenticate with Amazon Cognito User Pools.

#### Techincal API Design

The API was build using C# .Net Core 6.0.

**Prerequisite**

1. Need to have an AWS account
2. Create a User Pool in cognito service

## 
**Register**

Register a user in aws cognito

post /api/account/register

A verification code will be sent to your email.
 ## 
**ActivateEmail**

Activates the user.

post /api/Account/ActivateCode/{code}/{email}

Write the code and the email to Enable the account.
## 
**Login**
 
post /api/Account/Login

Login with email and password.

*********************************************
##### **P.S.: Also, the API can be tested using Swagger. Once the API is up and running: http://YOUR-HOST/swagger**
##### **For more infos visit https://aws.amazon.com/blogs/mobile/use-csharp-to-register-and-authenticate-with-amazon-cognito-user-pools/**
*********************************************
