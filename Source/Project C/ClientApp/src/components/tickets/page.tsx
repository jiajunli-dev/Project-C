import { useEffect, useState } from "react";
import { columns } from "./columns";
import { DataTableTicket } from "@/types";
import { DataTable } from "./data-table";

async function getData(): Promise<DataTableTicket[]> {
  // Fetch data from API here.
  return [
    {
      id: 1,
      requestedBy: "User 1",
      machine: "Machine 1",
      assignee: "Assignee 1",
      priority: "non-critical",
      status: "open",
      createdAt: "2022-01-01",
    },
    {
      id: 2,
      requestedBy: "User 2",
      machine: "Machine 2",
      assignee: "Assignee 2",
      priority: "critical",
      status: "closed",
      createdAt: "2022-01-02",
    },
    {
      id: 3,
      requestedBy: "User 3",
      machine: "Machine 3",
      assignee: "Assignee 3",
      priority: "non-critical",
      status: "open",
      createdAt: "2022-01-03",
    },
    {
      id: 4,
      requestedBy: "User 4",
      machine: "Machine 4",
      assignee: "Assignee 4",
      priority: "critical",
      status: "closed",
      createdAt: "2022-01-04",
    },
    {
      id: 5,
      requestedBy: "User 5",
      machine: "Machine 5",
      assignee: "Assignee 5",
      priority: "non-critical",
      status: "open",
      createdAt: "2022-01-05",
    },
    {
      id: 6,
      requestedBy: "User 6",
      machine: "Machine 6",
      assignee: "Assignee 6",
      priority: "critical",
      status: "closed",
      createdAt: "2022-01-06",
    },
    {
      id: 7,
      requestedBy: "User 7",
      machine: "Machine 7",
      assignee: "Assignee 7",
      priority: "non-critical",
      status: "open",
      createdAt: "2022-01-07",
    },
    {
      id: 8,
      requestedBy: "User 8",
      machine: "Machine 8",
      assignee: "Assignee 8",
      priority: "critical",
      status: "closed",
      createdAt: "2022-01-08",
    },
    {
      id: 9,
      requestedBy: "User 9",
      machine: "Machine 9",
      assignee: "Assignee 9",
      priority: "non-critical",
      status: "open",
      createdAt: "2022-01-09",
    },
    {
      id: 10,
      requestedBy: "User 10",
      machine: "Machine 10",
      assignee: "Assignee 10",
      priority: "critical",
      status: "closed",
      createdAt: "2022-01-10",
    },
    {
      id: 11,
      requestedBy: "User 11",
      machine: "Machine 11",
      assignee: "Assignee 11",
      priority: "non-critical",
      status: "open",
      createdAt: "2022-01-11",
    },
    {
      id: 12,
      requestedBy: "User 12",
      machine: "Machine 12",
      assignee: "Assignee 12",
      priority: "critical",
      status: "closed",
      createdAt: "2022-01-12",
    },
  ];
}

export default function DemoPage() {
  const [data, setData] = useState<DataTableTicket[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      const result = await getData();
      setData(result);
    };

    fetchData();
  }, []);

  return (
    <div className="container mx-auto py-10">
      <DataTable columns={columns} data={data} />
    </div>
  );
}
