import { SignedIn, SignedOut } from "@clerk/clerk-react";
import RecentTicketCard from "./RecentTicketCard";
import LoginPage from "@/pages/LoginPage";
export default function RecentTickets() {
  return (
    <>
    <SignedIn>
    <div className="flex flex-col items-center justify-center flex-grow  dark:bg-background  ">
      <div className="w-full max-w-2xl p-4 space-y-6">

        <RecentTicketCard />
      </div>
    </div>
    </SignedIn>
    <SignedOut>
      <LoginPage/>
    </SignedOut>
    </>
  );
}
