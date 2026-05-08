const themeStorageKey = "themevar";
const defaultThemeName = "custom1";
const defaultCulture = "ja-JP";

function getCookie(name)
{
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length !== 2) return null;

    return parts.pop().split(";").shift();
}

function getCultureFromCookie()
{
    const cultureCookie = getCookie(".AspNetCore.Culture");
    if (!cultureCookie) return defaultCulture;

    const match = cultureCookie.match(/c=([^|]+)/);
    return match?.[1] || defaultCulture;
}

function parseThemeCookie(raw)
{
    if (!raw) return null;

    const parts = raw.split("|");
    if (parts.length !== 2) return null;

    return {
        themeName: parts[0] || defaultThemeName,
        isDarkMode: parts[1] === "1",
        culture: getCultureFromCookie()
    };
}

function getDefaultSettings()
{
    return {
        themeName: defaultThemeName,
        isDarkMode: false,
        culture: getCultureFromCookie()
    };
}

function tryGetLocalSettings()
{
    try
    {
        const raw = localStorage.getItem(themeStorageKey);
        if (!raw) return null;

        const parsed = JSON.parse(raw);
        if (!parsed) return null;

        return {
            themeName: parsed.themeName || defaultThemeName,
            isDarkMode: parsed.isDarkMode ?? false,
            culture: parsed.culture || getCultureFromCookie()
        };
    }
    catch
    {
        return null;
    }
}

window.getTheme = function ()
{
    const cookieTheme = parseThemeCookie(getCookie("theme"));
    const localSettings = tryGetLocalSettings();

    if (cookieTheme)
    {
        return {
            themeName: cookieTheme.themeName,
            isDarkMode: cookieTheme.isDarkMode,
            culture: localSettings?.culture || cookieTheme.culture || defaultCulture
        };
    }

    return localSettings || getDefaultSettings();
};

window.setTheme = function (options)
{
    const settings = {
        themeName: options?.themeName || defaultThemeName,
        isDarkMode: options?.isDarkMode ?? false,
        culture: options?.culture || getCultureFromCookie()
    };

    const link = document.getElementById("theme");
    if (link)
    {
        link.href = `css/${settings.themeName}${settings.isDarkMode ? "-dark" : ""}.css`;
    }
    localStorage.setItem(themeStorageKey, JSON.stringify(settings));

    return settings;
};

window.setCultureInTheme = function (culture)
{
    const settings = window.getTheme();
    settings.culture = culture || defaultCulture;

    localStorage.setItem(themeStorageKey, JSON.stringify(settings));

    return settings;
};

window.setCulture = async function (culture)
{
    const targetCulture = culture || defaultCulture;

    const response = await fetch("config/culture", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ Culture: targetCulture })
    });

    if (!response.ok)
    {
        throw new Error(`Failed to persist culture. Status: ${response.status}`);
    }

    window.setCultureInTheme(targetCulture);
};

(() =>
{
    const settings = window.getTheme();
    window.setTheme(settings);
})();
