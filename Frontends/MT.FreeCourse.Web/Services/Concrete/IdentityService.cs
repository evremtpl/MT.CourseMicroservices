﻿using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MT.FreeCourse.Shared.Dtos;
using MT.FreeCourse.Web.Models;
using MT.FreeCourse.Web.Services.Interfaces;
using MT.FreeCourse.Web.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace MT.FreeCourse.Web.Services.Concrete
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpclient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient httpclient, IHttpContextAccessor httpContextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpclient = httpclient;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            var discovery = await _httpclient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = discovery.TokenEndpoint
            };

            var token = await _httpclient.RequestRefreshTokenAsync(refreshTokenRequest);

            if(token.IsError) { return null; }


            var authenticationTokens= new List<AuthenticationToken>()
            {
                new AuthenticationToken {Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken {Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},

                new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            };



            var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync();
            var properties = authenticationResult.Properties;

            properties.StoreTokens(authenticationTokens);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties);

            return token;


        }

        public async Task RevokeRefreshToken()
        {
            var discovery = await _httpclient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new()
            {
                ClientId= _clientSettings.WebClientForUser.ClientId,
                ClientSecret= _clientSettings.WebClientForUser.ClientSecret,
                Address=discovery.RevocationEndpoint,
                Token=refreshToken,
                TokenTypeHint="refresh_token"
            };

            await _httpclient.RevokeTokenAsync(tokenRevocationRequest);
        }

        public async Task<Response<bool>> SignIn(SignInInput signInInput)
        {
            var discovery = await _httpclient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy {RequireHttps=false }
            });

            if(discovery.IsError)
            {
                throw discovery.Exception;
            }

            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = signInInput.Email,
                Password = signInInput.Password,
                Address = discovery.TokenEndpoint

            };

            var token = await _httpclient.RequestPasswordTokenAsync(passwordTokenRequest);

            if(token.IsError)
            {

                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();
                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions
                { PropertyNameCaseInsensitive = true });

                return Response<bool>.Fail(errorDto.Errors, 400);
            }

            var userInforequest = new UserInfoRequest
            {
                Token=token.AccessToken,
                Address=discovery.UserInfoEndpoint

            };
            var userInfo = await _httpclient.GetUserInfoAsync(userInforequest);

            if(userInfo.IsError)
            {
                throw userInfo.Exception;
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();

            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken {Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken {Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},

                new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            });


            authenticationProperties.IsPersistent = signInInput.IsRemember;

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return Response<bool>.Success(200);
        }
    }
}
