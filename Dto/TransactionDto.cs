using SelfCheckoutAPI.EntityModels;

public class TransactionDto{
    public int Id { get; set; }

    public string purchaseDate { get; set; }

    public List<TransactionItemDto> transactionItems { get; set; }

    public TransactionDto(string purchaseDate, List<TransactionItemDto> transactionItems){
        this.purchaseDate = purchaseDate;
        this.transactionItems = transactionItems;
    }
}