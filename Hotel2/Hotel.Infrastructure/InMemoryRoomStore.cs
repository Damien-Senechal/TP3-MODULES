namespace Hotel.Infrastructure;

using Hotel.Booking.Contracts;
using Hotel.Billing.Contracts;

using BookingRoomType = Hotel.Booking.Contracts.RoomTypeDto;
using BillingRoomType = Hotel.Billing.Contracts.RoomTypeDto;

public class InMemoryRoomStore : IBookingRoomRepository, IBillingRoomRepository
{
    private readonly List<RoomData> _rooms;

    private record RoomData(string Id, BookingRoomType Type, int Capacity, decimal BasePrice);

    public InMemoryRoomStore(List<(string Id, BookingRoomType Type, int Capacity, decimal BasePrice)> rooms)
    {
        _rooms = rooms.Select(r => new RoomData(r.Id, r.Type, r.Capacity, r.BasePrice)).ToList();
    }

    public RoomDto? GetById(string id)
    {
        var r = _rooms.FirstOrDefault(x => x.Id == id);
        return r == null ? null : new RoomDto { Id = r.Id, Type = r.Type, Capacity = r.Capacity, BasePrice = r.BasePrice };
    }

    public List<RoomDto> GetAvailable(DateTime from, DateTime to, List<ReservationDto> existing)
    {
        var bookedIds = existing
            .Where(r => r.Status != "Cancelled" && r.CheckIn < to && r.CheckOut > from)
            .Select(r => r.RoomId)
            .ToHashSet();

        return _rooms
            .Where(r => !bookedIds.Contains(r.Id))
            .Select(r => new RoomDto { Id = r.Id, Type = r.Type, Capacity = r.Capacity, BasePrice = r.BasePrice })
            .ToList();
    }

    BillingRoomDto? IBillingRoomRepository.GetById(string id)
    {
        var r = _rooms.FirstOrDefault(x => x.Id == id);
        return r == null ? null : new BillingRoomDto
        {
            Id = r.Id,
            BasePrice = r.BasePrice,
            Type = (BillingRoomType)(int)r.Type
        };
    }
}
