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
import { useUser } from "@clerk/clerk-react";

export default function CreateUserDialogue() {
  const { user } = useUser();

  const { toast } = useToast();
  const [open, setOpen] = useState(false);
  const wait = () => new Promise((resolve) => setTimeout(resolve, 100));
  const [name, setName] = useState("");
  const [username, setUsername] = useState("");
  const [selectedRole, setSelectedRole] = useState("");

  const handleUserCreation = (event: any) => {
    event.preventDefault();
    // TODO backend handle the object userToBeAdded
    const userToBeAdded = { name, username, selectedRole };
    console.log(userToBeAdded);
    wait().then(() => setOpen(false));
    toast({
      description: "User successfully added.",
      duration: 3500,
    });
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
              <Label htmlFor="name" className="text-right dark:text-white">
                Name
              </Label>
              <Input
                id="name"
                className="col-span-3 dark:bg-inherit dark:border-slate-700 dark:text-white"
                onChange={(e) => setName(e.target.value)}
              />
            </div>
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

                    <SelectItem
                      disabled={user?.publicMetadata.role !== "admin"}
                      value="employee"
                    >
                      <span className="dark:text-white">Employee</span>
                    </SelectItem>
                    <SelectItem
                      disabled={user?.publicMetadata.role !== "admin"}
                      value="admin"
                    >
                      <span className="dark:text-white">Admin</span>
                    </SelectItem>
                  </SelectGroup>
                </SelectContent>
              </Select>
            </div>
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
