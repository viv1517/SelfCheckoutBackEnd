using SelfCheckoutAPI.Services;
using SelfCheckoutAPI.EntityModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Cart.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<SelfCheckoutService>();
builder.Services.AddScoped<CartService>();
// Add services to the container.

builder.Services.AddControllers().AddOData(opt => {
                opt.AddRouteComponents("odata", GetEdmModel());
                opt.Filter().Select().Expand().OrderBy().SetMaxTop(null).Count();
            });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<SelfCheckoutContext>(opt =>
                opt.UseNpgsql(builder.Configuration.GetConnectionString("postgres")));

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseRouting();

app.MapControllers();

app.Run();

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    
    builder.EntitySet<Department>("Departments").EntityType.HasKey(k => new {
        k.Id
    });
 
    builder.EntitySet<Item>("Items").EntityType.HasKey(k => new {
        k.Upc
    });

    
    
    return builder.GetEdmModel();
}



