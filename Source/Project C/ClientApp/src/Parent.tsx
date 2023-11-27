import { Outlet } from "react-router-dom";
import Header from "@/components/Header";
import { SignedIn } from "@clerk/clerk-react";

const Parent = () => {
  return (
    <>
      <SignedIn>
        <Header />
      </SignedIn>
      <Outlet />
      {/* <Footer /> */}
    </>
  );
};

export default Parent;
