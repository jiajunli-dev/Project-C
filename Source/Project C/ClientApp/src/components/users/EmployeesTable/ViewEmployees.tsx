import { useEffect, useState } from "react";
import { columns } from "./columnsEmployees";
import { DataTable } from "../data-table";
import { SignedIn, SignedOut, useClerk } from "@clerk/clerk-react";
import { Employee } from "@/models/Employee";
import LoginPage from "@/pages/LoginPage";
import { employeeService } from "@/services/employeeService";

export default function ViewUsers() {
  const tokenType = "api_token";
  const [data2, setData2] = useState<Employee[]>([]);
  const clerk = useClerk();

  useEffect(() => {
    async function fetchDataAsync() {
      try {
        const token = await clerk.session?.getToken({ template: tokenType });
        const eService = new employeeService();

        if (token) {
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
          <DataTable columns={columns} data={data2} />
        </div>
      </SignedIn>
      <SignedOut>
        <LoginPage />
      </SignedOut>
    </>
  );
}
