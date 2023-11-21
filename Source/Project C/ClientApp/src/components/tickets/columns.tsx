"use client";
import ColumnHeader from "@/components/tickets/components/ColumnHeader";
import { ColumnDef } from "@tanstack/react-table";
import { Checkbox } from "../ui/checkbox";
import { DataTableTicket } from "@/types";

export const columns: ColumnDef<DataTableTicket>[] = [
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
        className="translate-y-[2px]"
      />
    ),
    cell: ({ row }) => (
      <Checkbox
        checked={row.getIsSelected()}
        onCheckedChange={(value) => row.toggleSelected(!!value)}
        aria-label="Select row"
        className="translate-y-[2px]"
      />
    ),
    enableSorting: false,
    enableHiding: false,
  },
  {
    accessorKey: "id",
    header: ({ column }) => <ColumnHeader column={column} title="ID" />,
    cell: ({ row }) => <div className="w-[80px]">{row.getValue("id")}</div>,
  },
  {
    accessorKey: "requestedBy",
    header: ({ column }) => {
      return <ColumnHeader column={column} title="Requested By" />;
    },
  },
  {
    accessorKey: "machine",
    header: ({ column }) => {
      return <ColumnHeader column={column} title="Machine" />;
    },
  },
  {
    accessorKey: "assignee",
    header: ({ column }) => {
      return <ColumnHeader column={column} title="Assignee"/>;
    },
  },
  {
    accessorKey: "priority",
    header: ({ column }) => {
      return <ColumnHeader column={column} title="Priority"/>;
    },
  },
  {
    accessorKey: "status",
    header: ({ column }) => {
      return <ColumnHeader column={column} title="Status"/>;
    },
  },
  {
    accessorKey: "createdAt",
    header: ({ column }) => {
      return <ColumnHeader column={column} title="Created At"/>;
    },
  },
];
