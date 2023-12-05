
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
    <form className="w-3/5 border-2 p-4">

    <StepCount currForm={currForm} maxForm={maxForm} />


    <h1 className="text-black text-lg font-bold mb-8">
        Check all of the information you have entered and make sure it is correct!
    </h1>

    <div className="flex flex-col gap-4 text-md text-black m-4">
        <h3>
        <span className="font-semibold">Description</span> :<br/> {ticketDescription}
        </h3>
        <h3>
        <p className="text-md font-semibold">Tried solutions:</p>
            {ticketTriedSolutions && 
            ticketTriedSolutions.map((solution) => (
            <p className="text-md" key={solution}>{solution},<br /></p> 
            ))}
        </h3>
        <h3>
        <span className="font-semibold">Additional notes</span> :<br/>  {ticketAdditionalNotes}
        </h3>
        <h3>
        <span className="font-semibold">Status :</span> {Status[ticketStatus]}
        </h3>
        <h3>
        <span className="font-semibold">Priority :</span> {Priority[ticketPriority]}
        </h3>
    </div>
    </form>
  );
};

export default FormPageFive;
