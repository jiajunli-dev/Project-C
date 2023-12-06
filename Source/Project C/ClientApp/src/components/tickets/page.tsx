import { useEffect, useState } from "react";
import { columns } from "./columns";
import { DataTable } from "./data-table";
import { useClerk } from "@clerk/clerk-react";
import { TicketService } from "@/services/ticketService";
import { Ticket } from "../../models/Ticket";

export default function DemoPage() {
  const tokenType = "api_token";
  const [data, setData] = useState<Ticket[]>([]);
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
    const newData = [
      { id: 1, createdBy: "test", priority: "test", status: "test" },
      { id: 2, createdBy: "test", priority: "test", status: "test" },
    ];
    setData(newData);
  }, []);

  return (
    <div className="container mx-auto py-10">
      <DataTable columns={columns} data={data} />
    </div>
  );
}
