using System;
using System.Collections.Generic;

namespace SelfCheckoutAPI.EntityModels;

public partial class CartItem
{
    public int Id { get; set; }

    public string ItemId { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual Item Item { get; set; } = null!;
}
