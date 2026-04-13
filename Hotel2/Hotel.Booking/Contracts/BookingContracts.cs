namespace Hotel.Booking.Contracts;

public interface IBookingService
{
    ReservationDto CreateReservation(string guestName, RoomTypeDto roomType,
        DateTime checkIn, DateTime checkOut, int guestCount,
        string email, string phone, string cancellationPolicy = "Flexible");
    void CheckIn(string reservationId);
    void CheckOut(string reservationId);
}

public interface IBookingReservationRepository
{
    void Add(ReservationDto reservation);
    ReservationDto? GetById(string id);
    List<ReservationDto> GetAll();
    void Update(ReservationDto reservation);
}

public interface IBookingRoomRepository
{
    RoomDto? GetById(string id);
    List<RoomDto> GetAvailable(DateTime from, DateTime to, List<ReservationDto> existing);
}

public interface IConfirmationSender
{
    void SendBookingConfirmation(string email, string guestName, string reservationId,
        DateTime checkIn, DateTime checkOut, string roomId);
}

public class ReservationDto
{
    public string Id { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public RoomTypeDto RoomType { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int GuestCount { get; set; }
    public string Status { get; set; } = "Confirmed";
    public string CancellationPolicyName { get; set; } = "Flexible";
    public string GuestEmail { get; set; } = string.Empty;
    public string GuestPhone { get; set; } = string.Empty;
    public int Nights => (CheckOut - CheckIn).Days;
}

public class RoomDto
{
    public string Id { get; set; } = string.Empty;
    public RoomTypeDto Type { get; set; }
    public int Capacity { get; set; }
    public decimal BasePrice { get; set; }
}

public enum RoomTypeDto { Standard, Suite, Family }
