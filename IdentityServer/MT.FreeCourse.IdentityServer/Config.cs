// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace MT.FreeCourse.IdentityServer
{
    public static class Config
    {

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_catalog"){ Scopes={ "catalog_fullpermission" } },
             new ApiResource("resource_photo_stock"){ Scopes={ "photo_stock_fullpermission" } },
             new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                        new IdentityResources.OpenId(),
                        new IdentityResources.Profile(),
                       new IdentityResource(){ Name="roles", DisplayName="Roles", Description="User Roles" , UserClaims= new []{ "role"} }
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
               new ApiScope("catalog_fullpermission","catalog api için full erişim"),
               new ApiScope("photo_stock_fullpermission","photo_stock  api için full erişim"),
               new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {

                new Client
                {
                   ClientName="Asp.Net Core MVC",
                   ClientId="webMvcClient",
                   AllowOfflineAccess=true;
                   ClientSecrets = { new  Secret("secret".Sha256()) },
                   AllowedGrantTypes={ new string (GrantType.ClientCredentials) },
                   AllowedScopes={ "catalog_fullpermission", "photo_stock_fullpermission", IdentityServerConstants.LocalApi.ScopeName }
                },

                    new Client
                {
                   ClientName="Asp.Net Core MVC",
                   ClientId="webMvcClientForUser",
                   ClientSecrets = { new  Secret("secret".Sha256()) },
                   AllowedGrantTypes={ new string (GrantType.ResourceOwnerPassword) },
                   AllowedScopes=
                        {
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "roles" //kullanıcı online olmasa bile refresh tokenla access token alınabilir
                        },
                   AccessTokenLifetime=1*60*60,
                   RefreshTokenExpiration=TokenExpiration.Absolute,
                   AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                   RefreshTokenUsage=TokenUsage.ReUse

                },
            };
    }
}