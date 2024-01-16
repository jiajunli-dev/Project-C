import { TicketService } from "../../../../services/ticketService";
import { useClerk } from "@clerk/clerk-react";
import { Priority } from "@/models/Priority";

const priorityMapping: { [key: string]: Priority } = {
  None: Priority.None,
  Critical: Priority.Critical,
};

export const useUpdateTicketPriority = () => {
  const clerk = useClerk();
  const tokenType = "api_token";

  const updateTicketPriority = async (ticketID: number, priority: string) => {
    try {
      const token = await clerk.session?.getToken({ template: tokenType });
      const service = new TicketService();
      if (token) {
        const ticket = await service.getById(token, ticketID);
        if (ticket) {
          ticket.priority = priorityMapping[priority];
          await service.update(token, ticket);
          window.location.reload();


        }
      }
    } catch (error) {
      console.log(error);
    }
  };

  return updateTicketPriority;
};
