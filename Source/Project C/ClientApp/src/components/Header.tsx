import logo from "../assets/viscon_logo-removebg-preview.png";
import searchIcon from "../assets/search_icon.png";
import bellIcon from "../assets/bell_icon.png";
import inboxIcon from "../assets/inbox_icon.png";
import chevDownIcon from "../assets/chevron_down_icon.png";
import greenLightIcon from "../assets/green_light_icon.png";
import { useNavigate } from "react-router-dom";
import "../header.css";
import { SignedIn, SignedOut, UserButton } from "@clerk/clerk-react";
import ShowName from "./ShowName";
import Switcher from "../Switcher";

const Header = () => {
  const navigate = useNavigate();

  return (
    <>
      <div className="min-h-[60px] absolute top-0 right-0 left-0 flex items-center justify-between  p-5 border-b-[1px] border-gray-200 bg-white dark:bg-black">
        <div>
          <img
            src={logo}
            alt="A logo of Viscon Group"
            className="h-[60px] max-w-[180px] cursor-pointer hover:scale-105 ease-linear duration-150 "
            onClick={() => navigate("/")}
          />
        </div>
        <div className="flex items-center">
          <ul className="flex text-black items-center text-lg header__ul gap-10">
            <li className="cursor-pointer hover:text-orange-600 ease-linear duration-150 dark:text-white">
              Create ticket
            </li>
            <li className="cursor-pointer hover:text-orange-600 ease-linear duration-150 dark:text-white">
              FAQ
            </li>
            <li className="cursor-pointer hover:text-orange-600 ease-linear duration-150 dark:text-white">
              About us
            </li>
            <li className="cursor-pointer hover:text-orange-600 ease-linear duration-150 dark:text-white">
              Contact us
            </li>
          </ul>
        </div>

        <div className="flex items-center">
          {/* If NOT signed in */}
          <SignedOut>
            <button
              className="flex items-center cursor-pointer bg-gray-200 dark:bg-gray-700 mr-[1.75rem]"
              onClick={() => navigate("/authentication")}
            >
              <p className="text-black font-semibold name dark:text-white ">Sign In</p>
              {/* <img className="mx-1 mr-3 w-[28%] " src={chevDownIcon} alt="" /> */}
            </button>
          </SignedOut>

          {/* If signed in */}
          <div className="flex gap-7 items-center">
            <SignedIn>
              <div className="flex gap-2 items-center ">
                <ShowName></ShowName>

                <img
                  className="h-[10px]"
                  src={greenLightIcon}
                  alt="Green Light Icon"
                />
              </div>
              <div className="mr-6">
                <UserButton afterSignOutUrl="/"></UserButton>
              </div>
            </SignedIn>
          </div>

          <div className="flex items-center gap-[1.5rem] pr-[5rem]">
            <img
              src={searchIcon}
              className="object-contain cursor-pointer search__icon hover:scale-105 ease-linear duration-150 dark:bg-white rounded-xl"
              alt="Search Icon"
            />
            <img
              src={inboxIcon}
              className="object-contain cursor-pointer hover:scale-105 ease-linear duration-150 dark:bg-white rounded-xl"
              alt="Inbox Icon"
            />
            <Switcher></Switcher>
          </div>
        </div>
      </div>
    </>
  );
};

export default Header;
