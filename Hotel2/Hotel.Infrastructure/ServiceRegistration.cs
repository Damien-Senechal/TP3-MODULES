namespace Hotel.Infrastructure;

using Hotel.Booking.Contracts;
using Hotel.Billing.Contracts;
using Hotel.Housekeeping.Contracts;
using Microsoft.Extensions.DependencyInjection;

using BookingRoomType = Hotel.Booking.Contracts.RoomTypeDto;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureModule(
        this IServiceCollection services,
        List<(string Id, BookingRoomType Type, int Capacity, decimal BasePrice)> rooms)
    {
        services.AddSingleton<InMemoryStore>();
        services.AddSingleton<IBookingReservationRepository>(sp => sp.GetRequiredService<InMemoryStore>());
        services.AddSingleton<IBillingReservationRepository>(sp => sp.GetRequiredService<InMemoryStore>());
        services.AddSingleton<IHousekeepingReservationRepository>(sp => sp.GetRequiredService<InMemoryStore>());

        services.AddSingleton<InMemoryRoomStore>(_ => new InMemoryRoomStore(rooms));
        services.AddSingleton<IBookingRoomRepository>(sp => sp.GetRequiredService<InMemoryRoomStore>());
        services.AddSingleton<IBillingRoomRepository>(sp => sp.GetRequiredService<InMemoryRoomStore>());

        services.AddSingleton<IConfirmationSender, EmailSender>();
        services.AddSingleton<ICleaningNotifier, SmsSender>();

        return services;
    }
}
