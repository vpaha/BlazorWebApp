public class ToastService
{
    public event Action<ToastOption> ShowToastTrigger;

    public void ShowSuccess(string content, string title = "")
    {
        ShowToastTrigger?.Invoke(new ToastOption()
        {
            Title = title,
            Content = content,
            ToastType = ToastType.Success
        });
    }

    public void ShowWarning(string content, string title = "")
    {
        ShowToastTrigger?.Invoke(new ToastOption()
        {
            Title = title,
            Content = content,
            ToastType = ToastType.Warning
        });
    }

    public void ShowError(string content, string title = "")
    {
        ShowToastTrigger?.Invoke(new ToastOption()
        {
            Title = title,
            Content = content,
            ToastType = ToastType.Error
        });
    }

}