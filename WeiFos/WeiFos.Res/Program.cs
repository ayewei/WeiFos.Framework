using WeiFos.Admin;
using WeiFos.NetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("authdomain", builder =>
    {
        //builder.AllowAnyOrigin() //�����κ���Դ����������
        builder.WithOrigins(AppGlobal.Admin) //�O�����S����ā�Դ���ж�����Ԓ������ `,` ���_
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();//ָ������cookie

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
//�����֤����IsAuthenticatedʧЧ
app.UseAuthentication();

app.UseCors("authdomain");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
