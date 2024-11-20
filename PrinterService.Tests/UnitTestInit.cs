using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

public static class UnitTestInit
{
    public static WebApplicationBuilder builder;

    public static WebApplication webapp;

    public static void Init()
    {
        builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            EnvironmentName = "test"
        });
        builder.Configuration.AddJsonFile("appsettings.json").AddJsonFile("appsettings.test.json");
        AppSetting.Init(builder.Configuration);
        StringExtension.InitPrintCommand(AppSetting.Config?.GetSection("PrintCommand").Value);
        webapp = builder.Build();
    }
}
