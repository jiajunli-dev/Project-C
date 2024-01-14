import { CardHeader, CardContent, Card } from "@/components/ui/card";
import { useNavigate } from "react-router-dom";
import FetchCustomerRecentTickets from "./FetchCustomerRecentTickets";
const RecentTicketCard = () => {
  const fetchedData = FetchCustomerRecentTickets();
  const navigate = useNavigate();
  return fetchedData?.map((ticket) => (
    <Card
      onClick={() => navigate(`/ticket/${ticket.id}`)}
      className="cursor-pointer  rounded-lg   duration-500 hover:scale-105"
    >
      <CardHeader className="flex justify-between items-center">
        <h3 className="text-lg font-bold text-gray-900 dark:text-gray-100">
          Ticket #{ticket.id}
        </h3>
        <p className="text-sm text-gray-600 dark:text-gray-400">Status: {ticket.status}</p>
      </CardHeader>
      <CardContent className="text-gray-600 dark:text-gray-400">
        Issue Description: {ticket.description}
      </CardContent>
    </Card>
  ));
};
export default RecentTicketCard;
