import AdminDashboard from "@/components/adminCards/AdminDashboard";
import { SignedIn, SignedOut } from "@clerk/clerk-react";
import LoginPage from "@/pages/LoginPage";
import { useUser } from "@clerk/clerk-react";
import Employee from "./Employee";
import CustomerPage from "./CustomerPage";



const HomePage = () => {
  const { user } = useUser();

  return (
    <>
      <SignedIn>
        {user?.publicMetadata.role === "admin" ? (
          <AdminDashboard />
        ) : user?.publicMetadata.role === "employee" ? (
          <Employee />
        ) : user?.publicMetadata.role === "customer" ? (
          <CustomerPage />
        ) : (
          <h1 className="text-black">Error</h1>
        )}
      </SignedIn>
      <SignedOut>
        <LoginPage />
      </SignedOut>
    </>
  );
};

export default HomePage;
