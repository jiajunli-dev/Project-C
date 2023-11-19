import { Status } from "../../types"
import StepCount from "./StepCount"

interface FormPageThreeProps {
    setCurrForm: React.Dispatch<React.SetStateAction<number>>
    currForm: number
    maxForm: number
    setTicketStatus: React.Dispatch<React.SetStateAction<Status>>
    ticketStatus: Status
}
const FormPageThree = ({ setTicketStatus, ticketStatus, currForm, maxForm}:FormPageThreeProps) => {
  return (
    
    <form className="w-3/5 border-2 p-4">

        <StepCount currForm={currForm} maxForm={maxForm} />

    <h1 className="text-black text-lg font-bold mb-8">
                Is the malfunction critical to your in house operations?
            </h1>

    <div className="relative z-0 w-full mb-6 group">
            <ul className="flex justify-between w-full px-[10px] text-black">
                <li className="flex justify-center relative "><span className="absolute">Not</span></li>
                <li className="flex justify-center relative"><span className="absolute">Critical</span></li>
            </ul>
            <input value={ticketStatus} type="range" className="w-full mt-6" min="1" max="2" step="1" onChange={(e) => setTicketStatus(parseInt(e.target.value))}/>
    </div>
    </form>
  )
}

export default FormPageThree
