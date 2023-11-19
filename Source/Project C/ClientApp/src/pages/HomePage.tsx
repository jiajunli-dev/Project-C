import { SignedIn, SignedOut } from "@clerk/clerk-react";
import LoginPage from "./LoginPage";
import AdminDashboard from "./AdminTickets";
import Header from "../components/Header";

const HomePage = () => {
  return (
    <div className="min-h-screen">
      <SignedOut>
        <LoginPage />
      </SignedOut>
      <SignedIn>
        <Header></Header>
        <AdminDashboard/>
      </SignedIn>
    </div>
  );
};

export default HomePage;
