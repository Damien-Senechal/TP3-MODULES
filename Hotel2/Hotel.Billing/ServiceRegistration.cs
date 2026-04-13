namespace Hotel.Billing;

using Hotel.Billing.Contracts;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddBillingModule(this IServiceCollection services)
    {
        services.AddSingleton<TaxCalculator>();
        services.AddSingleton<PricingStrategyFactory>();
        services.AddSingleton<InvoiceGenerator>();
        services.AddSingleton<IBillingService, BillingService>();
        return services;
    }
}
