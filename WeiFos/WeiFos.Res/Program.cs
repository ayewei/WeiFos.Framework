using WeiFos.Admin;
using WeiFos.NetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("authdomain", builder =>
    {
        //builder.AllowAnyOrigin() //允许任何来源的主机访问
        builder.WithOrigins(AppGlobal.Admin) //設定允許跨域的來源，有多個的話可以用 `,` 隔開
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();//指定处理cookie

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseStaticHttpContext();
//app.UseAuthorization();
//身份认证否则IsAuthenticated失效
app.UseAuthentication();

app.UseCors("authdomain");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
