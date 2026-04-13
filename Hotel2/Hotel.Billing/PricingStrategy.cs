namespace Hotel.Billing;

using Hotel.Billing.Contracts;

internal interface IPricingStrategy
{
    decimal CalculateNightRate(BillingRoomDto room);
}

internal class StandardPricingStrategy : IPricingStrategy
{
    public decimal CalculateNightRate(BillingRoomDto room) => room.BasePrice;
}

internal class SuitePricingStrategy : IPricingStrategy
{
    public decimal CalculateNightRate(BillingRoomDto room) => room.BasePrice * 1.2m;
}

internal class FamilyPricingStrategy : IPricingStrategy
{
    public decimal CalculateNightRate(BillingRoomDto room) => room.BasePrice * 0.9m;
}

internal class PricingStrategyFactory
{
    public IPricingStrategy Create(RoomTypeDto roomType) => roomType switch
    {
        RoomTypeDto.Standard => new StandardPricingStrategy(),
        RoomTypeDto.Suite    => new SuitePricingStrategy(),
        RoomTypeDto.Family   => new FamilyPricingStrategy(),
        _                    => new StandardPricingStrategy()
    };
}
