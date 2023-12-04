import logo from "../assets/viscon_logo-removebg-preview.png";
import greenLightIcon from "../assets/green_light_icon.png";
import { useNavigate } from "react-router-dom";
import { SignedIn, SignedOut, UserButton } from "@clerk/clerk-react";
import ShowName from "./ShowName";
import DarkModeToggle from "./DarkModeToggle";

const Header = () => {
  const navigate = useNavigate();

  return (
    <>
        <header className="flex items-center justify-between p-5  border-b-[1px] border-gray-200  dark:border-gray-800 dark:bg-background ">
          <div>
            <img
              src={logo}
              alt="A logo of Viscon Group"
              className="h-[60px] max-w-[180px] cursor-pointer hover:scale-95 ease-linear duration-150 "
              onClick={() => navigate("/")}
            />
          </div>
          <div className="flex items-center">
            <ul className="flex text-black items-center text-lg header__ul gap-10">
              <li onClick={() => navigate("/create-ticket")} className="cursor-pointer hover:text-orange-600 ease-linear duration-150 dark:text-white">
                Create Ticket
              </li>
   
              <li onClick={() => navigate("/view-tickets")} className="cursor-pointer hover:text-orange-600 ease-linear duration-150 dark:text-white">
                View Tickets
              </li>
            </ul>
          </div>
          <div className="flex items-center">
            <SignedOut>
              <button
                className="flex items-center cursor-pointer bg-gray-200 dark:bg-gray-700 mr-[1.75rem]"
                onClick={() => navigate("/authentication")}
              >
                <p className="text-black font-semibold name dark:text-white ">
                  Sign In
                </p>
              </button>
            </SignedOut>

            {/* If signed in */}
            <div className="flex gap-7 items-center">
              <SignedIn>
                <DarkModeToggle/>
                <div className="flex gap-2 items-center ">
                  <ShowName></ShowName>

                  <img
                    className="h-[10px]"
                    src={greenLightIcon}
                    alt="Green Light Icon"
                  />
                </div>
                <div className="mr-6 dark:border-2 dark:border-white rounded-full ">
                  <UserButton afterSignOutUrl="/" ></UserButton>
                </div>
              </SignedIn>
            </div>
          </div>
        </header>
    </>
  );
};

export default Header;
