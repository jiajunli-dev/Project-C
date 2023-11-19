import { useState, useEffect } from "react";
import FormPageOne from "../components/FormPages/FormPageOne";
import FormPageTwo from "../components/FormPages/FormPageTwo";
import FormPageThree from "../components/FormPages/FormPageThree";
import FormPageFour from "../components/FormPages/FormPageFour";
import FormPageFive from "../components/FormPages/FormPageFive";
import {Ticket, Status, Priority} from "../types"

const CreateTicket = () => {

    const maxForm = 4;
    const [currForm, setCurrForm] = useState<number>(0);
    const [ticket, setTicket] = useState<Ticket>({
        description: "",
        triedSolutions: "",
        additionalNotes: "",
        priority: 0,
        status: 1,
    });

    const [ticketDescription, setTicketDescription] = useState<string>("")
    const [ticketTriedSolutions, setTicketTriedSolutions] = useState<string>("")
    const [ticketAdditionalNotes, setTicketAdditionalNotes] = useState<string>("")
    const [ticketPriority, setTicketPriority] = useState<Priority>(0)
    const [ticketStatus, setTicketStatus] = useState<Status>(1)    

  return (
    <div className="w-full h-screen flex justify-center items-center">
        
        {/* Form 1  */}
        {currForm === 0 && (
            <FormPageOne ticketDescription={ticketDescription} setTicketDescription={setTicketDescription}  setCurrForm={setCurrForm} currForm={currForm} />
        )}

        {/* Form 2  */}
        {currForm === 1 && (
            <FormPageTwo ticketTriedSolutions={ticketTriedSolutions} setTicketTriedSolutions={setTicketTriedSolutions} setCurrForm={setCurrForm}  currForm={currForm} maxForm={maxForm} />
        )}

        {/* Form 3  */}
        {currForm === 2 && (
            <FormPageThree ticketAdditionalNotes={ticketAdditionalNotes} setTicketAdditionalNotes={setTicketAdditionalNotes} setCurrForm={setCurrForm} currForm={currForm} maxForm={maxForm} />
        )}

        {/* Form 4  */}
        {currForm === 3 && (
            <FormPageFour setTicketPriority={setTicketPriority} setTicketStatus={setTicketStatus} ticketPriority={ticketPriority} ticketStatus={ticketStatus} setCurrForm={setCurrForm} currForm={currForm} maxForm={maxForm} />
        )}

        {/* Form 5  */}
        {currForm === 4 && (
            <FormPageFive ticketAdditionalNotes={ticketAdditionalNotes} ticketDescription={ticketDescription} ticketTriedSolutions={ticketTriedSolutions} ticketPriority={ticketPriority} ticketStatus={ticketStatus} setCurrForm={setCurrForm} currForm={currForm} maxForm={maxForm} />
        )}


        {/* submitbutton 
            <button type="submit" className="min-w-full rounded-none text-white bg-black hover:text-black hover:bg-white hover:border-2 hover:border-black focus:outline-none focus:ring-black font-medium text-sm sm:w-auto px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">Submit</button>
        */}
    </div>
  )
}

export default CreateTicket
