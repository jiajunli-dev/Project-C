
interface FormPageThreeProps {
    setCurrForm: React.Dispatch<React.SetStateAction<number>>
    currForm: number
    maxForm: number
    setTicketAdditionalNotes: React.Dispatch<React.SetStateAction<string>>
    ticketAdditionalNotes: string
}
const FormPageThree = ({setTicketAdditionalNotes,ticketAdditionalNotes, setCurrForm, currForm, maxForm}:FormPageThreeProps) => {
  return (
    
    <form className="w-3/5 border-2 p-4">

            <h1 className="text-black text-lg font-bold mb-8">
                Add additional notes to clarify the issue even further!
            </h1>

    <div className="relative z-0 w-full mb-6 group">
        <textarea value={ticketAdditionalNotes} maxLength={2048} rows={12} onChange={(e) => setTicketAdditionalNotes(e.target.value)} name="ticket_triedsolutions" id="ticket_triedsolutions" className="block py-2.5 px-0 w-full text-sm text-gray-900 bg-transparent border-0 border-b-2 border-gray-300 appearance-none dark:text-white dark:border-gray-600 dark:focus:border-white focus:outline-none focus:ring-0 focus:border-black peer" placeholder=" " required />
        <label htmlFor="floating_email" className="peer-focus:font-medium absolute text-sm text-gray-500 dark:text-gray-400 duration-300 transform -translate-y-6 scale-75 top-3 -z-10 origin-[0] peer-focus:left-0 peer-focus:text-black peer-focus:dark:text-white peer-placeholder-shown:scale-100 peer-placeholder-shown:translate-y-0 peer-focus:scale-75 peer-focus:-translate-y-6">Additional notes </label>
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
                    if(currForm === maxForm || ticketAdditionalNotes === "") return
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
