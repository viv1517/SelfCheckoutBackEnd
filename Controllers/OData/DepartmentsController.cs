using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using SelfCheckoutAPI.EntityModels;

public class DepartmentsController: ODataController
{
    private readonly SelfCheckoutContext _context;
    public DepartmentsController(SelfCheckoutContext context)
    {
        _context = context;
    }

    [EnableQuery]
    public ActionResult Get()
    {
        return Ok(_context.Departments);
    }

    [EnableQuery]
    public async Task<ActionResult> Get(int key)
    {
        var entity = await _context.Departments.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        return Ok(entity);
    }

    public async Task<ActionResult> Patch(int key, Delta<Department> model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var entity = await _context.Departments.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        model.Patch(entity);

        await _context.SaveChangesAsync();

        return Updated(entity);
    }


    public async Task<ActionResult> Post([FromBody] Department model)
    {
        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }

        _context.Departments.Add(model);
        await _context.SaveChangesAsync();

        return Created(model);
    }

    public async Task<ActionResult> Put(int key, [FromBody] Department update)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (key != update.Id)
        {
            return BadRequest();
        }

        _context.Entry(update).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return Updated(update);
    }


    public async Task<ActionResult> Delete(int key)
    {
        var entity = await _context.Departments.FindAsync(key);
        if (entity == null)
        {
            return NotFound();
        }

        _context.Departments.Remove(entity);
        await _context.SaveChangesAsync();

        return Ok();
    }
}