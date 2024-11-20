public static class AppSetting
{
    public static IConfiguration? Config { get; private set; }

    public static void Init(IConfiguration? config)
    {
        Config = config;
    }
}
