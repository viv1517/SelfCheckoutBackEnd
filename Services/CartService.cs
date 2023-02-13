using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SelfCheckoutAPI.EntityModels;


namespace Cart.Services;

public class CartService{

    SelfCheckoutContext _context;

    public CartService(SelfCheckoutContext context){
        this._context = context;
    }

    public  List<CartItemDto> GetCartById(int id){
        if(!_context.CartItems.Any(cart => cart.Id == id)){
            return new List<CartItemDto>();
        }
        var cartItems = _context.CartItems.Include(item => item.Item).Where(cart => cart.Id == id).ToList();

        return cartItems.Select(item => (CartItemDto)item).ToList();
    }
}