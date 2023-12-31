using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);


// Servisleri yapılandırma
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Uygulama ortamı ile ilgili yapılandırmaları yapma
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // Prodüksiyon ortamı ile ilgili yapılandırmaları yapma
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Routing yapısını yapılandırma
app.UseRouting();

// Endpoint tanımlamalarını yapma
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
