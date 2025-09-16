﻿namespace WebAdminPanel.Services.UI
{
    public class ThemeService
    {
        public bool IsDarkMode { get; private set; } = true;

        public event Action? OnThemeChanged;

        public void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            OnThemeChanged?.Invoke();
        }
    }
}
