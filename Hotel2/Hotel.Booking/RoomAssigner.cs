namespace Hotel.Booking;

using Hotel.Booking.Contracts;

internal class RoomAssigner
{
    private readonly IBookingRoomRepository _roomRepo;
    private readonly IBookingReservationRepository _reservationRepo;

    public RoomAssigner(IBookingRoomRepository roomRepo, IBookingReservationRepository reservationRepo)
    {
        _roomRepo = roomRepo;
        _reservationRepo = reservationRepo;
    }

    public RoomDto? FindAvailableRoom(RoomTypeDto type, DateTime checkIn, DateTime checkOut, int guestCount)
    {
        var existing = _reservationRepo.GetAll();
        var available = _roomRepo.GetAvailable(checkIn, checkOut, existing);
        return available.FirstOrDefault(r => r.Type == type && r.Capacity >= guestCount);
    }
}
