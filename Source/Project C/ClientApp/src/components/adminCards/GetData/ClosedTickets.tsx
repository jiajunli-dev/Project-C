import { useClerk } from "@clerk/clerk-react";
import { useEffect, useState } from "react";
import { TicketService } from "@/services/ticketService";
import { Status } from "@/models/Status";
export default function ClosedTickets() {
  const [result, setResult] = useState<number>();
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
          const filteredData = data.filter((ticket) => ticket.status == Status.Closed);
          setResult(filteredData.length);
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
