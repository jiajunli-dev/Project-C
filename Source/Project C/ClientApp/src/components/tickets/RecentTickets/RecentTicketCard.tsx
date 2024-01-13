import { CardHeader, CardContent, Card } from "@/components/ui/card";
import { useNavigate } from "react-router-dom";
const RecentTicketCard = () => {
  // TODO: FETCH TICKET DATA
  // TODO: Replace with actual ticket data
  
  const navigate = useNavigate();
  return (
    <Card onClick={() => navigate(`/ticket/3`)} className="cursor-pointer w-full rounded-lg transform transition duration-500 hover:scale-105">
      <CardHeader className="flex justify-between items-center">
        <h3 className="text-lg font-bold text-gray-900 dark:text-gray-100">
          Ticket #12345
        </h3>
        <p className="text-sm text-gray-600 dark:text-gray-400">Status: Open</p>
      </CardHeader>
      <CardContent className="text-gray-600 dark:text-gray-400">
        Faulty machine reported. Model: XYZ123. Issue: Machine not turning on.
      </CardContent>
    </Card>
  );
};
export default RecentTicketCard;
