import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import FormPageOne from "../components/FormPages/FormPageOne";
import FormPageTwo from "../components/FormPages/FormPageTwo";
import FormPageThree from "../components/FormPages/FormPageThree";
import FormPageFour from "../components/FormPages/FormPageFour";
import FormPageFive from "../components/FormPages/FormPageFive";
import FormPagePictures from "../components/FormPages/FormPagePictures";
enum Status {
  Open,
  Closed,
}
import { SignedIn, useUser } from "@clerk/clerk-react";
import { useClerk } from "@clerk/clerk-react";
import { TicketService } from "@/services/ticketService";
import { Priority } from "@/models/Priority";
import { CreateTicket as ticketCreationType } from "@/models/CreateTicket";

const CreateTicket = () => {
  const navigate = useNavigate();
  const clerk = useClerk();
  const tokenType = "api_token";
  const user = useUser();

  const maxForm = 5;
  const [currForm, setCurrForm] = useState<number>(0);

  const [ticketDescription, setTicketDescription] = useState<string>("");
  const [ticketTriedSolutions, setTicketTriedSolutions] = useState<string[]>(
    []
  );
  const [ticketImages, setTicketImages] = useState<string[]>([]); // TODO: Add images to ticket object ad send to back end

  const [ticketAdditionalNotes, setTicketAdditionalNotes] =
    useState<string>("");
  const [ticketPriority, setTicketPriority] = useState<Priority>(1);
  const [formOneError, setFormOneError] = useState<boolean>(false);
  const [formTwoError, setFormTwoError] = useState<boolean>(false);
  const [formThreeError, setFormThreeError] = useState<boolean>(false);
  const [formFourError, setFormFourError] = useState<boolean>(false);

  const checkInput = () => {
    if (currForm === 0 && ticketDescription === "") {
      setFormOneError(true);
      return false;
    }
    if (currForm === 1 && ticketTriedSolutions.length < 1) {
      setFormTwoError(true);
      return false;
    }
    if (currForm === 2 && ticketAdditionalNotes === "") {
      setFormThreeError(true);
      return false;
    }
    if (currForm === 3 && !ticketPriority) {
      setFormFourError(true);
      return false;
    }
    return true;
  };

  const handleSubmit = async () => {
    try {
      // Get the authentication token
      const token = await clerk.session?.getToken({ template: tokenType });

      // Create an instance of the TicketService
      const service = new TicketService();

      if (token) {
        // Create a new ticket object
        const finalTicket = new ticketCreationType();
        finalTicket.createdBy = user.user?.username ?? "Unknown";
        finalTicket.description = ticketDescription;
        finalTicket.triedSolutions = ticketTriedSolutions;
        finalTicket.additionalNotes = ticketAdditionalNotes;
        finalTicket.priority = ticketPriority;
        finalTicket.status = 1;

        // Validate the ticket object
        const errors = finalTicket.validate();
        console.log(errors);
        if (errors.length > 0) {
          console.log("Validation errors");
          return;
        }

        // Call the create function from the TicketService
        try {
          const data = await service.create(token, finalTicket);
          // If creation is successful, perform additional actions
          if (data && data.id) {
            // Get the ticket by its ID (just an example, adjust as needed)
            const result = await service.getById(token, data.id);
            console.log(result);
            if (!result) return;
            navigate(`/ticket/${result.id}`);
          }
        } catch (createError) {
          console.error("Error creating ticket:", createError);
        }
      }
    } catch (error) {
      console.error("Error fetching data:", error);
    }
  };

  const resetErrors = () => {
    setFormOneError(false);
    setFormTwoError(false);
    setFormThreeError(false);
    setFormFourError(false);
  };

  useEffect(() => {
    resetErrors();
  }, [currForm]);

  return (
      <div className="mt-4 ">
        <div className=" h-full flex flex-col justify-center items-center ">
          {currForm <= maxForm && (
            <div className=" flex w-3/5 justify-between border-x-2 border-t-2  rounded-t-lg p-1 items-center  ">
              <h1 className="text-lg  ml-3 font-semibold dark:text-white">
                Create ticket
              </h1>
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
              formOneError={formOneError}
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
              formTwoError={formTwoError}
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
              formThreeError={formThreeError}
            />
          )}

          {/* Form 4  */}
          {currForm === 3 && (
            <FormPageFour
              ticketPriority={ticketPriority}
              setTicketPriority={setTicketPriority}
              setCurrForm={setCurrForm}
              currForm={currForm}
              maxForm={maxForm}
              formFourError={formFourError}
            />
          )}
          {/* Form 5  */}
          {currForm === 4 && (
            <FormPagePictures
              setCurrentShow={setCurrForm}
              currentShow={currForm}
              finishedImages={ticketImages}
              setFinishedImages={setTicketImages}
              maxForm={maxForm}
              />
        )}

          {/* Form 6  */}
          {currForm === 5 && (
          
            <FormPageFive
              ticketAdditionalNotes={ticketAdditionalNotes}
              ticketDescription={ticketDescription}
              ticketTriedSolutions={ticketTriedSolutions}
              ticketPriority={ticketPriority}
              ticketStatus={Status.Open}
              setCurrForm={setCurrForm}
              currForm={currForm}
              maxForm={maxForm}/>
          
          )}

          <div className="w-3/5 border-t-0">
            {currForm === maxForm ? (
              <div className="flex border-x-2 border-b-2 w-full justify-end">
                <button
                  onClick={() => currForm > 0 && setCurrForm(currForm - 1)}
                  type="button"
                  className="min-w-[10%] flex border justify-center rounded-none text-white bg-gray-200  hover:text-black hover:bg-white  hover:border-black focus:outline-none focus:ring-black font-medium text-sm sm:w-auto p-2 text-center "

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
                  className="min-w-[10%] flex justify-center text-white  border-black border-2 bg-blue-500 hover:opacity-80  focus:ring-black text-xs font-bold sm:w-auto p-2 text-center "
                >
                  Submit
                </button>
              </div>
            ) : (
              <div className="flex border-2 w-full justify-end">
                <button
                  onClick={() => currForm > 0 && setCurrForm(currForm - 1)}
                  type="button"
                  className="min-w-[10%] flex justify-center rounded-none text-white  border-x-2  hover:bg-white hover:border-2 focus:outline-none focus:ring-black font-medium text-sm sm:w-auto p-2 text-center dark:bg-slate-200 "
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
                  className="dark:bg-slate-200 min-w-[10%] flex justify-center rounded-none text-white border-x-2  hover:text-black hover:bg-white hover:border-2 hover:border-black focus:outline-none focus:ring-black font-medium text-sm sm:w-auto p-2 text-center"

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
        </div>
      </div>
  );
};

export default CreateTicket;
