import { Priority, Status } from "../../types"

interface FormPageThreeProps {
    setCurrForm: React.Dispatch<React.SetStateAction<number>>
    currForm: number
    maxForm: number
    setTicketPriority: React.Dispatch<React.SetStateAction<Priority>>
    ticketPriority: Priority
    setTicketStatus: React.Dispatch<React.SetStateAction<Status>>
    ticketStatus: Status
}
const FormPageThree = ({setTicketPriority, ticketPriority, setTicketStatus, ticketStatus, setCurrForm, currForm, maxForm}:FormPageThreeProps) => {
  return (
    
    <form className="w-3/5 border-2 p-4">

            <h1 className="text-black text-lg font-bold mb-8">
                How urgent is this issue?
            </h1>

    <div className="relative z-0 w-full mb-6 group">
            <ul className="flex justify-between w-full px-[10px] text-black">
                <li className="flex justify-center relative"><span className="absolute">Low</span></li>
                <li className="flex justify-center relative"><span className="absolute">Medium</span></li>
                <li className="flex justify-center relative"><span className="absolute">High</span></li>
            </ul>
            <input value={ticketPriority} type="range" className="w-full mt-6" min="0" max="2" step="1" onChange={(e) => setTicketPriority(parseInt(e.target.value))}/>
    </div>

    <h1 className="text-black text-lg font-bold mb-8">
                How critical is this issue to your operations?
            </h1>

    <div className="relative z-0 w-full mb-6 group">
            <ul className="flex justify-between w-full px-[10px] text-black">
                <li className="flex justify-center relative"><span className="absolute">Critical</span></li>
                <li className="flex justify-center relative"><span className="absolute">Urgent</span></li>
                <li className="flex justify-center relative"><span className="absolute">High</span></li>
                <li className="flex justify-center relative"><span className="absolute">Medium</span></li>
                <li className="flex justify-center relative"><span className="absolute">Low</span></li>
                <li className="flex justify-center relative"><span className="absolute">None</span></li>
            </ul>
            <input value={ticketStatus} type="range" className="w-full mt-6" min="1" max="6" step="1" onChange={(e) => setTicketStatus(parseInt(e.target.value))}/>
    </div>

    <div className="flex justify-evenly gap-2 ">

        <button 
             type="button" onClick={() => currForm > 0 && setCurrForm(currForm -1)}
             className="flex justify-center min-w-[50%] rounded-none text-white bg-black hover:text-black hover:bg-white hover:border-2 hover:border-black focus:outline-none focus:ring-black font-medium text-sm sm:w-auto px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-8 h-6">
                    <path strokeLinecap="round" strokeLinejoin="round" d="M6.75 15.75L3 12m0 0l3.75-3.75M3 12h18" />
                </svg>
        </button>

            <button 
                onClick={() => {
                    if(currForm === maxForm || ticketPriority == null || ticketStatus == null) return
                    else{ setCurrForm(currForm + 1)}}}
                    type="button" className=" flex justify-center min-w-[50%] rounded-none text-white bg-black hover:text-black hover:bg-white hover:border-2 hover:border-black focus:outline-none focus:ring-black font-medium text-sm sm:w-auto px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="w-8 h-6">
                        <path strokeLinecap="round" strokeLinejoin="round" d="M17.25 8.25L21 12m0 0l-3.75 3.75M21 12H3" />
                    </svg>
            </button>
        </div>

    </form>
  )
}

export default FormPageThree
