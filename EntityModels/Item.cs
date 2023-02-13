using System;
using System.Collections.Generic;

namespace SelfCheckoutAPI.EntityModels;

public partial class Item
{
    public string Upc { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public double Price { get; set; }

    public double? Discount { get; set; }

    public int Quantity { get; set; }

    public int DepartmentId { get; set; }

    public bool IsTaxed { get; set; }

    public bool IsDiscontinued { get; set; }

    public virtual ICollection<CartItem> CartItems { get; } = new List<CartItem>();

    public virtual Department? Department { get; set; } = null!;

    public virtual ICollection<TransactionItem> TransactionItems { get; } = new List<TransactionItem>();
}
