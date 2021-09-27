using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace RoamlerLocationSearch.WebAPI
{
    public sealed class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Inject the neccessary header to the request
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext context)
        {
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
            context.Response.Headers.Add("referrer-policy", new StringValues("no-referrer-when-downgrade"));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-XSS-Protection
            context.Response.Headers.Add("x-xss-protection", new StringValues("1; mode=block"));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Expect-CT
            // You can use https://report-uri.com/ to get notified when a misissued certificate is detected
            context.Response.Headers.Add("Expect-CT",
                new StringValues("max-age=3600, enforce, report-uri=\"https://example.report-uri.com/r/d/ct/enforce\""));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Feature-Policy
            // https://github.com/w3c/webappsec-feature-policy/blob/master/features.md
            // https://developers.google.com/web/updates/2018/06/feature-policy
            context.Response.Headers.Add("Permissions-Policy", "geolocation=()");


            // https://developer.mozilla.org/en-US/docs/Web/HTTP/CSP
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
            context.Response.Headers.Add("Content-Security-Policy", new StringValues(
                "base-uri 'none';" +
                "block-all-mixed-content;" +
                "child-src 'none';" +
                "connect-src 'none';" +
                "default-src 'none';" +
                "font-src 'none';" +
                "form-action 'none';" +
                "frame-ancestors 'none';" +
                "frame-src 'none';" +
                "img-src 'none';" +
                "manifest-src 'none';" +
                "media-src 'none';" +
                "object-src 'none';" +
                "sandbox;" +
                "script-src 'none';" +
                "script-src-attr 'none';" +
                "script-src-elem 'none';" +
                "style-src 'none';" +
                "style-src-attr 'none';" +
                "style-src-elem 'none';" +
                "upgrade-insecure-requests;" +
                "worker-src 'none';"
                ));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
            context.Response.Headers.Add("x-content-type-options", new StringValues("nosniff"));

            return _next(context);
        }
    }
}