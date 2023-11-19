import StepCount from "./StepCount"

enum Status {
    NonCritical = 1,
    Critical = 2,
}


interface FormPageFiveProps {
    setCurrForm: React.Dispatch<React.SetStateAction<number>>
    currForm: number
    maxForm: number
    ticketDescription: string
    ticketTriedSolutions: string
    ticketAdditionalNotes: string
    ticketStatus: Status
}
const FormPageFive = ({ticketAdditionalNotes,ticketDescription,ticketTriedSolutions, ticketStatus, currForm, maxForm}:FormPageFiveProps) => {
  return (
    
    <form className="w-3/5 border-2 p-4">

    <StepCount currForm={currForm} maxForm={maxForm} />


    <h1 className="text-black text-lg font-bold mb-8">
        Check all of the information you have entered and make sure it is correct!
    </h1>

    <div className="flex flex-col gap-4 text-md text-black m-4">
        <h3>
        Description : {ticketDescription}
        </h3>
        <h3>
        Tried solutions: {ticketTriedSolutions}
        </h3>
        <h3>
        Additional notes: {ticketAdditionalNotes}
        </h3>
        <h3>
        Status: {Status[ticketStatus]}
        </h3>
    </div>
    </form>
  )
}

export default FormPageFive
