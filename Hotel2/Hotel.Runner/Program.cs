using Microsoft.Extensions.DependencyInjection;
using Hotel.Booking;
using Hotel.Billing;
using Hotel.Housekeeping;
using Hotel.Infrastructure;
using Hotel.Booking.Contracts;
using Hotel.Billing.Contracts;
using Hotel.Housekeeping.Contracts;

using RoomTypeDto = Hotel.Booking.Contracts.RoomTypeDto;

Console.WriteLine("=== Le Mas des Oliviers — Hotel Management ===\n");

var rooms = new List<(string Id, RoomTypeDto Type, int Capacity, decimal BasePrice)>
{
    ("101", RoomTypeDto.Standard, 2, 80m),
    ("102", RoomTypeDto.Standard, 2, 80m),
    ("201", RoomTypeDto.Suite,    2, 200m),
    ("301", RoomTypeDto.Family,   4, 120m),
};

var services = new ServiceCollection();

services.AddInfrastructureModule(rooms);
services.AddBookingModule();
services.AddBillingModule();
services.AddHousekeepingModule();

var provider = services.BuildServiceProvider();

var bookingService      = provider.GetRequiredService<IBookingService>();
var billingService      = provider.GetRequiredService<IBillingService>();
var housekeepingService = provider.GetRequiredService<IHousekeepingService>();

// --- Scenario 1: Create reservations ---
Console.WriteLine("--- Creating reservations ---\n");

var alice = bookingService.CreateReservation(
    "Alice Martin", RoomTypeDto.Standard,
    new DateTime(2025, 6, 15), new DateTime(2025, 6, 18),
    2, "alice@example.com", "+33612345001");

Console.WriteLine();

var bob = bookingService.CreateReservation(
    "Bob Dupont", RoomTypeDto.Suite,
    new DateTime(2025, 6, 15), new DateTime(2025, 6, 22),
    2, "bob@example.com", "+33612345002");

Console.WriteLine();

var durand = bookingService.CreateReservation(
    "Famille Durand", RoomTypeDto.Family,
    new DateTime(2025, 6, 20), new DateTime(2025, 6, 25),
    4, "durand@example.com", "+33612345003");

// --- Scenario 2: Conflict ---
Console.WriteLine("\n--- Attempting conflicting reservation ---\n");
try
{
    bookingService.CreateReservation(
        "Charlie Noir", RoomTypeDto.Standard,
        new DateTime(2025, 6, 16), new DateTime(2025, 6, 19),
        2, "charlie@example.com", "+33612345004");
}
catch (Exception ex)
{
    Console.WriteLine($"  Expected conflict: {ex.Message}");
}

// --- Scenario 3: Check-in ---
Console.WriteLine("\n--- Check-in ---\n");
bookingService.CheckIn(alice.Id);

// --- Scenario 4: Invoice ---
Console.WriteLine("\n--- Invoice for Bob ---\n");
var invoice = billingService.GetInvoice(bob.Id);
invoice.Print();

// --- Scenario 5: Housekeeping schedule ---
Console.WriteLine("\n--- Housekeeping schedule for June 18 ---\n");
var schedule = housekeepingService.GetSchedule(new DateTime(2025, 6, 18));
if (schedule.Count == 0)
    Console.WriteLine("  No cleaning tasks for this date.");
else
    foreach (var task in schedule)
        Console.WriteLine($"  [{task.Type}] Room {task.RoomId} (Reservation: {task.ReservationId})");

// --- Scenario 6: Check-out ---
Console.WriteLine("\n--- Check-out ---\n");
bookingService.CheckOut(alice.Id);

Console.WriteLine("\n=== Done ===");
