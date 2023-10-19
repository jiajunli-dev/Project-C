import { Card } from "flowbite-react";
import ticketsIcon from "../../assets/tickets.png";

interface TotalTicketsCardProps {
  count: number;
}

export default function TotalTicketsCard({ count }: TotalTicketsCardProps) {
  return (
    <Card className="min-w-[15rem] mt-2" href="#">
      <div className="flex gap-10">
        <img className="w-16" src={ticketsIcon} alt="" />
        <div>
          <h5 className="text-2xl font-bold tracking-tight text-gray-900 dark:text-white">
            <p>{count}</p>
          </h5>
          <ul className="font-normal text-gray-700 dark:text-gray-400">
            <li>Total Tickets</li>
          </ul>
        </div>
      </div>
    </Card>
  );
}
