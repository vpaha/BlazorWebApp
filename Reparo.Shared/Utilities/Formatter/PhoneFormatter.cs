public static class PhoneFormatter
{
    public static string FormatPhone(string phone)
    {
        if (!string.IsNullOrWhiteSpace(phone))
        {
            phone = phone.Replace("-", "");
            phone = new string(phone?.Where(c => char.IsDigit(c)).ToArray());
            if (phone?.Length > 3 && phone?.Length <= 6)
            {
                var len = phone.Length - 3 >= 3 ? 3 : phone.Length - 3;
                phone = $"{phone.Substring(0, 3)}-{phone.Substring(3, len)}";
            }
            else if (phone?.Length > 3 && phone?.Length > 6)
            {
                var len = phone.Length - 6 >= 4 ? 4 : phone.Length - 6;
                phone = $"{phone.Substring(0, 3)}-{phone.Substring(3, 3)}-{phone.Substring(6, len)}";
            }
        }
        return phone;
    }
}