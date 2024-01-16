import { useClerk } from "@clerk/clerk-react";
import { useEffect, useState } from "react";
import { TicketService } from "@/services/ticketService";
export default function OpenTickets() {
  const [result, setResult] = useState({});
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
          if (!data.length) return;
          const monthCounts = data.reduce(
            (counts: { [key: number]: number }, ticket) => {
              if (!ticket.createdAt) return counts;
              const month = ticket.createdAt?.getMonth() + 1; 
              if (month) {
                counts[month] = (counts[month] || 0) + 1;
              }
              return counts;
            },
            {}
          );

          setResult(monthCounts);
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
