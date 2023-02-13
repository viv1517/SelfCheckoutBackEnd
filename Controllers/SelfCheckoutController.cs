using Microsoft.AspNetCore.Mvc;
using SelfCheckoutAPI.Services;
using SelfCheckoutAPI.EntityModels;

namespace SelfCheckoutAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SelfCheckoutController : ControllerBase{
    SelfCheckoutService _selfCheckoutService;

    public SelfCheckoutController(SelfCheckoutService selfCheckoutService){
        this._selfCheckoutService = selfCheckoutService;
    }

     [HttpGet("GetItemsByUPC")]
    public ActionResult GetItemsByUPC(string upc){
       return Ok(_selfCheckoutService.GetItemsByUPC(upc));
    }

    [HttpGet("GetItemsByDepartment")]
    public ActionResult GetItemsByDepartment(string department){
       return Ok(_selfCheckoutService.GetItemsByDepartment(department));
    }

    [HttpGet("SetDiscountByUPC")]
    public ActionResult SetDiscountByUPC(string upc, double discount){
        _selfCheckoutService.SetDiscountByUPC(upc, discount);
        return Ok();
    }

    [HttpPost("SetDiscountByDepartment")]
    public ActionResult SetDiscountByDepartment(string departmentName, double discount){
       _selfCheckoutService.SetDiscountByDepartment(departmentName, discount);
       return Ok();
    }

    [HttpPost("AddItem")]
    public ActionResult AddItem([FromBody] ItemDto item){
       
       return Ok(_selfCheckoutService.AddItem(item));
    }

    [HttpGet("RemoveItem")]
    public ActionResult RemoveItem(string upc){
        _selfCheckoutService.RemoveItem(upc);
        
        return Ok();
    }

    [HttpGet("GetAllDepartments")]
    public ActionResult GetAllDepartments(){
       return Ok(_selfCheckoutService.GetAllDepartments());
    }

    [HttpPost("AddDepartment")]
    public ActionResult AddDepartment(string departmentName){
        _selfCheckoutService.RemoveItem(departmentName);
        return Ok();
    }

    // [HttpGet("EditItem")]
    // public async Task EditItem(){
       
    // }

    [HttpPost("UpdatePrice")]
    public ActionResult UpdatePrice(string upc, double price){
        _selfCheckoutService.UpdatePrice(upc, price);
        return Ok();
    }

    [HttpPost("AddToQuantity")]
    public ActionResult AddToQuantity(string upc, int amount){
        _selfCheckoutService.AddToQuantity(upc, amount);
        return Ok();
    }

    [HttpPost("DeleteFromQuantity")]
    public ActionResult DeleteFromQuantity(string upc, int amount){
        _selfCheckoutService.DeleteFromQuantity(upc, amount);
        return Ok();
    }

    [HttpPost("UpdateName")]
    public ActionResult UpdateName(string upc, string newName){
        _selfCheckoutService.UpdateName(upc, newName);
        return Ok();
    }

    [HttpPost("UpdateIsTaxed")]
    public ActionResult UpdateIsTaxed(string upc, Boolean isTaxed){
        _selfCheckoutService.UpdateIsTaxed(upc, isTaxed);
        return Ok();
    }

    [HttpPost("UpdateIsDiscontinued")]
    public ActionResult UpdateIsDiscontinued(string upc, Boolean isDiscontinued){
        _selfCheckoutService.UpdateIsDiscontinued(upc, isDiscontinued);
        return Ok();
    }

    [HttpGet("GetTax")]
    public ActionResult GetTax(){
       return Ok(_selfCheckoutService.GetTax());
    }


    [HttpPost("ProcessPayment")]
    public ActionResult ProcessPayment([FromBody]ReceiptDto receipt){
        if(_selfCheckoutService.validateItems(receipt)){
            return Ok(_selfCheckoutService.ProcessPayment(receipt));
        }
        
        return BadRequest("Not Correct");
    }

    [HttpGet("EditDepartmentName")]
    public ActionResult EditDepartmentName(string oldName, string newName){
        Console.WriteLine("hello new and old", newName, oldName);
        return Ok(_selfCheckoutService.EditDepartmentName(newName,oldName));
    }

    [HttpGet("GetDepartments")]
    public ActionResult GetDepartments(string name){
        return Ok(_selfCheckoutService.GetDepartments(name)[0]);
    }

    [HttpGet("ViewAllItems")]
    public ActionResult ViewAllItems(){
        return Ok(_selfCheckoutService.ViewAllItems());
    }

    [HttpGet("ViewTransactions")]
    public ActionResult ViewTransactions(){
        return Ok(_selfCheckoutService.ViewTransactions());
    }
    
    
}