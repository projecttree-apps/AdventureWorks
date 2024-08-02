using AdventureWorks.BAL.IService;
using AdventureWorks.BAL.Service;
using AdventureWorks.DAL.Data;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);


var modelBuilder = new ODataConventionModelBuilder();

builder.Services.AddDbContext<dbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Interface Specification
builder.Services.AddScoped<ICustomerService, CustomerService>();
#endregion


builder.Services.AddControllers().AddOData(
options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
