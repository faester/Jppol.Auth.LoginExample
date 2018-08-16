using Jppol.Auth.LoginExample.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Jppol.Auth.LoginExample
{
    public class Startup
    {

	public IConfiguration Configuration {get;set;}

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
	    var builder = new ConfigurationBuilder()
		    .SetBasePath(Directory.GetCurrentDirectory())
		    .AddJsonFile("appsettings.json");

	    Configuration = builder.Build();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
	    	Console.WriteLine(context.GetType().Name);
		if (context.Request.Path.Equals("/login")) 
		{
			var codeChallenge = new CodeChallenge();
			var authParameters = AuthorizationParameters(codeChallenge);
		        Uri endpoint = new Uri(new Uri(Configuration["auth:endpoint"]), "/connect/authorize?" + authParameters.GetQueryString());
			context.Response.StatusCode = 302;
			context.Response.Headers["Location"] = endpoint.ToString();
			context.Response.Cookies.Append("codeChallenge", codeChallenge.Verifier);
		}
		else if (context.Request.Path.Equals("/login-implicit")) 
		{
			var codeChallenge = new CodeChallenge();
			var authParameters = AuthorizationParameters(codeChallenge, "token id_token");
			authParameters["scope"] = "openid access-features SAAPI userservice";
			Uri endpoint = new Uri(new Uri(Configuration["auth:endpoint"]), "/connect/authorize?" + authParameters.GetQueryString());
			context.Response.StatusCode = 302;
			context.Response.Headers["Location"] = endpoint.ToString();
			context.Response.Cookies.Append("codeChallenge", codeChallenge.Verifier);
		}
		else if (context.Request.Path.Equals("/logout")) 
		{
		        Uri endpoint = new Uri(new Uri(Configuration["auth:endpoint"]), "/connect/endsession" + context.Request.QueryString);
			context.Response.StatusCode = 302;
			context.Response.Headers["Location"] = endpoint.ToString();
		}
		else 
		{
	                await context.Response.SendFileAsync("./wwwroot/receive.html");
		}
            });
        }

	private Dictionary<string, string> AuthorizationParameters(CodeChallenge codeChallenge, string responseType = "code id_token token")
	{
		var qsParameters = new Dictionary<string, string>();
		qsParameters.Add("response_type", responseType);
		qsParameters.Add("redirect_uri", Configuration["auth:redirect"]);
		qsParameters.Add("client_id", Configuration["auth:clientId"]);
		qsParameters.Add("error_uri", Configuration["auth:error"]);
		qsParameters.Add("code_challenge_method", codeChallenge.Method);
		qsParameters.Add("code_challenge", codeChallenge.Challenge);
		qsParameters.Add("scope", Configuration["auth:scope"]);
		qsParameters.Add("nonce", Guid.NewGuid().ToString());
//		qsParameters.Add("response_mode", "form_post");
		qsParameters.Add("state", "i am statefull");
		return qsParameters;
	}
    }

    internal static class DictionaryExtensions
    {
	    public static string GetQueryString(this Dictionary<string, string> input) 
	    {
		var queryStringParameters = new QueryStringParameters();
		foreach (var kvp in input){
			queryStringParameters.Add(kvp.Key, kvp.Value);
		}

		return queryStringParameters.GetQueryString();
	    }
    }
}
