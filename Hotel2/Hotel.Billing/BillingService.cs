namespace Hotel.Billing;

using Hotel.Billing.Contracts;

internal class InvoiceGenerator
{
    private readonly PricingStrategyFactory _pricingFactory;
    private readonly TaxCalculator _taxCalculator;
    private readonly IBillingRoomRepository _roomRepo;

    public InvoiceGenerator(
        PricingStrategyFactory pricingFactory,
        TaxCalculator taxCalculator,
        IBillingRoomRepository roomRepo)
    {
        _pricingFactory = pricingFactory;
        _taxCalculator = taxCalculator;
        _roomRepo = roomRepo;
    }

    public InvoiceDto Generate(BillingReservationDto reservation)
    {
        var room = _roomRepo.GetById(reservation.RoomId)
            ?? throw new Exception($"Room {reservation.RoomId} not found");

        var pricing = _pricingFactory.Create(reservation.RoomType);
        var nightRate = pricing.CalculateNightRate(room);
        var subtotal = nightRate * reservation.Nights;
        var tva = _taxCalculator.CalculateTva(subtotal);
        var touristTax = _taxCalculator.CalculateTouristTax(reservation.GuestCount, reservation.Nights);

        return new InvoiceDto
        {
            ReservationId = reservation.Id,
            GuestName = reservation.GuestName,
            Lines = new List<InvoiceLineDto>
            {
                new() { Description = $"{reservation.Nights} night(s) × {nightRate:C}/night", Amount = subtotal },
                new() { Description = "TVA (10%)", Amount = tva },
                new() { Description = $"Tourist tax ({reservation.GuestCount} pers. × {reservation.Nights} nights × 1.50€)", Amount = touristTax }
            }
        };
    }
}

internal class BillingService : IBillingService
{
    private readonly InvoiceGenerator _invoiceGenerator;
    private readonly IBillingReservationRepository _reservationRepo;

    public BillingService(InvoiceGenerator invoiceGenerator, IBillingReservationRepository reservationRepo)
    {
        _invoiceGenerator = invoiceGenerator;
        _reservationRepo = reservationRepo;
    }

    public InvoiceDto GetInvoice(string reservationId)
    {
        var reservation = _reservationRepo.GetById(reservationId)
            ?? throw new Exception($"Reservation {reservationId} not found");

        return _invoiceGenerator.Generate(reservation);
    }
}
