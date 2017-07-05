using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Authority.IdentityServer.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddTemporarySigningCredential()
                .AddInMemoryIdentityResources(new[] {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResource
                    {
                        Name = JwtClaimTypes.Role,
                        UserClaims = new [] { JwtClaimTypes.Role },
                    }
                })
                .AddInMemoryApiResources(new[] {
                    new ApiResource
                    {
                        Name = "lending-ui",
                        UserClaims = new [] { JwtClaimTypes.Role },
                        Scopes = new []{
                            new Scope("lending-ui.account-manager")
                        }
                    },
                    new ApiResource
                    {
                        Name = "lending-admin-ui",
                        UserClaims = new [] { JwtClaimTypes.Role },
                        Scopes = new []{
                            new Scope("lending-admin-ui.account-manager")
                        }
                    }
                })
                .AddInMemoryClients(new[] {
                    new Client
                    {
                        ClientId = "mvc-client",
                        ClientName = "mvc-client",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        RedirectUris = new [] { "http://localhost:5001/signin-oidc" },
                        ClientSecrets = new[] {
                            new Secret("mvc-client-secret".Sha256())
                        },
                        AllowedScopes = new[]{
                            "lending-admin-ui.account-manager"
                        }
                    }
                })
                .AddTestUsers(new List<TestUser> {
                    new TestUser
                    {
                        Username = "foo",
                        Password = "foo-pass",
                        Claims = new []{
                            new Claim(JwtClaimTypes.Role, "super-duper")
                        }
                    }
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Console.Title = Program.Title;
            loggerFactory.AddConsole(LogLevel.Debug);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.Run(async context => await context.Response.WriteAsync($"You are running {Program.Title}"));
        }
    }
}
