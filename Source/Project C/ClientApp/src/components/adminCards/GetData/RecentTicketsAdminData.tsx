import { useClerk } from "@clerk/clerk-react";
import { useEffect, useState } from "react";
import { TicketService } from "@/services/ticketService";
import { Ticket } from "@/models/Ticket";
export default function OpenTickets() {
  const [result, setResult] = useState<Ticket[]>();
  const clerk = useClerk();
  const tokenType = "api_token";

  useEffect(() => {
    async function fetchDataAsync() {
      try {
        const token = await clerk.session?.getToken({ template: tokenType });
        const service = new TicketService();
        if (token) {
          const data = await service.getAll(token);
          if (!data) return;
          setResult(data);
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
