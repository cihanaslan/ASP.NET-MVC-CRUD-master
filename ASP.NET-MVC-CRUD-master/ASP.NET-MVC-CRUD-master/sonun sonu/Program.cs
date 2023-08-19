using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);


// Servisleri yapýlandýrma
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Uygulama ortamý ile ilgili yapýlandýrmalarý yapma
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // Prodüksiyon ortamý ile ilgili yapýlandýrmalarý yapma
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Routing yapýsýný yapýlandýrma
app.UseRouting();

// Endpoint tanýmlamalarýný yapma
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
