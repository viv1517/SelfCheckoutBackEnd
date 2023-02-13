using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SelfCheckoutAPI.EntityModels;

namespace SelfCheckoutAPI.Services;

public class SelfCheckoutService{
    SelfCheckoutContext _context;

     public SelfCheckoutService(SelfCheckoutContext context){
        this._context = context;
    }

    public List<Item> GetItemsByUPC(string upc){
        var result =  _context.Items.Include(item => item.Department).Where(item => item.Upc == upc).ToList();
        result = result;
        return result;
    }

    public List<Item> GetItemsByDepartment(string departmentName){
        Department department = GetDepartments(departmentName)[0];
        var result =  _context.Items.Include(item => item.Department).Where(item => item.DepartmentId == department.Id).ToList();
        result = result;
        return result;
    }

    public List<Department> GetDepartments(string departmentName){
        // if(!_context.Departments.Any(departments))
        return _context.Departments.Where(department => department.DepartmentName == departmentName).ToList();
    }

    public Item AddItem(ItemDto itemToCreate){
        if(_context.Items.Any(item => item.Upc == itemToCreate.Upc)){
            Item foundItem = GetItemsByUPC(itemToCreate.Upc)[0];
            if (foundItem.Department.DepartmentName!= itemToCreate.DepartmentName){
                Department department = GetDepartments(itemToCreate.DepartmentName)[0];
                foundItem.DepartmentId = department.Id;
                _context.SaveChanges();
            }
            foundItem.Discount = itemToCreate.Discount;
            foundItem.IsDiscontinued = itemToCreate.IsDiscontinued;
            foundItem.ItemName = itemToCreate.ItemName;
            foundItem.Price = itemToCreate.Price;
            _context.SaveChanges();
            return foundItem;
        }
        Item item = new Item(){
            Upc = itemToCreate.Upc,
            ItemName = itemToCreate.ItemName,
            Price = itemToCreate.Price,
            Discount = itemToCreate.Discount,
            Quantity = itemToCreate.Quantity,
            DepartmentId = GetDepartments(itemToCreate.DepartmentName)[0].Id,
            IsTaxed = itemToCreate.IsTaxed,
            IsDiscontinued = itemToCreate.IsDiscontinued,
            Department = GetDepartments(itemToCreate.DepartmentName)[0]

        };
        _context.Items.Add(item);
        _context.SaveChanges();
        return item;
    }

    public void RemoveItem(string upc){
        var item = GetItemsByUPC(upc)[0];
        _context.Remove(item);
        _context.SaveChanges();
    }

    public void SetDiscountByDepartment(string departmentName, double discount){
        var items = GetItemsByDepartment(departmentName);
        foreach(Item item in items){
            item.Discount = discount;
            _context.SaveChanges();
        }
    }

    public void SetDiscountByUPC(string upc, double discount){
        var item = GetItemsByUPC(upc)[0];
        item.Discount = discount;
        _context.SaveChanges();
    }

    public void AddDepartment(string departmentName){
        Department department = new Department() {
            DepartmentName = departmentName
        };

        _context.Departments.Add(department);
        _context.SaveChanges();
    }

    public List<Department> GetAllDepartments(){
        return _context.Departments.ToList();
    }

    public void UpdatePrice(string upc, double price){
        var item = GetItemsByUPC(upc)[0];
        item.Price = price;
        _context.SaveChanges();
    }

    public void AddToQuantity(string upc, int amount){
        var item = GetItemsByUPC(upc)[0];
        item.Quantity = item.Quantity + amount;
        _context.SaveChanges();
    }

    public void DeleteFromQuantity(string upc, int amount){
        var item = GetItemsByUPC(upc)[0];
        item.Quantity = item.Quantity - amount;
        _context.SaveChanges();
    }

    public void UpdateName(string upc, string newName){
        var item = GetItemsByUPC(upc)[0];
        item.ItemName = newName;
        _context.SaveChanges();
    }

    public void UpdateIsTaxed(string upc, Boolean isTaxed){
        var item = GetItemsByUPC(upc)[0];
        item.IsTaxed = isTaxed;
        _context.SaveChanges();
    }

