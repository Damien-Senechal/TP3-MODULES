namespace Hotel.Infrastructure;

using Hotel.Booking.Contracts;
using Hotel.Billing.Contracts;
using Hotel.Housekeeping.Contracts;

using BookingRoomType = Hotel.Booking.Contracts.RoomTypeDto;
using BillingRoomType = Hotel.Billing.Contracts.RoomTypeDto;

public class InMemoryStore :
    IBookingReservationRepository,
    IBillingReservationRepository,
    IHousekeepingReservationRepository
{
    private readonly Dictionary<string, BookingData> _store = new();

    private record BookingData(
        string Id, string GuestName, string RoomId,
        BookingRoomType RoomType, DateTime CheckIn, DateTime CheckOut,
        int GuestCount, string Status, string CancellationPolicyName,
        string GuestEmail, string GuestPhone);

    public void Add(ReservationDto r) =>
        _store[r.Id] = new BookingData(r.Id, r.GuestName, r.RoomId,
            r.RoomType, r.CheckIn, r.CheckOut, r.GuestCount,
            r.Status, r.CancellationPolicyName, r.GuestEmail, r.GuestPhone);

    public ReservationDto? GetById(string id) =>
        _store.TryGetValue(id, out var d) ? ToBookingDto(d) : null;

    public List<ReservationDto> GetAll() =>
        _store.Values.Select(ToBookingDto).ToList();

    public void Update(ReservationDto r) => Add(r);

    BillingReservationDto? IBillingReservationRepository.GetById(string id) =>
        _store.TryGetValue(id, out var d) ? ToBillingDto(d) : null;

    public List<HousekeepingReservationDto> GetByDateRange(DateTime from, DateTime to) =>
        _store.Values
            .Where(r => r.Status != "Cancelled" && r.CheckIn < to && r.CheckOut > from)
            .Select(ToHousekeepingDto)
            .ToList();

    private static ReservationDto ToBookingDto(BookingData d) => new()
    {
        Id = d.Id, GuestName = d.GuestName, RoomId = d.RoomId,
        RoomType = d.RoomType, CheckIn = d.CheckIn, CheckOut = d.CheckOut,
        GuestCount = d.GuestCount, Status = d.Status,
        CancellationPolicyName = d.CancellationPolicyName,
        GuestEmail = d.GuestEmail, GuestPhone = d.GuestPhone
    };

    private static BillingReservationDto ToBillingDto(BookingData d) => new()
    {
        Id = d.Id, GuestName = d.GuestName, RoomId = d.RoomId,
        RoomType = (BillingRoomType)(int)d.RoomType,
        CheckIn = d.CheckIn, CheckOut = d.CheckOut, GuestCount = d.GuestCount
    };

    private static HousekeepingReservationDto ToHousekeepingDto(BookingData d) => new()
    {
        Id = d.Id, RoomId = d.RoomId, CheckIn = d.CheckIn, CheckOut = d.CheckOut
    };
}
