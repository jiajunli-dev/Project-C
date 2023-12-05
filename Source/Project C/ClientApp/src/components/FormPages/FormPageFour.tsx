import { Priority } from "@/models/Priority"
import StepCount from "./StepCount"

interface FormPageThreeProps {
    setCurrForm: React.Dispatch<React.SetStateAction<number>>
    currForm: number
    maxForm: number
    ticketPriority: Priority,
    setTicketPriority: React.Dispatch<React.SetStateAction<Priority>>
    formFourError: boolean
}
const FormPageThree = ({formFourError, ticketPriority, setTicketPriority, currForm, maxForm}:FormPageThreeProps) => {
  return (
    
    <form className="w-3/5 border-2 p-4 pb-[223px] dark:bg-[#121212]">

        <StepCount currForm={currForm} maxForm={maxForm} />

        <h1 className="text-black text-lg font-bold mb-2 dark:text-white">
            Is the malfunction critical to your in-house operations?
        </h1>
        {
            formFourError && (
                <div className="text-red-500 text-sm mb-2">
                    Please select an option
                </div>
            )
        }


        <div className="relative z-0 w-full mb-6 group">
            <select
                value={ticketPriority}
                className="w-1/4 mt-6 bg-white border-2 border-gray-300 focus:outline-none focus:border-indigo-500 text-gray-700 py-2 px-4 pr-8 rounded leading-tight"
                onChange={(e) => setTicketPriority(parseInt(e.target.value))}
            >
                <option value="1">Not Critical</option>
                <option value="2">Critical</option>
            </select>
        </div>
    </form>
  )
}

export default FormPageThree
