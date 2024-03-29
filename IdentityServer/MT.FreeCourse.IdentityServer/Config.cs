﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
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
            new ApiResource("resource_basket"){ Scopes={ "basket_fullpermission" } },
            new ApiResource("resource_photo_stock"){ Scopes={ "photo_stock_fullpermission" } },
            new ApiResource("resource_discount"){ Scopes={ "discount_fullpermission" } },
            new ApiResource("resource_order"){ Scopes={ "order_fullpermission" } },
            new ApiResource("resource_fakepayment"){ Scopes={ "fakepayment_fullpermission" } },
            new ApiResource("resource_gateway"){ Scopes={ "gateway_fullpermission" } },
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
               new ApiScope("basket_fullpermission","basket için full erişim"),
               new ApiScope("discount_read","discount  api için read erişim"),
               new ApiScope("discount_fullpermission","discount  api için full erişim"),
               new ApiScope("order_fullpermission","order  api için full erişim"),
               new ApiScope("fakepayment_fullpermission","fakepayment  api için full erişim"),
               new ApiScope("gateway_fullpermission","gateway  api için full erişim"),
               new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {

                new Client
                {
                   ClientName="Asp.Net Core MVC",
                   ClientId="webMvcClient",

                   ClientSecrets = { new  Secret("secret".Sha256()) },
                   AllowedGrantTypes={ new string (GrantType.ClientCredentials) },
                   AllowedScopes={ "catalog_fullpermission", "photo_stock_fullpermission", "gateway_fullpermission" ,IdentityServerConstants.LocalApi.ScopeName }
                },

                    new Client
                {
                   ClientName="Asp.Net Core MVC",
                   ClientId="webMvcClientForUser",

                   AllowOfflineAccess=true,
                   ClientSecrets = { new  Secret("secrets".Sha256()) },
                   AllowedGrantTypes={ new string (GrantType.ResourceOwnerPassword) },
                   AllowedScopes=
                        {
                        "basket_fullpermission","discount_fullpermission","order_fullpermission","fakepayment_fullpermission","gateway_fullpermission",
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.LocalApi.ScopeName,
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