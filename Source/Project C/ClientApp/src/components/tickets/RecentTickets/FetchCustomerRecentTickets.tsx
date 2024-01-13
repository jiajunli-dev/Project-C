import { Ticket } from "@/models/Ticket";
import { useClerk } from "@clerk/clerk-react";
import { useEffect, useState } from "react";
import { TicketService } from "@/services/ticketService";
export default function FetchCustomerRecentTickets() {
  const [result, setResult] = useState<Ticket[]>();
  const clerk = useClerk();
  const tokenType = "api_token";
  const user = useClerk();

  useEffect(() => {
    async function fetchDataAsync() {
      try {
        const token = await clerk.session?.getToken({ template: tokenType });
        const service = new TicketService();
        if (token) {
          const data = await service.getAll(token);
          if (!data) return;
          const userTickets = data.filter(
            (ticket) => ticket.createdBy === user.user?.username
          );
          setResult(userTickets);
        } else {
          console.error("Token not retrieved");
        }
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    }

    fetchDataAsync();
  }, [clerk.session]);
  return result;
}