    public void UpdateIsDiscontinued(string upc, Boolean isDiscontinued){
        var item = GetItemsByUPC(upc)[0];
        item.IsDiscontinued = isDiscontinued;
        _context.SaveChanges();
    }

    public Tax GetTax(){
        return  _context.Taxes.ToList()[0];
        
    }

    public Double ProcessPayment(ReceiptDto receipt){
        Double total = calculateTotal(receipt);
        int index = 0;
        Transaction transaction = new Transaction(){
            DatePurchased = DateTime.Today
        };
        _context.Transactions.Add(transaction);
        _context.SaveChanges();
        int id = transaction.Id;
        while (index < receipt.itemsBought.Count()){
            TransactionItem item = new TransactionItem(){
                TransactionId = id,
                ItemId = receipt.itemsBought[index].Upc,
                PriceBought = GetPriceByUPC(receipt.itemsBought[index].Upc),
                // PriceBought = receipt.priceBought[index],
                QuantityBought = receipt.amountBought[index]
            };
            _context.TransactionItems.Add(item);
            _context.SaveChanges();
            index += 1;
        }
        // Receipt newReceipt = new Receipt(total, )
        return total;

    }

    //goes through every item in the receipt and checks if the item exists and is 
    //not discontinued
    public Boolean validateItems(ReceiptDto receipt){
        foreach(ItemDto item in receipt.itemsBought){
            if(!_context.Items.Any(searchItem => searchItem.Upc == item.Upc) 
            && _context.Items.Where(searchItem => searchItem.Upc == item.Upc).ToList()[0].IsDiscontinued == true){
                return false;
            }
        }
        return true;
    }

    public Double calculateTotal(ReceiptDto receipt){
        int index = 0;
        Double total = 0;
        var tax = GetTax().TaxRate;
        while(index < receipt.itemsBought.Count()){
            Double subtotal = receipt.priceBought[index] * receipt.amountBought[index];
            total += subtotal;
            index += 1;
        }

        return total;
    }

    public Double GetPriceByUPC(string upc){
        Item item = GetItemsByUPC(upc)[0];
        Double price = item.Price;
        if (item.Discount != null){
            Double discount = (Double)item.Discount * price;
            price -= discount;
        }
        if (item.IsTaxed){
            Double withTax = Math.Round(price * (GetTax().TaxRate/100), 2);
            price += withTax;
        }
        return price;
    }

    public Department EditDepartmentName(string newName, string oldName){
        Department department = GetDepartments(oldName)[0];
        department.DepartmentName = newName;
        _context.SaveChanges();
        return GetDepartments(newName)[0];
    }

    public List<Item> ViewAllItems(){
        return _context.Items.Include(item => item.Department).ToList();
    }

    public List<TransactionDto> ViewTransactions(){
        List<TransactionDto> transactions = GetTransactions(_context.Transactions.Include(Item => Item.TransactionItems).ToList());
        return transactions;
    }

    public List<TransactionDto> GetTransactions(List<Transaction> transactions){
        List<TransactionDto> result = new List<TransactionDto>();
        
        foreach(Transaction transaction in transactions){
            Console.WriteLine("transaction", transaction);
            List<TransactionItemDto> items = new List<TransactionItemDto>();
            foreach(TransactionItem item in transaction.TransactionItems){
                Item found = GetItemsByUPC(item.ItemId)[0];
                Console.WriteLine("item", item);
                items.Add(new TransactionItemDto(){
                    TransactionId = item.TransactionId,
                    ItemId = item.ItemId,
                    Name = found.ItemName,
                    PriceBought = item.PriceBought,
                    QuantityBought = item.QuantityBought
                });
            }
            // foreach(Item item in transaction.TransactionItems){
            //     item.
            //     items.Add(new TransactionItemDto(){
            //         TransactionId = item,
            //         ItemId = item.Upc,
            //         Name = item.ItemName,
            //         PriceBought = item.Price,
            //         QuantityBought = item
            //     })
            // }
            result.Add(new TransactionDto(transaction.DatePurchased.ToString(), items){
                Id = transaction.Id,
            });
        //    items = new List<TransactionItemDto>();
        }
        return result;
    }

}