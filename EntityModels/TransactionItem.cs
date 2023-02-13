using System;
using System.Collections.Generic;

namespace SelfCheckoutAPI.EntityModels;

public partial class TransactionItem
{
    public int TransactionId { get; set; }

    public string ItemId { get; set; } = null!;

    public double PriceBought { get; set; }

    public int QuantityBought { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}
