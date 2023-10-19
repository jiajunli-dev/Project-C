import { Card } from "flowbite-react";
import hourGlassIcon from "../../assets/hourglass.png";

interface DefaultCardProps {
  count: number;
}

export default function DefaultCard({ count }: DefaultCardProps) {
  return (
    <Card className="min-w-[15rem] mt-2" href="#">
      <div className="flex gap-10">
        <img className="w-14" src={hourGlassIcon} alt="" />
        <div>
          <h5 className="text-2xl font-bold tracking-tight text-gray-900 dark:text-white">
            <p className="">{count}</p>
          </h5>
          <ul className="font-normal text-gray-700 dark:text-gray-400">
            <li>Active Tickets</li>
          </ul>
        </div>
      </div>
    </Card>
  );
}
