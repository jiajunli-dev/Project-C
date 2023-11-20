import { Select, Table, TextInput } from "flowbite-react";
import { useState } from "react";
import chevronUp from "../assets/chevron-up.png";
import chevronDown from "../assets/chevron-down.png";
import "../css/admintable.css";
import { Pagination } from "@mui/material";

import { Button } from "@/components/ui/button"


interface TableData {
  id: number;
  requestedBy: string;
  subject: string;
  assignee: string;
  priority: string;
  createDate: string;
  status: string;
}

export default function DefaultTable() {
  const initialData: TableData[] = [
    {
      id: 3,
      requestedBy: "Alice",
      subject: "Server Outage",
      assignee: "Bob",
      priority: "Urgent",
      createDate: "21-10-2023",
      status: "Open",
    },
    {
      id: 4,
      requestedBy: "Eva",
      subject: "Network Connectivity",
      assignee: "Dan",
      priority: "Low",
      createDate: "22-10-2023",
      status: "Closed",
    },
    {
      id: 5,
      requestedBy: "Grace",
      subject: "Software Update",
      assignee: "Henry",
      priority: "Urgent",
      createDate: "23-10-2023",
      status: "Open",
    },
    {
      id: 6,
      requestedBy: "Ivy",
      subject: "Hardware Replacement",
      assignee: "Jack",
      priority: "Low",
      createDate: "24-10-2023",
      status: "Closed",
    },
    {
      id: 7,
      requestedBy: "Leo",
      subject: "Database Issue",
      assignee: "Mike",
      priority: "Urgent",
      createDate: "30-10-2023",
      status: "Open",
    },
    {
      id: 8,
      requestedBy: "Nina",
      subject: "Email Problem",
      assignee: "Test",
      priority: "Urgent",
      createDate: "26-10-2023",
      status: "Closed",
    },
    {
      id: 9,
      requestedBy: "Olivia",
      subject: "Security Update",
      assignee: "Sophia",
      priority: "Low",
      createDate: "28-10-2023",
      status: "Open",
    },
    {
      id: 10,
      requestedBy: "Liam",
      subject: "Software Bug",
      assignee: "Ethan",
      priority: "Urgent",
      createDate: "29-10-2023",
      status: "Open",
    },
    {
      id: 11,
      requestedBy: "Mia",
      subject: "Hardware Failure",
      assignee: "Ava",
      priority: "Low",
      createDate: "30-10-2023",
      status: "Open",
    },
    {
      id: 12,
      requestedBy: "Noah",
      subject: "Database Update",
      assignee: "Lucas",
      priority: "Low",
      createDate: "31-10-2023",
      status: "Open",
    },
    {
      id: 13,
      requestedBy: "Sophia",
      subject: "Network Issue",
      assignee: "Elijah",
      priority: "Urgent",
      createDate: "1-11-2023",
      status: "Closed",
    },
    {
      id: 9,
      requestedBy: "Olivia",
      subject: "Security Update",
      assignee: "Sophia",
      priority: "Low",
      createDate: "28-10-2023",
      status: "Open",
    },
    {
      id: 10,
      requestedBy: "Liam",
      subject: "Software Bug",
      assignee: "Ethan",
      priority: "Urgent",
      createDate: "29-10-2023",
      status: "Open",
    },
    {
      id: 11,
      requestedBy: "Mia",
      subject: "Hardware Failure",
      assignee: "Ava",
      priority: "Low",
      createDate: "30-10-2023",
      status: "Open",
    },
    {
      id: 12,
      requestedBy: "Noah",
      subject: "Database Update",
      assignee: "Lucas",
      priority: "Low",
      createDate: "31-10-2023",
      status: "Open",
    },
    {
      id: 13,
      requestedBy: "Sophia",
      subject: "Network Issue",
      assignee: "Elijah",
      priority: "Urgent",
      createDate: "1-11-2023",
      status: "Closed",
    },
  ];

  const [data, setData] = useState<TableData[]>(initialData);
  const [searchQuery, setSearchQuery] = useState("");
  const [sortOrder, setSortOrder] = useState<{ [key: string]: "asc" | "desc" }>(
    {
      id: "asc",
      requestedBy: "asc",
      subject: "asc",
      assignee: "asc",
      priority: "asc",
      createDate: "asc",
      status: "asc",
    }
  );

  const [entriesPerPage, setEntriesPerPage] = useState<number>(10);
  const [currentPage, setCurrentPage] = useState<number>(1);

  
  const toggleSortOrder = (field: keyof TableData) => {
    setSortOrder((prevSortOrder) => {
      const newSortOrder = { ...prevSortOrder };
      newSortOrder[field] = prevSortOrder[field] === "asc" ? "desc" : "asc";
  
      const sortedData = [...data];
      sortedData.sort((a, b) => {
        if (newSortOrder[field] === "asc") {
          return a[field] < b[field] ? -1 : 1;
        } else {
          return a[field] > b[field] ? -1 : 1;
        }
      });
  

  
      setData(sortedData);
  
      return newSortOrder;
    });
  };

  const chevronClass = (field: keyof TableData) =>
    sortOrder[field] === "asc" ? chevronDown : chevronUp;

  const updateFilteredData = () => {
    const updatedFilteredData = initialData.filter((item) => {
      const query = searchQuery.toLowerCase();
      return (
        item.requestedBy.toLowerCase().includes(query) ||
        item.subject.toLowerCase().includes(query) ||
        item.assignee.toLowerCase().includes(query) ||
        item.priority.toLowerCase().includes(query) ||
        item.createDate.includes(query) ||
        item.status.toLowerCase().includes(query)
      );
    });

    const updatedTotalPages = Math.ceil(
      updatedFilteredData.length / entriesPerPage
    );

    if (currentPage > updatedTotalPages) {
      setCurrentPage(updatedTotalPages);
    }

    return updatedFilteredData;
  };

  const filteredData = updateFilteredData();

  const totalPages = Math.ceil(filteredData.length / entriesPerPage);

  const displayedData = filteredData.slice(
    (currentPage - 1) * entriesPerPage,
    currentPage * entriesPerPage
  );


  return (
    <>
      <div className="flex items-center justify-between relative  mb-6">
        <TextInput
          id="small"
          sizing="l"
          className="text-black"
          type="text"
          placeholder="Filter tickets..."
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
        />
      </div>

      <Table className="border border-slate-200 bg-[#F9FAFD] ">
        <Table.Head>
          <Table.HeadCell
            className="cursor-pointer select-none relative"
            onClick={() => toggleSortOrder("id")}
          >
            ID
            <img
              className="w-5 absolute top-2 left-[2.5rem]"
              src={chevronClass("id")}
              alt=""
            />
          </Table.HeadCell>
          <Table.HeadCell
            className="cursor-pointer select-none relative"
            onClick={() => toggleSortOrder("requestedBy")}
          >
            Requested By
            <img
              className="w-5 absolute top-2 left-[7.9rem]"
              src={chevronClass("requestedBy")}
              alt=""
            />
          </Table.HeadCell>
          <Table.HeadCell
            className="cursor-pointer select-none relative"
            onClick={() => toggleSortOrder("subject")}
          >
            Subject
            <img
              className="w-5 absolute top-2 left-[5.5rem]"
              src={chevronClass("subject")}
              alt=""
            />
          </Table.HeadCell>
          <Table.HeadCell
            className="cursor-pointer select-none relative"
            onClick={() => toggleSortOrder("assignee")}
          >
            Assignee
            <img
              className="w-5 absolute top-2 left-[5.5rem]"
              src={chevronClass("assignee")}
              alt=""
            />
          </Table.HeadCell>
          <Table.HeadCell
            className="cursor-pointer select-none relative"
            onClick={() => toggleSortOrder("priority")}
          >
            Priority
            <img
              className="w-5 absolute top-2 left-[5.5rem]"
              src={chevronClass("priority")}
              alt=""
            />
          </Table.HeadCell>
          <Table.HeadCell
            className="cursor-pointer select-none relative"
            onClick={() => toggleSortOrder("createDate")}
          >
            Created At
            <img
              className="w-5 absolute top-2 left-[6.5rem]"
              src={chevronClass("createDate")}
              alt=""
            />
          </Table.HeadCell>
          <Table.HeadCell
            className="cursor-pointer select-none relative"
            onClick={() => toggleSortOrder("status")}
          >
            Status
            <img
              className="w-5 absolute top-2 left-[5.5rem]"
              src={chevronClass("status")}
              alt=""
            />
          </Table.HeadCell>
          <Table.HeadCell>Action</Table.HeadCell>
        </Table.Head>
        <Table.Body className="divide-y">
          {displayedData.map((item, index) => (
            <Table.Row
              key={index}
              className="bg-white dark:border-gray-700 dark:bg-gray-800"
            >
              <Table.Cell className="font-bold text-black">
                {item.id}
              </Table.Cell>
              <Table.Cell className="font-bold text-black">
                {item.requestedBy}
              </Table.Cell>
              <Table.Cell className="font-bold text-black">
                {item.subject}
              </Table.Cell>
              <Table.Cell className="font-bold text-black">
                {item.assignee}
              </Table.Cell>
              <Table.Cell className={item.priority.toLowerCase()}>
                {item.priority}
              </Table.Cell>
              <Table.Cell className="font-bold text-black">
                {item.createDate}
              </Table.Cell>
              <Table.Cell className={item.status.toLowerCase()}>
                {item.status}
              </Table.Cell>
              <Table.Cell>
                <a
                  className="font-Urgent text-cyan-600 hover:underline dark:text-cyan-500"
                  href="/tables"
                >
                  <p>Edit</p>
                </a>
              </Table.Cell>
            </Table.Row>
          ))}
        </Table.Body>
      </Table>
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-3 mt-2">
          <p className="text-black">Rows per page</p>
          <Select
            value={entriesPerPage.toString()}
            onChange={(e) => setEntriesPerPage(Number(e.target.value))}
            className=""
          >
            <option value="10">10</option>
            <option value="25">25</option>
            <option value="50">50</option>
            <option value="100">100</option>
          </Select>
        </div>
        <Pagination
          count={totalPages}
          onChange={(event, page) => {
            setCurrentPage(page);
          }}
          shape="rounded"
        />
      </div>
    </>
  );
}
