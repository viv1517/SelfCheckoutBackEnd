using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using SelfCheckoutAPI.EntityModels;

public class ItemsController: ODataController
{
    private readonly SelfCheckoutContext _context;
    public ItemsController(SelfCheckoutContext context)
    {
        _context = context;
    }

    [EnableQuery]
    public ActionResult Get()
    {
        return Ok(_context.Items);
    }

    [EnableQuery]
    public async Task<ActionResult> Get(string key)
    {
        var entity = await _context.Items.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        return Ok(entity);
    }

    public async Task<ActionResult> Patch(string key, Delta<Item> model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entity = await _context.Items.Include(item => item.Department).FirstOrDefaultAsync(item => item.Upc == key);
        if (entity == null)
        {
            return NotFound();
        }
       
        model.Patch(entity);
         _context.Entry(entity).Property(item => item.Upc).IsModified=false;
        await _context.SaveChangesAsync();

        return Updated(entity);
    }


    public async Task<ActionResult> Post([FromBody] Item model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Items.Add(model);
        await _context.SaveChangesAsync();

        return Created(model);
    }

    public async Task<ActionResult> Put(string key, [FromBody] Item update)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (key != update.Upc)
        {
            return BadRequest();
        }

        _context.Entry(update).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return Updated(update);
    }


    public async Task<ActionResult> Delete(string key)
    {
        var entity = await _context.Items.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        _context.Items.Remove(entity);
        await _context.SaveChangesAsync();

        return Ok();
    }

    
}