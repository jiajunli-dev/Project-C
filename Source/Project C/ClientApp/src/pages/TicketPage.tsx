import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { useClerk } from "@clerk/clerk-react";
import { TicketService } from "../services/ticketService";
import { Ticket } from "../models/Ticket";
import { format } from "date-fns";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
// import 'moment/locale/nl';
import { Priority } from "@/models/Priority";
enum Status {
  Open = 1,
  Closed = 2,
}

const TicketPage = () => {
  const tokenType = "api_token";
  const clerk = useClerk();
  const { id } = useParams();
  const [ticket, setTicket] = useState<Ticket | undefined>(undefined);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    async function fetchDataAsync() {
      try {
        const token = await clerk.session?.getToken({ template: tokenType });
        const service = new TicketService();

        if (token && id) {
          const result = await service.getById(token, parseInt(id));
          if (result) console.log(result);
          setLoading(false);
          if (result) {
            setTicket(result);
          }
        }
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    }

    fetchDataAsync();
  }, [clerk.session]);

  if (!loading && !ticket) {
    return (
      <div className="-mt-32 flex w-full h-screen justify-center items-center">
        Ticket not found
      </div>
    );
  }

  return (
    ticket && (
      <div className=" flex justify-center items-center dark:bg-background h-screen">
        <Card className="w-4/5 -mt-[10%]">
          <CardHeader>
            <div className="flex w-full justify-between">
              <CardTitle className="dark:text-white">Ticket created by {ticket.createdBy} </CardTitle>
              {ticket.status == 2 ? (
                <p className="text-sm text-red-500"> {Status[2]}</p>
              ) : (
                <p className="text-sm text-green-500"> {Status[1]}</p>
              )}
            </div>
            <CardDescription>
              {ticket.createdAt && (
                <p className="text-xs text-gray-500 dark:text-white">
                  Created on{" "}
                  {format(new Date(ticket.createdAt), "MMM d yyyy, h:mm:ss ")}
                </p>
              )}
            </CardDescription>
          </CardHeader>
          <CardContent className="dark:[&>p]:text-gray-100">
            <p className="text-sm font-semibold">Description:</p>
            <p className="text-sm">{ticket.description}</p>
            <br />
            <p className="text-sm font-semibold">Additional notes:</p>
            <p className="text-sm">{ticket.additionalNotes}</p>
            <br />
            <p className="text-sm font-semibold">Tried solutions:</p>
            {ticket.triedSolutions &&
              ticket.triedSolutions.map((solution) => (
                <p className="text-sm" key={solution}>
                  {solution} <br />
                </p>
              ))}
            <br />

            <div className="flex gap-16 ">
              <div className="dark:[&>p]:text-gray-100">
                <p className="text-sm font-semibold">Priority:</p>
                {ticket.priority == 1 ? (
                  <p className="text-sm ">{Priority[1]}</p>
                ) : (
                  <p className="text-sm ">
                    {Priority[2]}
                  </p>
                )}
              </div>
            </div>

            <br />
          </CardContent>
        </Card>
      </div>
    )
  );
};

export default TicketPage;
