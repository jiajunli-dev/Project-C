import RecentTicketCard from "./RecentTicketCard";
export default function RecentTickets() {
  return (
    <div className="flex flex-col items-center justify-center flex-grow    ">
      <div className="w-full max-w-2xl p-4 space-y-6">
        <h2 className="text-3xl font-bold text-center text-gray-900 dark:text-gray-100">
          Your Tickets
        </h2>
        <RecentTicketCard />
      </div>
    </div>
  );
}
