enum Priority {
    Low = 0,
    Medium = 1,
    High = 2
}

enum Status {
    Critical = 1,
    Urgent = 2,
    High = 3,
    Medium = 4,
    Low = 5,
    None = 6
}

interface FormPageThreeProps {
    setCurrForm: React.Dispatch<React.SetStateAction<number>>
    currForm: number
    maxForm: number
    ticketDescription: string
    ticketTriedSolutions: string
    ticketAdditionalNotes: string
    ticketPriority: Priority
    ticketStatus: Status
}
const FormPageThree = ({ticketAdditionalNotes,ticketDescription,ticketTriedSolutions,ticketPriority, ticketStatus, setCurrForm, currForm, maxForm}:FormPageThreeProps) => {
  return (
    
    <form className="w-3/5 border-2 p-4">



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
        Priority: {Priority[ticketPriority]}
        </h3>
        <h3>
        Status: {Status[ticketStatus]}
        </h3>
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
                    if(currForm === maxForm ) return
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
