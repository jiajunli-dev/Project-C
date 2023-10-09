import { useState, useEffect } from "react";

export default function useDarkSide() {
  // Use the initial theme from localStorage, or default to "light" if it's undefined
  const initialTheme = localStorage.getItem("theme") || "light";
  console.log(initialTheme)

  const [theme, setTheme] = useState(initialTheme);
  const colorTheme = theme === "dark" ? "light" : "dark";

  useEffect(() => {
    const root = window.document.documentElement;
    root.classList.remove(colorTheme);
    root.classList.add(theme);
    localStorage.setItem("theme", theme);
  }, [theme, colorTheme]);

  return [colorTheme, setTheme];
}
