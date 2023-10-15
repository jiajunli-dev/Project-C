import { Card } from "flowbite-react";

export default function DefaultCard() {
  return (
    <Card className="min-w-[15rem]  mt-2" href="#">
      <h5 className="text-2xl font-bold tracking-tight text-gray-900 dark:text-white">
        <p>Current Malfunctions</p>
      </h5>
      <ul className="font-normal text-gray-700 dark:text-gray-400">
        <li>0</li>
      </ul>
    </Card>
  );
}
