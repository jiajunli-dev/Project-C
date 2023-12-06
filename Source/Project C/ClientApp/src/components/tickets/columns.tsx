"use client";
import ColumnHeader from "@/components/tickets/components/ColumnHeader";
import { ColumnDef } from "@tanstack/react-table";
import { Checkbox } from "../ui/checkbox";
import { Ticket } from "@/models/Ticket";
import { DataTableRowActions } from "./components/data-table-row-actions";

export const columns: ColumnDef<Ticket>[] = [
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
    header: ({ column }) => <ColumnHeader column={column} title="ID" />,
    cell: ({ row }) => <div className="w-[80px]">{row.getValue("id")}</div>,
  },
  {
    accessorKey: "createdBy",
    header: ({ column }) => {
      return <ColumnHeader column={column} title="Created By" className="bg-red-500" />;
    },
  },
  {
    accessorKey: "priority",
    header: ({ column }) => {
      return <ColumnHeader column={column} title="Priority" />;
    },
  },
  {
    accessorKey: "status",
    header: ({ column }) => {
      return <ColumnHeader column={column} title="Status" />;
    },
  },
  {
    id: "actions",
    cell: ({ row }) => <DataTableRowActions row={row} />,
  },
];
