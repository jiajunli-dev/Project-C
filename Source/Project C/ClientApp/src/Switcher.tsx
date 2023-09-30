import { useState } from "react";
import { DarkModeSwitch } from "react-toggle-dark-mode";
import useDarkSide from "./Hooks/useDarkSide";

export default function Switcher() {
	const [colorTheme, setColorTheme] = useDarkSide();
	const [darkSide, setDarkSide] = useState(colorTheme === "white");

	const toggleDarkMode = (checked: any) => {
		//@ts-ignore
		setColorTheme(colorTheme)
		setDarkSide(checked);
	};

	return (
		<>
			<DarkModeSwitch
				checked={darkSide}
				onChange={toggleDarkMode}
				size={30}
			/>
		</>
	);
}