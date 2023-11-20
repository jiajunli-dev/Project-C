import { SignedIn, SignedOut } from "@clerk/clerk-react";
import LoginPage from "./LoginPage";
import AdminDashboard from "./AdminTickets";
import Header from "../components/Header";
import Page from "../components/tickets/page"

const HomePage = () => {
  return (
    <div className="min-h-screen">
      <SignedOut>
        <LoginPage />
      </SignedOut>
      <SignedIn>
        <Header></Header>
        <Page />
      </SignedIn>
    </div>
  );
};

export default HomePage;
