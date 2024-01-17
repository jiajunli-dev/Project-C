"use client";
import ColumnHeader from "@/components/tickets/components/ColumnHeader";
import { ColumnDef } from "@tanstack/react-table";
import { Checkbox } from "../../ui/checkbox";
import { EmployeeDataTableRowActions } from "./employee-data-table-row-actions";
import { Customer } from "@/models/Customer";

export const columns: ColumnDef<Customer>[] = [
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
    accessorKey: "username",
    header: ({ column }) => {
      return (
        <ColumnHeader
          column={column}
          title="Username"
          className="dark:[&>button>span]:text-white"
        />
      );
    },
  },
  {
    accessorKey: "email",
    header: ({ column }) => {
      return (
        <ColumnHeader
          column={column}
          title="Email"
          className="dark:[&>button>span]:text-white"
        />
      );
    },
  },
  {
    id: "actions",
    cell: ({ row }) => <EmployeeDataTableRowActions row={row} />,
  },
];
