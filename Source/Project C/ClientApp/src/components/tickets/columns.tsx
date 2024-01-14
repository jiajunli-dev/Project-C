"use client";
import ColumnHeader from "@/components/tickets/components/ColumnHeader";
import { ColumnDef } from "@tanstack/react-table";
import { Checkbox } from "../ui/checkbox";
import { Ticket } from "@/models/Ticket";
import { DataTableRowActions } from "./components/data-table-row-actions";

export const columns = (
  deleteTicket: (ticket: Ticket) => Promise<void>
): ColumnDef<Ticket>[] => [
  {
    id: "select",
    header: ({ table }) => (
      <Checkbox
        checked={
          table.getIsAllPageRowsSelected() ||
          (table.getIsSomePageRowsSelected() && "indeterminate")
        }
        onCheckedChange={(value) => table.toggleAllPageRowsSelected(!!value)}
        aria-label="Select all"
        className="translate-y-[2px] "
      />
    ),
    cell: ({ row }) => (
      <Checkbox
        checked={row.getIsSelected()}
        onCheckedChange={(value) => row.toggleSelected(!!value)}
        aria-label="Select row"
        className="translate-y-[2px] "
      />
    ),
    enableSorting: false,
    enableHiding: false,
  },
  {
    accessorKey: "id",
    header: ({ column }) => (
      <ColumnHeader
        column={column}
        className="dark:[&>button>span]:text-white"
        title="ID"
      />
    ),
    cell: ({ row }) => (
      <div className="w-[80px] dark:text-white">{row.getValue("id")}</div>
    ),
  },
  {
    accessorKey: "createdBy",
    header: ({ column }) => {
      return (
        <ColumnHeader
          column={column}
          title="Created By"
          className="dark:[&>button>span]:text-white"
        />
      );
    },
  },
  {
    accessorKey: "priority",
    header: ({ column }) => {
      return (
        <ColumnHeader
          column={column}
          title="Priority"
          className="dark:[&>button>span]:text-white"
        />
      );
    },
  },
  {
    accessorKey: "status",
    header: ({ column }) => {
      return (
        <ColumnHeader
          column={column}
          title="Status"
          className="dark:[&>button>span]:text-white"
        />
      );
    },
  },
  {
    id: "actions",
    cell: ({ row }) => (
      <DataTableRowActions row={row} deleteTicket={deleteTicket} />
    ),
  },
];
