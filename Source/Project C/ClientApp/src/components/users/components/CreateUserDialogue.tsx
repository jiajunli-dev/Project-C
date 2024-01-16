import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { useToast } from "@/components/ui/use-toast";
import { useState } from "react";
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { useSignUp, useClerk } from '@clerk/clerk-react';
import { employeeService } from "@/services/employeeService";
import { customerService } from "@/services/customerService";
import { CreateCustomer } from "@/models/CreateCustomer";
import { CreateEmployee } from "@/models/CreateEmployee";

export default function CreateUserDialogue() {
  const { toast } = useToast();
  const [open, setOpen] = useState(false);
  const clerk = useClerk();
  const employeeRepository = new employeeService();
  const customerRepository = new customerService();
  const tokenType = 'api_token';
  const wait = () => new Promise((resolve) => setTimeout(resolve, 100));

  const [username, setUsername] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [selectedRole, setSelectedRole] = useState("customer");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [companyName, setCompanyName] = useState("");
  const [departmentName, setDepartmentName] = useState("");
  const [companyPhoneNumber, setCompanyPhoneNumber] = useState("");

  const { isLoaded, signUp } = useSignUp();

  // todo handle loading state
  if (!isLoaded)
    return null;

  const handleUserCreation = async (event: any) => {
    event.preventDefault();

    try {
      const token = await clerk.session?.getToken({ template: tokenType });
      if (token) {
        var result;

        switch (selectedRole) {
          case "customer":
            const model = new CreateCustomer();
            model.username = username;
            model.firstName = firstName;
            model.lastName = lastName;
            model.email = email;
            model.phoneNumber = phoneNumber;
            model.companyName = companyName;
            model.companyPhoneNumber = companyPhoneNumber;
            model.departmentName = departmentName;

            result = await customerRepository.create(token, model);
            break;
          case "employee":
            result = await employeeRepository.create(token, {
              username, firstName, lastName,
              email, phoneNumber, role: selectedRole,
            } as CreateEmployee);
            break;
          case "admin":
            result = await employeeRepository.create(token, {
              username, firstName, lastName,
              email, phoneNumber, role: selectedRole,
            } as CreateEmployee);
            break;
        }

        if (result) {
          wait().then(() => setOpen(false));
          toast({
            description: "User successfully added with ID: " + result.id,
            duration: 3500,
          });
        }
        else {
          toast({
            description: "User creation failed",
            duration: 3500,
          });
        }
      }
    } catch (error) {
      console.log(error);
    }

    event.preventDefault();
  };

  return (
    <div className="mx-auto">
      <Dialog open={open} onOpenChange={setOpen}>
        <DialogTrigger asChild>
          <Button variant="outline" className="dark:text-white">
            Add User
          </Button>
        </DialogTrigger>
        <DialogContent className="sm:max-w-[425px]">
          <DialogHeader>
            <DialogTitle className="dark:text-white">Add a user</DialogTitle>
          </DialogHeader>
          <div className="grid gap-4 py-4">
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="username" className="text-right dark:text-white">
                Username
              </Label>
              <Input
                id="username"
                className="col-span-3  dark:bg-inherit dark:border-slate-700 dark:text-white"
                onChange={(e) => setUsername(e.target.value)}
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="email" className="text-right dark:text-white">
                Email
              </Label>
              <Input
                id="email"
                className="col-span-3  dark:bg-inherit dark:border-slate-700 dark:text-white"
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="firstName" className="text-right dark:text-white">
                First Name
              </Label>
              <Input
                id="firstName"
                className="col-span-3 dark:bg-inherit dark:border-slate-700 dark:text-white"
                onChange={(e) => setFirstName(e.target.value)}
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="lastName" className="text-right dark:text-white">
                Last Name
              </Label>
              <Input
                id="lastName"
                className="col-span-3 dark:bg-inherit dark:border-slate-700 dark:text-white"
                onChange={(e) => setLastName(e.target.value)}
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="phoneNumber" className="text-right dark:text-white">
                Phone Number
              </Label>
              <Input
                id="phoneNumber"
                className="col-span-3 dark:bg-inherit dark:border-slate-700 dark:text-white"
                onChange={(e) => setPhoneNumber(e.target.value)}
              />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="employee" className="text-right dark:text-white ">
                Roles
              </Label>
              <Select onValueChange={setSelectedRole}>
                <SelectTrigger className="w-[180px]">
                  <SelectValue placeholder="Select a role" />
                </SelectTrigger>
                <SelectContent>
                  <SelectGroup>
                    <SelectItem value="customer">
                      <span className="dark:text-white">Customer</span>
                    </SelectItem>
                    <SelectItem value="employee">
                      <span className="dark:text-white">Employee</span>
                    </SelectItem>
                    <SelectItem value="admin">
                      <span className="dark:text-white">Admin</span>
                    </SelectItem>
                  </SelectGroup>
                </SelectContent>
              </Select>
            </div>

            {selectedRole === "customer" && (
              <div className="grid gap-4 py-4">
                <div className="grid grid-cols-4 items-center gap-4">
                  <Label htmlFor="departmentName" className="text-right dark:text-white">
                    Department Name
                  </Label>
                  <Input
                    id="departmentName"
                    className="col-span-3  dark:bg-inherit dark:border-slate-700 dark:text-white"
                    onChange={(e) => setDepartmentName(e.target.value)}
                  />
                </div>

                <div className="grid grid-cols-4 items-center gap-4">
                  <Label htmlFor="companyName" className="text-right dark:text-white">
                    Company Name
                  </Label>
                  <Input
                    id="companyName"
                    className="col-span-3  dark:bg-inherit dark:border-slate-700 dark:text-white"
                    onChange={(e) => setCompanyName(e.target.value)}
                  />
                </div>

                <div className="grid grid-cols-4 items-center gap-4">
                  <Label htmlFor="companyPhoneNumber" className="text-right dark:text-white">
                    Company Phone Number
                  </Label>
                  <Input
                    id="companyPhoneNumber"
                    className="col-span-3  dark:bg-inherit dark:border-slate-700 dark:text-white"
                    onChange={(e) => setCompanyPhoneNumber(e.target.value)}
                  />
                </div>
              </div>
            )}

          </div>
          <DialogFooter>
            <form
              onSubmit={(event) => {
                handleUserCreation(event);
              }}
            >
              <Button type="submit">Add user</Button>
            </form>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
}
