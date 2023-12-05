
import { Priority } from "@/models/Priority"
import StepCount from "./StepCount"
enum Status {
    Open,
    Closed
}

interface FormPageFiveProps {
    setCurrForm: React.Dispatch<React.SetStateAction<number>>
    currForm: number
    maxForm: number
    ticketDescription: string
    ticketTriedSolutions: string[]
    ticketAdditionalNotes: string
    ticketStatus: Status
    ticketPriority: number
}
const FormPageFive = ({ticketPriority,ticketAdditionalNotes,ticketDescription,ticketTriedSolutions, ticketStatus, currForm, maxForm}:FormPageFiveProps) => {

  return (
    <form className="w-3/5 border-2 p-4 dark:bg-[#121212]">

    <StepCount currForm={currForm} maxForm={maxForm} />


    <h1 className="text-black text-lg font-bold mb-8 dark:text-white">
        Check all of the information you have entered and make sure it is correct!
    </h1>

    <div className="flex flex-col gap-4 text-md text-black m-4">
        <h3 className="dark:text-white">
        <span className="font-semibold dark:text-white">Description</span> :<br/> {ticketDescription}
        </h3>
        <h3>
        <p className="text-md font-semibold dark:text-white">Tried solutions:</p>
            {ticketTriedSolutions && 
            ticketTriedSolutions.map((solution) => (
            <p className="text-md dark:text-white" key={solution}>{solution},<br /></p> 
            ))}
        </h3>
        <h3 className="dark:text-white">
        <span className="font-semibold dark:text-white">Additional notes</span> :<br/>  {ticketAdditionalNotes}
        </h3>
        <h3 className="dark:text-white">
        <span className="font-semibold dark:text-white">Status :</span> {Status[ticketStatus]}
        </h3>
        <h3 className="dark:text-white">
        <span className="font-semibold dark:text-white">Priority :</span> {Priority[ticketPriority]}
        </h3>
    </div>
    </form>
  );
};

export default FormPageFive;
