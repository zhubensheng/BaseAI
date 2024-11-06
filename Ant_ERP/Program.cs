using Admin.Domain.Options;
using Admin.Domain.Utils;
using Admin.Model.SystemModel;
using Admin.Repositorie.ISystem;
using AntDesign.ProLayout;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(config =>
{
    //���趨���JsonResult���ı����������
    config.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);

    config.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    config.JsonSerializerOptions.Converters.Add(new DateTimeNullableConvert());
});
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAntDesign();
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(sp.GetService<NavigationManager>()!.BaseUri)
});
builder.Services.Configure<ProSettings>(builder.Configuration.GetSection("ProSettings"));
builder.Services.AddInteractiveStringLocalizer();
builder.Services.AddLocalization();
{
    builder.Configuration.GetSection("DBConnection").Get<DBConnectionOption>();
    Console.WriteLine(DBConnectionOption.DbType);
}
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Ant_ERP.Api", Version = "v1" });
    //���Api��ע�ͣ�true��ʾ��ʾ������ע�ͣ�
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, true);
    //���Domain��ע�ͣ�true��ʾ��ʾ������ע�ͣ�
    var xmlFile1 = $"{Assembly.GetExecutingAssembly().GetName().Name.Replace("Api", "Domain")}.xml";
    var xmlPath1 = Path.Combine(AppContext.BaseDirectory, xmlFile1);
    c.IncludeXmlComments(xmlPath1, true);
    c.DocInclusionPredicate((docName, apiDes) =>
    {
        if (!apiDes.TryGetMethodInfo(out MethodInfo method))
            return false;
        var version = method.DeclaringType.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
        if (docName == "v1" && !version.Any())
            return true;
        var actionVersion = method.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
        if (actionVersion.Any())
            return actionVersion.Any(v => v == docName);
        return version.Any(v => v == docName);
    });
});
builder.Services.AddScoped<IUserTable_Repositories, UserTable_Repositories>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
InitDB(app);
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AntSK API"); //ע���м��v1Ҫ������SwaggerDoc��������ֱ���һ��
});
app.Run();

void InitDB(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        //codefirst ������
        var _repository = scope.ServiceProvider.GetRequiredService<IUserTable_Repositories>();
        _repository.GetDB().DbMaintenance.CreateDatabase();
        _repository.GetDB().CodeFirst.InitTables(typeof(UserTable));
        _repository.GetDB().CodeFirst.InitTables(typeof(RolesTable));
        //����vector���������ݿ�û������Ҫ�ṩ֧�����������ݿ�
        _repository.GetDB().Ado.ExecuteCommandAsync($"CREATE EXTENSION IF NOT EXISTS vector;");
    }
}