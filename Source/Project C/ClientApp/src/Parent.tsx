import { Outlet } from "react-router-dom";
import Header from "@/components/Header";
import { SignedIn } from "@clerk/clerk-react";

const Parent = () => {
  return (
    <section className="min-h-screen ">
      <SignedIn>
        <Header />
      </SignedIn>
      <Outlet />
    </section>
  );
};

export default Parent;
