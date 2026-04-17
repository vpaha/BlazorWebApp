public struct CascadingParams
{
    public const string MediaQuery = "MediaQuery";
}

public struct GridConstants
{
    public const int PageCount = 10;
    public const int RowCount = 25;
    public static string[] PageSizes = new string[] { "5", "10", "25", "50", "100" };
}

public struct DateConstants
{
    public static readonly DateTime MaxDate = new DateTime(2078, 12, 31);
    public static readonly DateTime MinDate = new DateTime(1980, 01, 01);
}

public struct ToastConstants
{
    public static readonly int TimeoutSuccess = 5000;
    public static readonly int TimeoutWarning = 0;
    public static readonly int TimeoutError = 0;
}