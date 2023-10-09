import { SignedIn, SignedOut } from "@clerk/clerk-react";
import LoginPage from "./LoginPage";
import AdminDashboard from "./AdminDashboard";
import Header from "../components/Header";

const HomePage = () => {
  return (
    <>
      <SignedOut>
        <LoginPage />
      </SignedOut>
      <SignedIn>
        <AdminDashboard />
      </SignedIn>
    </>
  );
};

export default HomePage;
