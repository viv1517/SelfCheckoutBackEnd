using SelfCheckoutAPI.EntityModels;

public class Receipt{
    public Double total { get; set; }

    public TransactionItem[] ?items { get; set; }
}
