import NewCard from "@/components/adminCards/AdminDashboard";
import { SignedIn, SignedOut } from "@clerk/clerk-react";
import LoginPage from "@/pages/LoginPage";

const HomePage = () => {
  return (
    <>
      <SignedIn>
        <NewCard />
      </SignedIn>
      <SignedOut>
        <LoginPage/>
      </SignedOut>
    </>
  );
};

export default HomePage;
