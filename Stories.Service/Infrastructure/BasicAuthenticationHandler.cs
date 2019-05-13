using Stories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Stories.Service.Infrastructure
{
    public class BasicAuthenticationHandler : DelegatingHandler
    {
        private const string basicAuthenticationResponseHeader = "WWW-Authenticate";
        private const string basicAuthenticationResponseHeaderValue = "Basic";

        private readonly CustomPrincipalProvider principalProvider;

        public BasicAuthenticationHandler(CustomPrincipalProvider principalProvider)
        {
            this.principalProvider = principalProvider;
        }

        private Credentials ParseAuthorizationHeader(string authHeader)
        {
            var credentials = Encoding.ASCII.GetString(Convert.FromBase64String(authHeader)).Split(':');

            if (credentials.Length != 2 ||
                credentials.Any(string.IsNullOrEmpty))
                return null;

            return new Credentials()
            {
                Email = credentials[0],
                Token = credentials[1]
            };
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authValue = request.Headers.Authorization;

            if (authValue != null && !string.IsNullOrWhiteSpace(authValue.Parameter))
            {
                var parsedCredentials = ParseAuthorizationHeader(authValue.Parameter);

                request.GetRequestContext().Principal =
                    await principalProvider.CreatePrincipals(parsedCredentials.Email, parsedCredentials.Token);
            }

            return await base.SendAsync(request, cancellationToken).ContinueWith(task =>
            {
                var response = task.Result;
                if (response.StatusCode == HttpStatusCode.Unauthorized &&
                !response.Headers.Contains(basicAuthenticationResponseHeader))
                    response.Headers.Add(basicAuthenticationResponseHeader, basicAuthenticationResponseHeaderValue);

                return response;
            });
        }
    }
}