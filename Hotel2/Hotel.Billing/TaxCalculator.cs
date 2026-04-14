namespace Hotel.Billing;

internal class TaxCalculator
{
    private const decimal AccommodationTvaRate = 0.12m;
    private const decimal TouristTaxPerPersonPerNight = 1.50m;

    public decimal CalculateTva(decimal subtotal) => subtotal * AccommodationTvaRate;

    public decimal CalculateTouristTax(int guestCount, int nights) =>
        guestCount * nights * TouristTaxPerPersonPerNight;
}
