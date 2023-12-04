import { useState } from "react";
import { useNavigate } from "react-router-dom";
import FormPageOne from "../components/FormPages/FormPageOne";
import FormPageTwo from "../components/FormPages/FormPageTwo";
import FormPageThree from "../components/FormPages/FormPageThree";
import FormPageFour from "../components/FormPages/FormPageFour";
import FormPageFive from "../components/FormPages/FormPageFive";
import { Ticket, Status } from "../types";
import { SignedIn } from "@clerk/clerk-react";
import { TicketService } from "@/services/ticketService";

const CreateTicket = () => {
  const navigate = useNavigate();
  const maxForm = 4;
  const [currForm, setCurrForm] = useState<number>(0);
  const [ticket, setTicket] = useState<Ticket>({
    description: "",
    triedSolutions: "",
    additionalNotes: "",
    status: 1,
  });

  const [ticketDescription, setTicketDescription] = useState<string>("");
  const [ticketTriedSolutions, setTicketTriedSolutions] = useState<string>("");
  const [ticketAdditionalNotes, setTicketAdditionalNotes] =
    useState<string>("");
  const [ticketStatus, setTicketStatus] = useState<Status>(1);

  const checkInput = () => {
    if (currForm === 0 && ticketDescription === "") return false;
    if (currForm === 1 && ticketTriedSolutions === "") return false;
    if (currForm === 2 && ticketAdditionalNotes === "") return false;
    if (currForm === 3 && !ticketStatus) return false;
    return true;
  };

  const handleSubmit = async () => {
    // SEND TICKET TO API
    // const ticketService = new TicketService();
    
    // ticketService.create("abc123", )

    setCurrForm(5);
  };

  return (
      <SignedIn>
        <div className=" mt-4 ">
          <div className="w-full h-full flex flex-col justify-center items-center">
            {currForm <= maxForm && (
              <div className="flex w-3/5 justify-between border-x-2 border-t-2  rounded-t-lg p-1 items-center  ">
                <h1 className="text-lg  font-semibold text-blue-500">
                  Create ticket
                </h1>
                {currForm === maxForm ? (
                  <div className="flex gap-2">
                    <button
                      onClick={() => currForm > 0 && setCurrForm(currForm - 1)}
                      type="button"
                      className="flex justify-center rounded-none text-white bg-white border-2 hover:text-black hover:bg-white hover:border-2 hover:border-black focus:outline-none focus:ring-black font-medium text-sm sm:w-auto p-2 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
                    >
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        strokeWidth={1.5}
                        stroke="currentColor"
                        className="w-4 h-4"
                      >
                        <path
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          d="M6.75 15.75L3 12m0 0l3.75-3.75M3 12h18"
                        />
                      </svg>
                    </button>

                    <button
                      onClick={() => {
                        if (!checkInput()) return;
                        handleSubmit();
                      }}
                      type="button"
                      className=" flex justify-center text-black  border-gray-300 rounded border-2 bg-white hover:text-black hover:bg-white hover:border-2 hover:border-black focus:outline-none focus:ring-black font-medium text-sm sm:w-auto p-2 text-center"
                    >
                      Submit
                    </button>
                  </div>
                ) : (
                  <div className="flex gap-2">
                    <button
                      onClick={() => currForm > 0 && setCurrForm(currForm - 1)}
                      type="button"
                      className="flex justify-center rounded-none text-white bg-white border-2 hover:text-black hover:bg-white hover:border-2 hover:opacity-80 focus:outline-none focus:ring-black font-medium text-sm sm:w-auto p-2 text-center "
                    >
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        strokeWidth={1.5}
                        stroke="currentColor"
                        className="w-4 h-4"
                      >
                        <path
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          d="M6.75 15.75L3 12m0 0l3.75-3.75M3 12h18"
                        />
                      </svg>
                    </button>

                    <button
                      onClick={() => {
                        if (currForm === maxForm || !checkInput()) return;
                        else {
                          setCurrForm(currForm + 1);
                        }
                      }}
                      type="button"
                      className=" flex justify-center  rounded-none text-white border-2 bg-white hover:text-black hover:bg-white hover:border-2 hover:opacity-80 focus:outline-none focus:ring-black font-medium text-sm sm:w-auto p-2 text-center"
                    >
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        strokeWidth={1.5}
                        stroke="currentColor"
                        className="w-4 h-4"
                      >
                        <path
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          d="M17.25 8.25L21 12m0 0l-3.75 3.75M21 12H3"
                        />
                      </svg>
                    </button>
                  </div>
                )}
              </div>
            )}

            {/* Form 1  */}
            {currForm === 0 && (
              <FormPageOne
                ticketDescription={ticketDescription}
                setTicketDescription={setTicketDescription}
                maxForm={maxForm}
                setCurrForm={setCurrForm}
                currForm={currForm}
              />
            )}

            {/* Form 2  */}
            {currForm === 1 && (
              <FormPageTwo
                ticketTriedSolutions={ticketTriedSolutions}
                setTicketTriedSolutions={setTicketTriedSolutions}
                setCurrForm={setCurrForm}
                currForm={currForm}
                maxForm={maxForm}
              />
            )}

            {/* Form 3  */}
            {currForm === 2 && (
              <FormPageThree
                ticketAdditionalNotes={ticketAdditionalNotes}
                setTicketAdditionalNotes={setTicketAdditionalNotes}
                setCurrForm={setCurrForm}
                currForm={currForm}
                maxForm={maxForm}
              />
            )}

            {/* Form 4  */}
            {currForm === 3 && (
              <FormPageFour
                setTicketStatus={setTicketStatus}
                ticketStatus={ticketStatus}
                setCurrForm={setCurrForm}
                currForm={currForm}
                maxForm={maxForm}
              />
            )}

            {/* Form 5  */}
            {currForm === 4 && (
              <FormPageFive
                ticketAdditionalNotes={ticketAdditionalNotes}
                ticketDescription={ticketDescription}
                ticketTriedSolutions={ticketTriedSolutions}
                ticketStatus={ticketStatus}
                setCurrForm={setCurrForm}
                currForm={currForm}
                maxForm={maxForm}
              />
            )}

            {/* Final Screen   */}
            {currForm === 5 && (
              <div className="flex flex-col justify-center items-center mt-8">
                <div className="flex gap-4 items-center">
                  <h1 className=" text-lg">Your form has been submitted</h1>
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                    strokeWidth={1.5}
                    stroke="currentColor"
                    className="w-6 h-6"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      d="M9 12.75L11.25 15 15 9.75M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                    />
                  </svg>
                </div>

                <button
                  onClick={() => {
                    navigate("/");
                  }}
                  type="button"
                  className=" flex justify-center text-black w-full border-gray-300 rounded border-2 bg-white hover:text-black hover:bg-white hover:border-2 hover:border-black focus:outline-none focus:ring-black font-medium text-sm sm:w-auto p-2 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
                >
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                    strokeWidth={1.5}
                    stroke="currentColor"
                    className="w-6 h-6"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      d="M2.25 12l8.954-8.955c.44-.439 1.152-.439 1.591 0L21.75 12M4.5 9.75v10.125c0 .621.504 1.125 1.125 1.125H9.75v-4.875c0-.621.504-1.125 1.125-1.125h2.25c.621 0 1.125.504 1.125 1.125V21h4.125c.621 0 1.125-.504 1.125-1.125V9.75M8.25 21h8.25"
                    />
                  </svg>
                </button>
              </div>
            )}
          </div>
        </div>
      </SignedIn>
  );
};

export default CreateTicket;
