import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "@/components/ui/accordion";
import { SignedIn, SignedOut } from "@clerk/clerk-react";
import { useNavigate } from "react-router-dom";
import LoginPage from "./LoginPage";
export default function FAQPage() {
  const navigate = useNavigate();
  return (
    <>
    <SignedIn>
      <div className="flex flex-col items-center justify-center h-[88vh] dark:bg-background">
        <div className="w-full max-w-3xl p-4 space-y-6">
          <h2 className="text-3xl font-bold text-center text-gray-900 dark:text-gray-100">
            Frequently Asked Questions
          </h2>
          <p className="text-gray-600 dark:text-gray-300 text-center">
            Find answers and solutions to common issues. If you can't find what
            you're looking for, feel free to create a{" "}
            <span
              onClick={() => navigate("/create-ticket")}
              className="cursor-pointer text-blue-800"
            >
              ticket
            </span>
            .
          </p>
          <Accordion type="single" collapsible className="w-full">
            <AccordionItem value="item-1">
              <AccordionTrigger>
                My machine suddenly stopped working as intended, do I immediately
                create a ticket?
              </AccordionTrigger>
              <AccordionContent>
                Before submitting a ticket for a machine that has stopped working,
                please follow these steps: Make sure to turn the machine off and
                on again to see if it resolves the issue. Check if the machine has
                an information screen, and if so, follow the steps displayed for
                the error. If the problem persists after these steps, you can
                proceed to create a support ticket.
              </AccordionContent>
            </AccordionItem>
            <AccordionItem value="item-2">
              <AccordionTrigger>
                Does my machine have to be a machine originating from Viscon
                Group?
              </AccordionTrigger>
              <AccordionContent>
                The Viscon Support Ticket application is designed exclusively for
                Viscon Group's machines. Unfortunately, we cannot provide support
                for any other machines through this platform. If you are
                experiencing issues with a non-Viscon machine, please contact the
                respective manufacturer or service provider for assistance.
              </AccordionContent>
            </AccordionItem>
            <AccordionItem value="item-3">
              <AccordionTrigger>
                How long does it typically take for support to respond to a
                submitted ticket?
              </AccordionTrigger>
              <AccordionContent>
                After you submit a ticket, the support team will review it. During
                the verification process to determine the severity of the issue,
                the ticket will be classified as a 'malfunction' if it meets the
                necessary requirements for being a genuine problem. The resolution
                time for tickets may vary, with more critical issues generally
                being addressed more promptly. However, the sooner you submit a
                ticket, the faster assistance can be provided. Typically, it
                should not take more than a week for your ticket to be resolved.
              </AccordionContent>
            </AccordionItem>
            <AccordionItem value="item-4">
              <AccordionTrigger>
                Can I track the status of my support ticket?
              </AccordionTrigger>
              <AccordionContent>
                Yes, you can track the status of your ticket. Upon submission, the
                ticket is assigned a severity level by the user. It's important to
                note that the support team may adjust the severity level if the
                issue is deemed less severe than initially indicated. Users have
                visibility into whether the ticket has been classified as a
                malfunction. Additionally, you can monitor any additional notes
                that are added to your ticket. This tracking capability allows
                users to stay informed about the progress and status of their
                support ticket throughout the resolution process.
              </AccordionContent>
            </AccordionItem>
            <AccordionItem value="item-5">
              <AccordionTrigger>
                Is there a limit to the number of support tickets I can submit?
              </AccordionTrigger>
              <AccordionContent>
                No, there is no limit to the number of support tickets you can
                submit. However, we encourage users not to repeatedly submit the
                same issue multiple times. If you have additional information or
                updates related to an existing ticket, it's recommended to add
                comments or notes to the original ticket rather than creating
                duplicates. This ensures that our support team can efficiently
                address and track the progress of each unique issue.
              </AccordionContent>
            </AccordionItem>
          </Accordion>
        </div>
      </div>
    </SignedIn>
    <SignedOut>
      <LoginPage/>
    </SignedOut>
    </>

  );
}
