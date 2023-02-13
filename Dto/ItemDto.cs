public class ItemDto{
    public string Upc { get; set; } = null!;
    public string ItemName { get; set; } = null!;

    public double Price { get; set; }

    public double? Discount { get; set; }

    public int Quantity { get; set; }

    public string DepartmentName { get; set; }

    public bool IsTaxed { get; set; }

    public bool IsDiscontinued { get; set; }
}