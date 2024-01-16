import { Status } from "@/models/Status";
import { TicketService } from "../../../../services/ticketService";
import { useClerk } from "@clerk/clerk-react";

const statusMapping: { [key: string]: Status } = {
  Closed: Status.Closed,
  Open: Status.Open,
  Registered: Status.Registered,
  Unresolved: Status.Unresolved,
};

export const useUpdateTicketStatus = () => {
  const clerk = useClerk();
  const tokenType = "api_token";

  const updateTicketStatus = async (ticketID: number, status: string) => {
    try {
      const token = await clerk.session?.getToken({ template: tokenType });
      const service = new TicketService();
      if (token) {
        const ticket = await service.getById(token, ticketID);
        if (ticket) {
          ticket.status = statusMapping[status];
          ticket.priority = ticket.status === Status.Closed ? 1 : 1;
          await service.update(token, ticket);
          window.location.reload();
        }
      }
    } catch (error) {
      console.log(error);
    }
  };

  return updateTicketStatus;
};
