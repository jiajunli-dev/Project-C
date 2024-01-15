import { useEffect, useState } from "react";
import { columns } from "./columns";
import { DataTable } from "./tickets-data-table";
import { SignedIn, SignedOut, useClerk } from "@clerk/clerk-react";
import { TicketService } from "@/services/ticketService";
import { Ticket } from "../../models/Ticket";
import LoginPage from "@/pages/LoginPage";

export default function TicketsPage() {
  const tokenType = "api_token";
  const [data, setData] = useState<Ticket[]>([]);
  const clerk = useClerk();

  const deleteTicket = async (ticket: Ticket) => {
    try {
      const token = await clerk.session?.getToken({ template: tokenType });
      const service = new TicketService();
      if (token) {
        if (ticket.id) await service.delete(token, ticket.id);
        const updatedData = data.filter(
          (ticketMap) => ticketMap.id !== ticket.id
        );
        setData(updatedData);
      }
    } catch (error) {
      console.error("Error deleting ticket:", error);
    }
  };

  useEffect(() => {
    async function fetchDataAsync() {
      try {
        const token = await clerk.session?.getToken({ template: tokenType });
        const service = new TicketService();
        if (token) {
          const data = await service.getAll(token);
          setData(data ?? []);
        }
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    }

    fetchDataAsync();
  }, [clerk.session]);

  return (
    <>
    <SignedIn>
    <div className="container mx-auto py-10">
      <DataTable columns={columns(deleteTicket)} data={data} deleteTicket={deleteTicket} />
    </div>
    </SignedIn>
    <SignedOut>
      <LoginPage/>
    </SignedOut>
    </>

  );
}
