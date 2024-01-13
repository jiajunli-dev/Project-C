import { Ticket } from "@/models/Ticket";
import { useEffect, useState } from "react";
import { TicketService } from '../../../services/ticketService';

import { useClerk } from "@clerk/clerk-react";
import { CreateTicket } from "@/models/CreateTicket";

const UpdateTicketStatus = (status: string) => {
  const [single, setSingle] = useState<Ticket>();
  const clerk = useClerk();
  const tokenType = 'api_token';

  useEffect(() => {
    async function UpdateTicket() {
        try {
            const token = await clerk.session?.getToken({ template: tokenType });
            const service = new TicketService();
            if (token) {
                const ticket = new CreateTicket();
                const model = await service.getById(token, 5);
                if (model) {
                    model.description = status;
                    const data = await service.update(token, model);
                    setSingle(data);
                    console.log(data);
                }
            }
        }
        catch (error) {
            console.log(error);
        }
    }
    UpdateTicket();
  }, [clerk.session]);
};
export default UpdateTicketStatus;
