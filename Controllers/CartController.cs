using Microsoft.AspNetCore.Mvc;
using SelfCheckoutAPI.Services;
using SelfCheckoutAPI.EntityModels;
using Cart.Services;

namespace Cart.Controllers;

[ApiController]
[Route("[controller]")]
public class CartController : ControllerBase{
    
    CartService _cartService;
    public CartController(CartService cartService){
        this._cartService = cartService;
    }

    [HttpGet("GetCartById")]
    public ActionResult GetCartById(int id){
        var result = _cartService.GetCartById(id);
        if (result.Count() == 0){
            return Ok("got it but doesn't exist");
        }
       return Ok(result);
    }
}