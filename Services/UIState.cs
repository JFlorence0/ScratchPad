using System;

// Namespace declaration, this is how the file wiil be found in the project
namespace ScratchPad.Services
{
    // Class declaration, this is the class that will be used to manage the UI state
    public class UIState
    {
        // Sidebar initial state
        public bool IsSidebarCollapsed { get; private set; } = false;

        /* Event for notifying subscribers about sidebar state changes,
        this is how other parts of the application will be notified */
        public event Action? SidebarStateChanged;

        // Method for toggling the sidebar state, we will call this method when the sidebar is toggled
        public void ToggleSidebar()
        {
            // Change the sidebar state opposite of what it was
            IsSidebarCollapsed = !IsSidebarCollapsed;
            // Signal the other parts of app that the sidebar state has changed
            SidebarStateChanged?.Invoke();
        }

        // This will track the last nav link selection of the user
        public string NavLinkSelection { get; set; } = string.Empty;

        /* Event for notifying subscribers about nav link selection state changes */
        public event Action? NavLinkSelectionChanged;

        public void SetNavLinkSelection(string selection)
        {
            NavLinkSelection = selection;
            NavLinkSelectionChanged?.Invoke();
        }

        // Initial state of dark mode, default is false (light mode)
        public bool IsDarkModeEnabled { get; private set; } = false;

        // Event for notifying subscribers about dark mode changes, just like the sidebar event
        public event Action? DarkModeStateChanged;

        // Same as the ToggleSidebar event, but for dark mode
        public void ToggleDarkMode()
        {
            IsDarkModeEnabled = !IsDarkModeEnabled;
            DarkModeStateChanged?.Invoke();
        }

    }
}
