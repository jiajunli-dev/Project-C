import RecentTicketCard from "./RecentTicketCard";
export default function RecentTickets() {
  return (
    <div className="flex flex-col items-center justify-center flex-grow    ">
      <div className="w-full max-w-2xl p-4 space-y-6">

        <RecentTicketCard />
      </div>
    </div>
  );
}
