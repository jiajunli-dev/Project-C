import { useEffect, useState } from "react";
import { columns } from "./columns";
import { DataTable } from "./data-table";
import { SignedIn, SignedOut, useClerk } from "@clerk/clerk-react";
import { Customer } from "@/models/Customer";
import { Employee } from "@/models/Employee";
import LoginPage from "@/pages/LoginPage";
import { employeeService } from "@/services/employeeService";
import { customerService } from "@/services/customerService";

export default function ViewUsers() {
  const tokenType = "api_token";
  const [data, setData] = useState<Customer[]>([]);
  const [data2, setData2] = useState<Employee[]>([]);
  const clerk = useClerk();

   useEffect(() => {
     async function fetchDataAsync() {
       try {
         const token = await clerk.session?.getToken({ template: tokenType });
         const eService = new employeeService();
         const cService = new customerService();

         if (token) {
           const data = await cService.getAll(token);
           setData(data ?? []);
           const data2 = await eService.getAll(token);
           setData2(data2 ?? []);
         }
       } catch (error) {
         console.error("Error fetching data:", error);
       }
     }

     fetchDataAsync();
   }, [clerk.session]);

  return (
    <>
    <SignedIn>
    <div className="container mx-auto py-10">
      <DataTable columns={columns} data={data} />
    </div>
    <div className="container mx-auto py-10">
      <DataTable columns={columns} data={data2} />
    </div>
    </SignedIn>
    <SignedOut>
      <LoginPage/>
    </SignedOut>
    </>
  );
}
