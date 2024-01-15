
import { useEffect, useState } from "react"
import { useNavigate, useParams } from "react-router-dom"
import { SignedIn, SignedOut, useClerk, useUser } from "@clerk/clerk-react"
import { TicketService } from '../services/ticketService';
import { Ticket } from '../models/Ticket';
import { format } from 'date-fns';
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
// import 'moment/locale/nl';
import { Priority } from "@/models/Priority";
import LoginPage from "./LoginPage";
import { time } from "console";
import Carousel from "@/components/ticketPage/Carousel";
enum Status {
  Open = 1,
  Closed = 2,
}

const TicketPage = () => {
  const tokenType = 'api_token';
  const clerk = useClerk();
  const { id } = useParams()
  const navigate = useNavigate();
  const [ticket, setTicket] = useState<Ticket | undefined>(undefined);
  const [loading, setLoading] = useState<boolean>(true);
  const [newTicketStatus, setNewTicketStatus] = useState<number>(ticket?.status || 1);
  const [newAdditionalNotes, setNewAdditionalNotes] = useState<string>("")
  const [isUpdating, setIsUpdating] = useState<boolean>(false);
  const user = useUser();

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
            setNewTicketStatus(result?.status || 1);
            setNewAdditionalNotes(result?.additionalNotes || "");
          }
        }

      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchDataAsync();

  }, [clerk.session]);

  const handleUpdate = async () => {
    try {
      // Get the authentication token
      const token = await clerk.session?.getToken({ template: tokenType });

      // Create an instance of the TicketService
      const service = new TicketService();

      if (token) {
        // Create a new ticket object
        const finalTicket = new Ticket();
        finalTicket.createdBy = ticket?.createdBy;
        finalTicket.updatedBy = user?.user?.username ?? "Unknown";
        finalTicket.createdAt = ticket?.createdAt;
        const currentDatetime = new Date();
        finalTicket.updatedAt = currentDatetime;
        finalTicket.description = ticket?.description;
        finalTicket.triedSolutions = ticket?.triedSolutions;
        finalTicket.additionalNotes = newAdditionalNotes;
        finalTicket.priority = ticket?.priority || 1;
        finalTicket.status = newTicketStatus;


        // Call the create function from the TicketService
        try {
          const data = await service.update(token, finalTicket);
          // If creation is successful, perform additional actions
          if (data && data.id) {
            // Get the ticket by its ID (just an example, adjust as needed)
            const result = await service.getById(token, data.id);
            console.log(result);
            if (!result) return;
            window.location.reload();


          }
        } catch (createError) {
          console.error("Error creating ticket:", createError);
        }
      }
    } catch (error) {
      console.error("Error fetching data:", error);
    }
  };

  const deleteTicket = async () => {
    try {
      // Get the authentication token
      const token = await clerk.session?.getToken({ template: tokenType });

      // Create an instance of the TicketService
      const service = new TicketService();

      if (token) {
        // Call the create function from the TicketService
        try {
          const data = await service.delete(token, ticket?.id || 0);
          // If creation is successful, perform additional actions
          console.log(data);
          navigate('/view-tickets');
        } catch (createError) {
          console.error("Error creating ticket:", createError);
        }
      }
    } catch (error) {
      console.error("Error fetching data:", error);
    }
  }
  //temporary imge links for testing dit moet vervangen worden door de images van de ticket
  const tempImages =
    [
      "https://res.cloudinary.com/ddpwhqyyq/image/upload/v1684019583/owljm2bfllhto8cbsbcq.png",
      "https://res.cloudinary.com/ddpwhqyyq/image/upload/v1680638328/efw8qag93xzmln6orpn6.png"
    ]

  if (!loading && !ticket) {
    return (
      <div className="-mt-32 flex w-full h-screen justify-center items-center">
        Ticket not found
      </div>
    );
  }

  return (
    <>
      <SignedIn>

        {ticket && (
          <div className="flex flex-col justify-center items-center bg-gray-50 h-full mt-4">
            <Card className="w-4/5">
              <CardHeader>
                <div className="flex w-full justify-between">
                  <CardTitle>Ticket created by {ticket.createdBy} </CardTitle>
                  {!isUpdating && (<>
                    {newTicketStatus == 2 ? (<p className="text-sm text-red-500"> {Status[2]}</p>)
                      : (<p className="text-sm text-green-500"> {Status[1]}</p>)}
                  </>)}
                  {isUpdating && (
                    <select value={newTicketStatus} onChange={(e) => setNewTicketStatus(parseInt(e.target.value))} className="block py-2.5 px-0 w-fit text-sm text-gray-900 bg-transparent border-0 border-b-2 border-gray-300 appearance-none dark:text-white dark:border-gray-600 dark:focus:border-white focus:outline-none focus:ring-0 focus:border-black peer" name="ticket_status" id="ticket_status">
                      <option value={1}>{Status[1]}</option>
                      <option value={2}>{Status[2]}</option>
                    </select>
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

                {isUpdating ?
                  (<div className="w-full ">
                    <textarea value={newAdditionalNotes} maxLength={2048} rows={4} onChange={(e) => setNewAdditionalNotes(e.target.value)} name="ticket_triedsolutions" id="ticket_triedsolutions" className="block py-2.5 px-0 w-full text-sm text-gray-900 bg-transparent border-0 border-b-2 border-gray-300 appearance-none dark:text-white dark:border-gray-600 dark:focus:border-white focus:outline-none focus:ring-0 focus:border-black peer" placeholder=" " required />
                  </div>)
                  :
                  (<>
                    <p className="text-sm font-semibold">Additional notes:</p>
                    <p className="text-sm">{ticket.additionalNotes}</p>
                  </>)}

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

                <div className="w-full flex justify-center">
                  <div className='max-w-[400px] flex justify-center items-center rounded-xl shadow-md'
                  >
                    <Carousel>
                      {tempImages.map((image, i) => (
                        <img src={image} alt="" key={i} className='min-w-full max-h-[300px] W-[400px] object-contain' />
                      ))}
                    </Carousel>

                  </div>
                </div>


                {user?.user?.publicMetadata.role === "admin" &&
                  (isUpdating ?
                    (
                      <div className="flex w-full justify-between">
                        <button onClick={() => setIsUpdating(false)} className="bg-red-500 hover:bg-red-700 text-white font-bold py-2 px-4 mt-4 rounded">
                          Cancel
                        </button>
                        <button onClick={() => handleUpdate()} className="bg-green-500 hover:bg-green-700 text-white font-bold py-2 px-4 mt-4 rounded">
                          Save
                        </button>
                      </div>
                    ) : (
                      <div className="flex w-full justify-end gap-2">
                        <button onClick={() => setIsUpdating(true)} className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 mt-4 rounded">
                          Edit
                        </button>
                        <button onClick={() => deleteTicket()} className="bg-red-500 hover:bg-red-700 text-white font-bold py-2 px-4 mt-4 rounded">
                          Delete ticket
                        </button>
                      </div>)
                  )}

                <br />
              </CardContent>
            </Card>
          </div>
        )}
      </SignedIn>
      <SignedOut>
        <LoginPage />
      </SignedOut>
    </>
  )
};

export default TicketPage;
