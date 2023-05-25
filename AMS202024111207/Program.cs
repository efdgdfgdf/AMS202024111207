using Microsoft.AspNetCore.Authentication.Cookies;
using AMS202024111207.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
var constr = $"Data Source=(LocalDB)\\MSSQLLocalDB; AttachDbFilename ={ builder.Environment.ContentRootPath}App_Data\\AmsDB.mdf; Integrated Security = True; Trusted_Connection = True; "; 
builder.Services.AddDbContext<AmsDbContext>
 (options => options.UseSqlServer(constr));
builder.Services.AddDbContext<AmsDbContext>();
//�����֤����,ʹ��cookie��֤ 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
 .AddCookie(options =>
 {
     //ֻ��͸��http(s)������ 
     options.Cookie.HttpOnly = true;
     //δ��¼ʱ�ض�����index 
     options.LoginPath = new PathString("/Home/Login");
     //�ܾ�����ʱ�ض�����index 
     options.AccessDeniedPath = new PathString("/Home/Login");
 });
var app = builder.Build();
app.UseStaticFiles();
//����cookie���� 
app.UseCookiePolicy();
//���������֤ 
app.UseAuthentication();
//������Ȩ���� 
app.UseAuthorization();
//Ĭ�ϵ�¼ҳ
app.MapControllerRoute(name: "default",
 pattern: "{Controller=Home}/{Action=Login}/{Id?}");
app.Run();