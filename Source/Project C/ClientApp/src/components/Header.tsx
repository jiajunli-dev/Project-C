import logo from "../assets/viscon_logo-removebg-preview.png";
import searchIcon from "../assets/search_icon.png";
import bellIcon from "../assets/bell_icon.png";
import inboxIcon from "../assets/inbox_icon.png";
import chevDownIcom from "../assets/chevron_down_icon.png";
import { useNavigate } from "react-router-dom";
import "../header.css";

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
        <div className="flex items-center cursor-pointer">
          <p className="text-black font-semibold name">Omar</p>
          <img  className="mx-1 mr-3 w-[28%] " src={chevDownIcom} alt="" />
        </div>

        <svg
          xmlns="http://www.w3.org/2000/svg"
          fill="#009fe3"
          viewBox="0 0 24 24"
          strokeWidth={1.5}
          stroke="currentColor"
          className="w-14 h-14 cursor-pointer mr-[25px] hover:scale-105 ease-linear duration-150"
          onClick={() => navigate("/authentication")}
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            d="M17.982 18.725A7.488 7.488 0 0012 15.75a7.488 7.488 0 00-5.982 2.975m11.963 0a9 9 0 10-11.963 0m11.963 0A8.966 8.966 0 0112 21a8.966 8.966 0 01-5.982-2.275M15 9.75a3 3 0 11-6 0 3 3 0 016 0z"
          />
        </svg>
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
