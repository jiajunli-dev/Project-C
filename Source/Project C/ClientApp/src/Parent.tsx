import { Outlet } from "react-router-dom";
import Header from "@/components/Header";
import { SignedIn } from "@clerk/clerk-react";
import { Toaster } from "@/components/ui/toaster";

const Parent = () => {
  return (
    <section className="flex flex-col min-h-screen bg-gray-50 dark:bg-background">
      <SignedIn>
        <Header />
      </SignedIn>
      <Outlet />
      <Toaster />
    </section>
  );
};

export default Parent;
