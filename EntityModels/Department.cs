using System;
using System.Collections.Generic;

namespace SelfCheckoutAPI.EntityModels;

public partial class Department
{
    public int Id { get; set; }

    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<Item> Items { get; } = new List<Item>();
}
