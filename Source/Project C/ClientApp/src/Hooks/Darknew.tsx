import { useState, useEffect } from 'react';

function ThemeToggle() {
  const [isDarkMode, setIsDarkMode] = useState(() => {
    const savedTheme = localStorage.getItem('color-theme');
    const prefersDarkMode = window.matchMedia('(prefers-color-scheme: dark)').matches;

    return savedTheme === 'dark' || (!savedTheme && prefersDarkMode);
  });

  // Function to toggle the theme
  const toggleTheme = () => {
    setIsDarkMode((prevMode) => !prevMode);
  };

  // Update the local storage whenever the theme changes
  useEffect(() => {
    if (isDarkMode) {
      document.documentElement.classList.add('dark');
      localStorage.setItem('color-theme', 'dark');
    } else {
      document.documentElement.classList.remove('dark');
      localStorage.setItem('color-theme', 'light');
    }
  }, [isDarkMode]);

  return (
    <div>
      <button className="bg-white dark:bg-gray-400 dark:text-white text-black" id="theme-toggle" onClick={toggleTheme}>
        {isDarkMode ? (
          <p>Light Mode</p>
          // <img id="theme-toggle-light-icon" src="light-icon.png" alt="Light Mode" />
        ) : (
          <p>Dark Mode</p>

          // <img id="theme-toggle-dark-icon" src="dark-icon.png" alt="Dark Mode" />
        )}
      </button>
    </div>
  );
}

export default ThemeToggle;
