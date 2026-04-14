namespace Hotel.Infrastructure;

using Hotel.Booking.Contracts;

public class PushNotificationSender : IConfirmationSender
{
    public void SendBookingConfirmation(string email, string guestName, string reservationId,
        DateTime checkIn, DateTime checkOut, string roomId)
    {
        Console.WriteLine($"  [PUSH] To: {email}");
        Console.WriteLine($"    Booking confirmed for {guestName}");
        Console.WriteLine($"    Reservation: {reservationId} | Room: {roomId}");
        Console.WriteLine($"    {checkIn:d} → {checkOut:d}");
    }
}