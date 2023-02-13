using System;
using System.Collections.Generic;

namespace SelfCheckoutAPI.EntityModels;

public partial class Transaction
{
    public int Id { get; set; }

    public DateTime DatePurchased { get; set; }

    public virtual ICollection<TransactionItem> TransactionItems { get; } = new List<TransactionItem>();
}
