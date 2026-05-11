
public static class DateTimeFormatter
{
    public static DateTime ConvertUTCTimeToLocalTime(DateTime? date, string offsetvalue)
    {
        DateTime dt = Convert.ToDateTime(date);
        offsetvalue = string.IsNullOrEmpty(offsetvalue) ? "0" : offsetvalue;
        return new DateTime(dt.AddMinutes(Convert.ToDouble(offsetvalue) * -1).Ticks);
    }

    public static DateTime ConvertLocalTimeToUTCTime(DateTime? date, string offsetvalue)
    {
        DateTime dt = Convert.ToDateTime(date);
        offsetvalue = string.IsNullOrEmpty(offsetvalue) ? "0" : offsetvalue;
        return new DateTime(dt.AddMinutes(Convert.ToDouble(offsetvalue) * 1).Ticks);
    }

    public static string FormatDate(DateTime? date)
    {
        string formattedDate = string.Empty;
        if (date != null)
        {
            formattedDate = string.Format("{0:MM/dd/yyyy}", date);
            if (formattedDate == "12/31/2078")
            {
                formattedDate = string.Empty;
            }
        }
        return formattedDate;
    }
}