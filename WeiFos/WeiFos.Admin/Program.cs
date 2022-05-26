using Microsoft.AspNetCore.Authentication.Cookies; 
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WeiFos.Admin;
using WeiFos.NetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessorExt();

#region JsonConvert设置

//序列化默认配置
JsonConvert.DefaultSettings = () =>
{
    return new JsonSerializerSettings
    {
        ContractResolver = new DefaultContractResolver(),
        DateFormatString = "yyyy-MM-dd HH:mm:ss",
        Formatting = Formatting.None,
        NullValueHandling = NullValueHandling.Ignore,
    };
};

var jsonSetting = JsonConvert.DefaultSettings();
builder.Services.AddMvc().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = jsonSetting.ContractResolver;
    options.SerializerSettings.DateFormatString = jsonSetting.DateFormatString;
    options.SerializerSettings.NullValueHandling = jsonSetting.NullValueHandling;
});


#endregion

#region Session相关

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    //开启身份认证后存储session
    //options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromSeconds(10);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//使用静态上下文，注册上下文访问器
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//将session 绘话状态存取在内存中
builder.Services.AddDistributedMemoryCache();
//session存储在数据库中
//services.AddDistributedSqlServerCache(o =>
//{
//    o.ConnectionString = "Server=.;Database=ASPNET5SessionState;Trusted_Connection=True;";
//    o.SchemaName = "dbo";
//    o.TableName = "Sessions";
//});

//配置redis缓存
//services.AddDistributedRedisCache(options => { options.Configuration = "localhost"; options.InstanceName = "SampleInstance"; });

builder.Services.AddSession();
//增加Cookie中间件配置
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options => { });

#endregion

var app = builder.Build();

//全局资源调度
AppGlobal.Instance.Initial();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticHttpContext();
app.UseCookiePolicy();
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
//app.UseAuthorization();

app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=PassPort}/{action=Login}/{id?}");

app.Run();
