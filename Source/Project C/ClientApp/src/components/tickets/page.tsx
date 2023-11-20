import  { useEffect, useState } from 'react';
import { Payment, columns } from "./columns"
import { DataTable } from "./data-table"

async function getData(): Promise<Payment[]> {
  // Fetch data from your API here.
  return [
    {
      id: "728ed52f",
      amount: 100,
      status: "pending",
      email: "m@example.com",
    },
    {
      id: "829ed63g",
      amount: 200,
      status: "success",
      email: "n@example.com",
    },
    {
      id: "930fd74h",
      amount: 300,
      status: "failed",
      email: "asdasde.com",
    },
    {
      id: "041ge85i",
      amount: 400,
      status: "pending",
      email: "p@asdsadsa.com",
    },
    {
      id: "152hf96j",
      amount: 500,
      status: "success",
      email: "q@example.com",
    },
    {
      id: "263ig07k",
      amount: 600,
      status: "failed",
      email: "r@example.com",
    },
    {
      id: "374jh18l",
      amount: 700,
      status: "pending",
      email: "s@example.com",
    },
    {
      id: "485ki29m",
      amount: 800,
      status: "success",
      email: "t@example.com",
    },
    {
      id: "596lj30n",
      amount: 900,
      status: "failed",
      email: "u@example.com",
    },
    {
      id: "607mk41o",
      amount: 1000,
      status: "pending",
      email: "v@example.com",
    },
  ];
}

export default function DemoPage() {
  const [data, setData] = useState<Payment[]>([]);

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
  )
}