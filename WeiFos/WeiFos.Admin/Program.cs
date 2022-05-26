using Microsoft.AspNetCore.Authentication.Cookies; 
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WeiFos.Admin;
using WeiFos.NetCore.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessorExt();

#region JsonConvert����

//���л�Ĭ������
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

#region Session���

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    //���������֤��洢session
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

//ʹ�þ�̬�����ģ�ע�������ķ�����
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//��session �滰״̬��ȡ���ڴ���
builder.Services.AddDistributedMemoryCache();
//session�洢�����ݿ���
//services.AddDistributedSqlServerCache(o =>
//{
//    o.ConnectionString = "Server=.;Database=ASPNET5SessionState;Trusted_Connection=True;";
//    o.SchemaName = "dbo";
//    o.TableName = "Sessions";
//});

//����redis����
//services.AddDistributedRedisCache(options => { options.Configuration = "localhost"; options.InstanceName = "SampleInstance"; });

builder.Services.AddSession();
//����Cookie�м������
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options => { });

#endregion

var app = builder.Build();

//ȫ����Դ����
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
