"use client";

import { Row } from "@tanstack/react-table";

import { Button } from "../../ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "../../ui/dropdown-menu";
import useIsDarkMode from "../../IsDarkModeChecker";
import { employeeService } from "@/services/employeeService";
import { useClerk } from "@clerk/clerk-react";
interface DataTableRowActionsProps<TData> {
  row: Row<TData>;
}

export function EmployeeDataTableRowActions<TData>({
  row,
}: DataTableRowActionsProps<TData>) {
  const isDarkMode = useIsDarkMode();
  const clerk = useClerk();
  const tokenType = "api_token";
  const deleteUser = async (userID: number) => {
    try {
      const token = await clerk.session?.getToken({ template: tokenType });
      const service = new employeeService();
      if (token) {
        await service.delete(token, userID);
        window.location.reload();
      }
    } catch (error) {
      console.error("Error deleting ticket:", error);
    }
  };

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button
          variant="ghost"
          className="flex h-8 w-8 p-0 data-[state=open]:bg-muted"
        >
          <div className="h-4 w-4">
            <svg
              width="15"
              height="15"
              viewBox="0 0 15 15"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                d="M3.625 7.5C3.625 8.12132 3.12132 8.625 2.5 8.625C1.87868 8.625 1.375 8.12132 1.375 7.5C1.375 6.87868 1.87868 6.375 2.5 6.375C3.12132 6.375 3.625 6.87868 3.625 7.5ZM8.625 7.5C8.625 8.12132 8.12132 8.625 7.5 8.625C6.87868 8.625 6.375 8.12132 6.375 7.5C6.375 6.87868 6.87868 6.375 7.5 6.375C8.12132 6.375 8.625 6.87868 8.625 7.5ZM12.5 8.625C13.1213 8.625 13.625 8.12132 13.625 7.5C13.625 6.87868 13.1213 6.375 12.5 6.375C11.8787 6.375 11.375 6.87868 11.375 7.5C11.375 8.12132 11.8787 8.625 12.5 8.625Z"
                fill={isDarkMode ? "#fff" : "#000"}
                fillRule="evenodd"
                clipRule="evenodd"
              ></path>
            </svg>
          </div>

          <span className="sr-only">Open menu</span>
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end" className="w-[160px]">
        <DropdownMenuItem
          onClick={() => deleteUser(row.original.id)}
          className="dark:text-white"
        >
          Delete
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
