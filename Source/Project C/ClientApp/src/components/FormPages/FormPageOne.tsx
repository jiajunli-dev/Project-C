import StepCount from "./StepCount"

interface FormPageOneProps {
    setCurrForm: React.Dispatch<React.SetStateAction<number>>
    currForm: number
    setTicketDescription: React.Dispatch<React.SetStateAction<string>>
    ticketDescription: string
    maxForm: number
}
const FormPageOne = ({ currForm, setTicketDescription, ticketDescription, maxForm}:FormPageOneProps) => {

  return (
            
            <form className="w-3/5 border-2 p-4">
            
            <StepCount currForm={currForm} maxForm={maxForm} />
            
            <h1 className="text-black text-lg font-bold mb-8">
                Explain the problem your facing in detail!
            </h1>

            <div className="relative z-0 w-full mb-6 group">
                <textarea value={ticketDescription} maxLength={2048} rows={12} onChange={(e) => setTicketDescription(e.target.value)} name="ticket_description" id="ticket_description" className="block py-2.5 px-0 w-full text-sm text-gray-900 bg-transparent border-0 border-b-2 border-gray-300 appearance-none dark:text-white dark:border-gray-600 dark:focus:border-white focus:outline-none focus:ring-0 focus:border-black peer" placeholder="" required />
                <label htmlFor="ticket_description" className="peer-focus:font-medium absolute text-sm text-gray-500 dark:text-gray-400 duration-300 transform -translate-y-6 scale-75 top-3 -z-10 origin-[0] peer-focus:left-0 peer-focus:text-black peer-focus:dark:text-white peer-placeholder-shown:scale-100 peer-placeholder-shown:translate-y-0 peer-focus:scale-75 peer-focus:-translate-y-6">Description</label>
            </div>
            </form>
  )
}

export default FormPageOne
