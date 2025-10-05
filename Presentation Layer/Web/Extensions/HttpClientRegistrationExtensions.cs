namespace Web.Extensions
{
    public static class HttpClientRegistrationExtensions
    {
        public static void AddApiHttpClient<TClient, TImplementation>(this IServiceCollection services, IConfiguration config)
        where TClient : class
        where TImplementation : class, TClient
        {
            services.AddHttpClient<TClient, TImplementation>(client =>
            {
                client.BaseAddress = new Uri(config["Services:URL_API"]);
            });
        }
    }
}
