import { useEffect, useState } from "react";
import { columns } from "./columns";
import { DataTable } from "./data-table";
import { SignedIn, SignedOut, useClerk } from "@clerk/clerk-react";
import { Customer } from "@/models/Customer";
import LoginPage from "@/pages/LoginPage";

export default function ViewUsers() {
  const tokenType = "api_token";
  const [data, setData] = useState<Customer[]>([]);
  const clerk = useClerk();

  // useEffect(() => {
  //   async function fetchDataAsync() {
  //     try {
  //       const token = await clerk.session?.getToken({ template: tokenType });
  //       const service = new TicketService();
  //       if (token) {
  //         const data = await service.getAll(token);
  //         setData(data ?? []);
  //       }
  //     } catch (error) {
  //       console.error("Error fetching data:", error);
  //     }
  //   }

  //   fetchDataAsync();
  // }, [clerk.session]);
  useEffect(() => {
    const newData: Customer[] = [
      new Customer({ id: "1", username: "Omar", email: "omar@gmail.com", companyName: "Viscon Group", departmentName: "IT" }),
      new Customer({ id: "2", username: "JiaJun", email: "jiajun@gmail.com", companyName: "Viscon Group", departmentName: "Cleaning" }),
    ];
    setData(newData);
  }, []);

  return (
    <>
    <SignedIn>
    <div className="container mx-auto py-10">
      <DataTable columns={columns} data={data} />
    </div>
    </SignedIn>
    <SignedOut>
      <LoginPage/>
    </SignedOut>
    </>
  );
}
