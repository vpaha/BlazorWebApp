

public static class ZipFormatter
{
    public static string FormatZipCode(string zipCode)
    {
        if (!string.IsNullOrWhiteSpace(zipCode))
        {
            zipCode = zipCode.Replace("-", "");
            zipCode = new string(zipCode?.Where(c => char.IsDigit(c)).ToArray());

            if (zipCode.Length >= 6 && zipCode.Length <= 10)
            {
                var len = zipCode.Length - 5 >= 4 ? 4 : zipCode.Length - 5;
                zipCode = $"{zipCode.Substring(0, 5)}-{zipCode.Substring(5, len)}";
            }
        }

        return zipCode;
    }
}