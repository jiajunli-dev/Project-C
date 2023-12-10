import AdminDashboard from "@/components/adminCards/AdminDashboard";
import { SignedIn, SignedOut } from "@clerk/clerk-react";
import LoginPage from "@/pages/LoginPage";

const HomePage = () => {
  return (
    <>
      <SignedIn>
        <AdminDashboard />
      </SignedIn>
      <SignedOut>
        <LoginPage/>
      </SignedOut>
    </>
  );
};

export default HomePage;
