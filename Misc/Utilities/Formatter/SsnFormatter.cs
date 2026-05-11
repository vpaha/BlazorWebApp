
public static class SsnFormatter
{
    public static string FormatSSN(string ssn)
    {
        if (!string.IsNullOrWhiteSpace(ssn))
        {
            ssn = ssn.Replace("-", "");
            ssn = ssn.Replace("*", "");
            ssn = new string(ssn?.Where(c => char.IsDigit(c)).ToArray());
            if (ssn?.Length > 4 && ssn?.Length < 6)
            {
                ssn = $"{ssn.Substring(0, 3)}-{ssn.Substring(3)}";
            }
            else if (ssn?.Length >= 6 && ssn?.Length <= 11)
            {
                var len = ssn.Length - 5 >= 4 ? 4 : ssn.Length - 5;
                ssn = $"{ssn.Substring(0, 3)}-{ssn.Substring(3, 2)}-{ssn.Substring(5, len)}";
            }
        }
        return ssn;
    }

    public static string MaskedSSN(string ssn)
    {
        if (!string.IsNullOrWhiteSpace(ssn))
        {
            ssn = ssn.Replace("-", "");
            ssn = ssn.Replace("*", "");
            if (ssn?.Length == 4)
            {
                ssn = $"***-**-{ssn}";
            }
            else if (ssn?.Length > 4 && ssn?.Length < 6)
            {
                ssn = $"{ssn.Substring(0, 3)}-{ssn.Substring(3)}";
            }
            else if (ssn?.Length == 9)
            {
                ssn = $"***-**-{ssn.Substring(5, 4)}";
            }
            else if (ssn?.Length >= 6 && ssn?.Length <= 11)
            {
                var len = ssn.Length - 5 >= 4 ? 4 : ssn.Length - 5;
                ssn = $"{ssn.Substring(0, 3)}-{ssn.Substring(3, 2)}-{ssn.Substring(5, len)}";
            }
        }

        return ssn;
    }
}