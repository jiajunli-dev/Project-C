import { Card } from "flowbite-react";
import warningIcon from "../../assets/warning.png";

interface MalfunctionCardProps {
  count: number;
}

export default function MalfunctionCard({ count }: MalfunctionCardProps) {
  return (
    <Card className="min-w-[15rem] mt-2" href="#">
      <div className="flex gap-10">
        <img className="w-14" src={warningIcon} alt="" />
        <div>
          <h5 className="text-2xl font-bold tracking-tight text-gray-900 dark:text-white">
            <p>{count}</p>
          </h5>
          <ul className="font-normal text-gray-700 dark:text-gray-400">
            <li>Current Malfunctions</li>
          </ul>
        </div>
      </div>
    </Card>
  );
}
