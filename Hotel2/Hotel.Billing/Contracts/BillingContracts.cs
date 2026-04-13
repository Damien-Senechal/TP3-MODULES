namespace Hotel.Billing.Contracts;

public interface IBillingService
{
    InvoiceDto GetInvoice(string reservationId);
}

public interface IBillingReservationRepository
{
    BillingReservationDto? GetById(string id);
}

public interface IBillingRoomRepository
{
    BillingRoomDto? GetById(string id);
}

public class BillingReservationDto
{
    public string Id { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public RoomTypeDto RoomType { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int GuestCount { get; set; }
    public int Nights => (CheckOut - CheckIn).Days;
}

public class BillingRoomDto
{
    public string Id { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public RoomTypeDto Type { get; set; }
}

public class InvoiceDto
{
    public string ReservationId { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public List<InvoiceLineDto> Lines { get; set; } = new();
    public decimal Total => Lines.Sum(l => l.Amount);

    public void Print()
    {
        Console.WriteLine($"  Invoice for {GuestName} (Reservation: {ReservationId})");
        foreach (var line in Lines)
            Console.WriteLine($"    {line.Description,-40} {line.Amount,10:C}");
        Console.WriteLine($"    {"TOTAL",-40} {Total,10:C}");
    }
}

public class InvoiceLineDto
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public enum RoomTypeDto { Standard, Suite, Family }
