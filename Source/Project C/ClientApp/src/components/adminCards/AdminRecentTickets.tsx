import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import RecentTicketsAdminData from "./GetData/RecentTicketsAdminData";
import { useNavigate } from "react-router-dom";
export function RecentTickets() {
  const data = RecentTicketsAdminData();
  const navigate = useNavigate();
  if (!data) return null;
  return (
    <div>
      {data.map((ticket) => (
        <div className="mb-5" key={ticket.id}>
          <div className="flex items-center">
            <Avatar className="h-9 w-9">
              <AvatarFallback>{ticket.createdBy?.slice(0, 2)}</AvatarFallback>
            </Avatar>
            <div className="ml-4 space-y-1">
              <p className="text-sm font-medium leading-none dark:text-white">
                {ticket.createdBy}
              </p>
              <p className="text-sm text-muted-foreground">
                {ticket.additionalNotes?.slice(0, 50)}...
              </p>
          </div>
            <div onClick={() =>  navigate(`/ticket/${ticket.id}`)} className="ml-auto font-medium dark:text-white cursor-pointer">
              Go to Ticket
            </div>
          </div>
        </div>
      ))}
    </div>
  );
}
