public class TransactionItemDto{
    public int TransactionId { get; set; }

    public string ItemId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public double PriceBought { get; set; }

    public int QuantityBought { get; set; }
}