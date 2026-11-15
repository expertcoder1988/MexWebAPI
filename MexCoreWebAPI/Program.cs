using MalikExpressMVC.Models.XML.PNR;
using MexCoreWebAPI.Model;
using MexCoreWebAPI.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using DinkToPdf;
using DinkToPdf.Contracts;

string MyAllowSpecificOrigins = "ReactPolicy";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("MexDb");
var visaconnectionString = builder.Configuration.GetConnectionString("VisaDb");
var oldMexconnectionString = builder.Configuration.GetConnectionString("MexOldDb");
var MexBlogConnectionString = builder.Configuration.GetConnectionString("MexBlogDb");
var HotelDbConnectionString = builder.Configuration.GetConnectionString("HotelDb");

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
    builder =>
    {
        builder.WithOrigins("https://malikexpress.com", "http://localhost:3000")
                       .AllowAnyMethod()
                      .AllowAnyHeader()
                       .SetIsOriginAllowed((host) => true)
                      .AllowCredentials();
    });
});

builder.Services.AddDbContext<MexDbContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddDbContext<VisaDbContext>(x => x.UseSqlServer(visaconnectionString));
builder.Services.AddDbContext<MexOldDBContext>(x => x.UseSqlServer(oldMexconnectionString));
builder.Services.AddDbContext<MexBlogDbContext>(x => x.UseSqlServer(MexBlogConnectionString));
builder.Services.AddDbContext<HotelDBContext>(x => x.UseSqlServer(HotelDbConnectionString));

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient<PayProService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

// Add DinkToPdf services
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                 ForwardedHeaders.XForwardedProto
});

app.UseSession();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();
