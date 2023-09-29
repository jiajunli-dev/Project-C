import logo from "../assets/viscon_logo-removebg-preview.png";
import searchIcon from "../assets/search_icon.png";
import bellIcon from "../assets/bell_icon.png";
import inboxIcon from "../assets/inbox_icon.png";
import chevDownIcon from "../assets/chevron_down_icon.png";
import greenLightIcon from "../assets/green_light_icon.png";
import { useNavigate } from "react-router-dom";
import "../header.css";
import {
  SignedIn,
  SignedOut,
  SignInButton,
  UserButton,
} from "@clerk/clerk-react";
import ShowName from "./ShowName";

const Header = () => {
  const navigate = useNavigate();

  return (
    <div className="min-h-[60px] absolute top-0 right-0 left-0 flex items-center justify-between bg-white p-5 border-b-[1px] border-gray-200">
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
          <li className="cursor-pointer hover:text-orange-600 ease-linear duration-150">
            Create ticket
          </li>
          <li className="cursor-pointer hover:text-orange-600 ease-linear duration-150">
            FAQ
          </li>
          <li className="cursor-pointer hover:text-orange-600 ease-linear duration-150">
            About us
          </li>
          <li className="cursor-pointer hover:text-orange-600 ease-linear duration-150">
            Contact us
          </li>
        </ul>
      </div>

      <div className="flex items-center">
        {/* If NOT signed in */}
        <SignedOut>
          <SignInButton>
            <button className="flex items-center cursor-pointer bg-white">
              <p className="text-black font-semibold name">Sign In</p>
              <img className="mx-1 mr-3 w-[28%] " src={chevDownIcon} alt="" />
            </button>
          </SignInButton>
        </SignedOut>

        {/* If signed in */}
        <div className="flex gap-7 items-center">
          <SignedIn>
            <div className="flex gap-2 items-center">
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
          <div className="piped p-0">d</div>
          <img
            src={searchIcon}
            className="object-contain cursor-pointer search__icon hover:scale-105 ease-linear duration-150"
            alt="Search Icon"
          />
          <img
            src={inboxIcon}
            className="object-contain cursor-pointer hover:scale-105 ease-linear duration-150"
            alt="Inbox Icon"
          />
          <img
            src={bellIcon}
            className="object-contain cursor-pointer hover:scale-105 ease-linear duration-150"
            alt="Bell Icon"
          />
        </div>
      </div>
    </div>
  );
};

export default Header;
