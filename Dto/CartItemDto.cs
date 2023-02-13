using SelfCheckoutAPI.EntityModels;

public class CartItemDto{
    public string item_name { get; set; }

    public double price { get; set; }

    public int Quantity { get; set; }

    public static explicit operator CartItemDto(CartItem item){
        return new CartItemDto(){
                item_name = item.Item.ItemName,
                price = item.Item.Price,
                Quantity = item.Quantity
            };
    }
}