import { RedirectToSignIn, SignedIn, SignedOut } from "@clerk/clerk-react";
import LoginPage from "./LoginPage";
import AdminDashboard from "./AdminTickets";
import SideBar from "../components/SideBar";

const HomePage = () => {
  return (
    <>
      <SignedOut>
        <LoginPage />
      </SignedOut>
      <SignedIn>
        <div className="flex min-h-screen">
          <SideBar></SideBar>
        </div>
      </SignedIn>
    </>
  );
};

export default HomePage;
