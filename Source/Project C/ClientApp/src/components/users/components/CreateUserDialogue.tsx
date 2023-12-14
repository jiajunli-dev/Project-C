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
import { Checkbox } from "@/components/ui/checkbox";
import { useToast } from "@/components/ui/use-toast";
import { useState } from "react";
// TODO backend handle creating a user
export default function CreateUserDialogue() {
  const { toast } = useToast();
  const [open, setOpen] = useState(false);
  const wait = () => new Promise((resolve) => setTimeout(resolve, 100));

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
              <Input id="name" className="col-span-3 dark:bg-inherit dark:border-slate-700 dark:text-white" />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="username" className="text-right dark:text-white">
                Username
              </Label>
              <Input id="username" className="col-span-3  dark:bg-inherit dark:border-slate-700 dark:text-white" />
            </div>
            <div className="grid grid-cols-4 items-center gap-4">
              <Label htmlFor="employee" className="text-right dark:text-white ">
                Employee
              </Label>
              <Checkbox id="employee" className="col-span-3  dark:border-slate-700 dark:text-white" />
            </div>
          </div>
          <DialogFooter>
            <form
              onSubmit={(event) => {
                wait().then(() => setOpen(false));
                event.preventDefault();
                toast({
                  description: "User successfully added.",
                  duration: 3000,
                });
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
