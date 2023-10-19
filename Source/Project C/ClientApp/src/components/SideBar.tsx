import "../css/sidebar.css";
import Darknew from "../Hooks/Darkmode";
import { SignOutButton } from "@clerk/clerk-react";
import { SignedIn, UserButton } from "@clerk/clerk-react";
import ShowName from "./ShowName";

const SideBar = () => {
  return (
    <div>
      <div className=" z-40 w-64 h-full px-3 py-4 overflow-y-auto bg-[#1b1b46]">
        <div>
          <a href="#" className="flex items-center pl-2.5 mb-5">
            <img
              src="/viscon_logo.jpeg"
              className="h-6 mr-3 sm:h-7"
              alt="Flowbite Logo"
            />
            <span className="self-center text-xl font-semibold whitespace-nowrap">
              Viscon Group
            </span>
          </a>
          <div className="flex gap-5 items-center pl-2.5 mb-5">
            <SignedIn>
              <div className="mr-1">
                <UserButton afterSignOutUrl="/"></UserButton>
              </div>
              <div className="flex  items-start flex-col ">
                <div>
                  <p className="">Welcome,</p>
                </div>
                <ShowName></ShowName>
              </div>
            </SignedIn>
          </div>
        </div>

        <ul className="space-y-2 font-medium">
          <li>
            <a
              href="#"
              className="flex items-center p-2  rounded-lg text-white hover:bg-gray-100 "
            >
              <svg
                className="w-5 h-5  transition duration-75  group-hover:text-gray-900 dark:group-hover:text-white"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                fill="currentColor"
                viewBox="0 0 22 21"
              >
                <path d="M16.975 11H10V4.025a1 1 0 0 0-1.066-.998 8.5 8.5 0 1 0 9.039 9.039.999.999 0 0 0-1-1.066h.002Z" />
                <path d="M12.5 0c-.157 0-.311.01-.565.027A1 1 0 0 0 11 1.02V10h8.975a1 1 0 0 0 1-.935c.013-.188.028-.374.028-.565A8.51 8.51 0 0 0 12.5 0Z" />
              </svg>
              <span className="ml-3">Admin Dashboard</span>
            </a>
          </li>

          <li>
            <a
              href="#"
              className="flex items-center p-2 rounded-lg text-white hover:bg-gray-100 dark:hover:bg-gray-700 group"
            >
              <svg
                className="flex-shrink-0 w-5 h-5  transition duration-75  group-hover:text-gray-900 dark:group-hover:text-white"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                fill="currentColor"
                viewBox="0 0 20 20"
              >
                <path d="m17.418 3.623-.018-.008a6.713 6.713 0 0 0-2.4-.569V2h1a1 1 0 1 0 0-2h-2a1 1 0 0 0-1 1v2H9.89A6.977 6.977 0 0 1 12 8v5h-2V8A5 5 0 1 0 0 8v6a1 1 0 0 0 1 1h8v4a1 1 0 0 0 1 1h2a1 1 0 0 0 1-1v-4h6a1 1 0 0 0 1-1V8a5 5 0 0 0-2.582-4.377ZM6 12H4a1 1 0 0 1 0-2h2a1 1 0 0 1 0 2Z" />
              </svg>
              <span className="flex-1 ml-3 whitespace-nowrap">Inbox</span>
              <span className="inline-flex items-center justify-center w-3 h-3 p-3 ml-3 text-sm font-medium text-blue-800 bg-blue-100 rounded-full ">
                3
              </span>
            </a>
          </li>
          <li>
            <a
              href="#"
              className="flex items-center p-2  rounded-lg text-white hover:bg-gray-100 dark:hover:bg-gray-700 group"
            >
              <svg
                className="flex-shrink-0 w-5 h-5  transition duration-75  "
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                fill="currentColor"
                viewBox="0 0 20 18"
              >
                <path d="M14 2a3.963 3.963 0 0 0-1.4.267 6.439 6.439 0 0 1-1.331 6.638A4 4 0 1 0 14 2Zm1 9h-1.264A6.957 6.957 0 0 1 15 15v2a2.97 2.97 0 0 1-.184 1H19a1 1 0 0 0 1-1v-1a5.006 5.006 0 0 0-5-5ZM6.5 9a4.5 4.5 0 1 0 0-9 4.5 4.5 0 0 0 0 9ZM8 10H5a5.006 5.006 0 0 0-5 5v2a1 1 0 0 0 1 1h11a1 1 0 0 0 1-1v-2a5.006 5.006 0 0 0-5-5Z" />
              </svg>
              <span className="flex-1 ml-3 whitespace-nowrap">Users</span>
            </a>
          </li>
          <li>
            <a
              href="#"
              className="flex items-center p-2  rounded-lg text-white hover:bg-gray-100 dark:hover:bg-gray-700 group"
            >
              <svg
                className="flex-shrink-0 w-5 h-5 transition duration-75   "
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                fill="currentColor"
                viewBox="0 0 18 20"
              >
                <path d="M17 5.923A1 1 0 0 0 16 5h-3V4a4 4 0 1 0-8 0v1H2a1 1 0 0 0-1 .923L.086 17.846A2 2 0 0 0 2.08 20h13.84a2 2 0 0 0 1.994-2.153L17 5.923ZM7 9a1 1 0 0 1-2 0V7h2v2Zm0-5a2 2 0 1 1 4 0v1H7V4Zm6 5a1 1 0 1 1-2 0V7h2v2Z" />
              </svg>
              <span className="flex-1 ml-3 whitespace-nowrap">Tickets</span>
            </a>
          </li>
        </ul>
        <ul className="pt-4 mt-4 space-y-2 font-medium border-t border-gray-200 ">
          <li>
            <Darknew />
          </li>
          <li>
            <SignOutButton>
              <button
                type="button"
                className="mt-1.5 ml-[0.35rem] text-white bg-gradient-to-r from-red-400 via-red-500 to-red-600 hover:bg-gradient-to-br focus:ring-4 focus:outline-none focus:ring-red-300 dark:focus:ring-red-800 shadow-lg shadow-red-500/50 dark:shadow-lg dark:shadow-red-800/80 font-medium rounded-lg text-sm px-3 py-2.5 text-center mr-2 mb-2"
              >
                Sign Out
              </button>
            </SignOutButton>
          </li>
        </ul>
      </div>
    </div>
  );
};

export default SideBar;