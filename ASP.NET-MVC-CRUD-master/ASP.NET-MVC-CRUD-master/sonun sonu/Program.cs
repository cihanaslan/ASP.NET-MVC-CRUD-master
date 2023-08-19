using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);


// Servisleri yap�land�rma
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Uygulama ortam� ile ilgili yap�land�rmalar� yapma
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // Prod�ksiyon ortam� ile ilgili yap�land�rmalar� yapma
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Routing yap�s�n� yap�land�rma
app.UseRouting();

// Endpoint tan�mlamalar�n� yapma
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
