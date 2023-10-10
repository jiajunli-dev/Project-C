import { useState, useEffect } from "react";

function ThemeToggle() {
  const [isDarkMode, setIsDarkMode] = useState(() => {
    const savedTheme = localStorage.getItem("color-theme");
    const prefersDarkMode = window.matchMedia(
      "(prefers-color-scheme: dark)"
    ).matches;

    return savedTheme === "dark" || (!savedTheme && prefersDarkMode);
  });

  // Function to toggle the theme
  const toggleTheme = () => {
    setIsDarkMode((prevMode) => !prevMode);
  };

  // Update the local storage whenever the theme changes
  useEffect(() => {
    if (isDarkMode) {
      document.documentElement.classList.add("dark");
      localStorage.setItem("color-theme", "dark");
    } else {
      document.documentElement.classList.remove("dark");
      localStorage.setItem("color-theme", "light");
    }
  }, [isDarkMode]);

  return (
    <div>
      <label className="relative inline-flex items-center cursor-pointer pl-1.5">
        <input
          type="checkbox"
          value=""
          className="sr-only peer"
          checked={isDarkMode}
          onClick={toggleTheme}
        />
        <div className="w-[2.55rem] h-6 bg-gray-200   rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after: pl-2.4 left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600"></div>
        <span className="ml-3 text-sm font-medium text-gray-900 dark:text-white">
          Dark Mode
        </span>
      </label>
    </div>
  );
}

export default ThemeToggle;
