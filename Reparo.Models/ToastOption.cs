public class ToastOption
{
    public string Title { get; set; }
    public string Content { get; set; }
    public ToastType ToastType { get; set; }
}

public enum ToastType
{
    Success,
    Error,
    Warning,
    Information
}