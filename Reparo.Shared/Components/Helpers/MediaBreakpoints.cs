using Syncfusion.Blazor;

public class Breakpoints
{
    public const string Mobile = nameof(ScreenSize.Mobile);
    public const string Tablet = nameof(ScreenSize.Tablet);
    public const string Laptop = nameof(ScreenSize.Laptop);
    public const string Large = nameof(ScreenSize.Large);

    public List<MediaBreakpoint> Default { get; } =
    [
        new()
        {
            Breakpoint = Mobile,
            MediaQuery = "(max-width: 599px)"
        },
        new()
        {
            Breakpoint = Tablet,
            MediaQuery = "(min-width: 600px) and (max-width: 999px)"
        },
        new()
        {
            Breakpoint = Laptop,
            MediaQuery = "(min-width: 1000px) and (max-width: 1199px)"
        },
        new()
        {
            Breakpoint = Large,
            MediaQuery = "(min-width: 1200px)"
        }
    ];

    public bool IsDesktop(string? breakpoint) =>
        breakpoint is Laptop or Large;
}