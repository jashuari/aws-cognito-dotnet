using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using cognitocoreapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace cognitocoreapi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly AmazonCognitoIdentityProviderClient _client;
        private readonly IConfiguration _configuration;

        private readonly string _clientId;
        private readonly string _poolId;
        private readonly string _awsSecretKey;
        private readonly string _awsAccessKey;
        private readonly CognitoUserPool _userPool;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
            _awsSecretKey = _configuration["AWS:AwsSecretKey"];
            _awsAccessKey = _configuration["AWS:AwsAccessKey"];
            _clientId = _configuration["AWS:ClientId"];
            _poolId = _configuration["AWS:UserPoolId"];
            _client = new AmazonCognitoIdentityProviderClient(_awsAccessKey, _awsSecretKey, RegionEndpoint.EUCentral1);
            _userPool = new CognitoUserPool(poolID: _poolId, clientID: _clientId, provider: _client);
        }

        // POST api/account
        [HttpPost]
        public async Task Register([FromBody] RegisterModel user)
        {
            try
            {
                var signUpRequest = new SignUpRequest()
                { 
                    ClientId = _clientId,
                    Password = user.Password,
                    Username = user.Email,
                };

                var attributes = new List<AttributeType>(){
                    new AttributeType(){Name="email",Value=user.Email},
                    new AttributeType(){Name="name",Value=user.FirstName},
                    new AttributeType(){Name="family_name",Value=user.LastName},
                };
                

                foreach (var attribute in attributes)
                {
                    signUpRequest.UserAttributes.Add(attribute);
                }

                await _client.SignUpAsync(signUpRequest);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("{code}/{email}")]
        [AllowAnonymous]
        public async Task ActivateCode(string code, string email)
        {
            try
            {
                var confirmRequest = new ConfirmSignUpRequest()
                {
                    Username = "arijanitjashari@gmail.com",
                    ClientId = _clientId,
                    ConfirmationCode = code,
                };

                var confirmResult = await _client.ConfirmSignUpAsync(confirmRequest);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var user = new CognitoUser(userID: model.Email, clientID: _clientId, pool: _userPool, provider: _client, username: model.Email);

                AuthFlowResponse context = await user.StartWithSrpAuthAsync(new InitiateSrpAuthRequest()
                {
                    Password = model.Password
                }).ConfigureAwait(false);

                var userResponse = await _client.AdminGetUserAsync(new AdminGetUserRequest()
                {
                    Username = user.UserID,
                    UserPoolId = _userPool.PoolID
                }).ConfigureAwait(false);


                return new OkObjectResult(new
                {
                    metadata = new
                    {
                        token = context.AuthenticationResult.IdToken,
                        userName = user.Username
                    }
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}