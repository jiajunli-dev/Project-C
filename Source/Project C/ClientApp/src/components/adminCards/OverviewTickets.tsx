"use client";

import { Bar, BarChart, ResponsiveContainer, XAxis, YAxis } from "recharts";
import OverviewTicketsData from "./GetData/OverviewTicketsData";

export function Overview() {
  const overviewData: { [key: string]: number } = OverviewTicketsData();
  const data = [
    {
      name: "Jan",
      total: overviewData?.["1"] || 0,
    },
    {
      name: "Feb",
      total: overviewData?.["2"] || 0,
    },
    {
      name: "Mar",
      total: overviewData?.["3"] || 0,
    },
    {
      name: "Apr",
      total: overviewData?.["4"] || 0,
    },
    {
      name: "May",
      total: overviewData?.["5"] || 0,
    },
    {
      name: "Jun",
      total: overviewData?.["6"] || 0,
    },
    {
      name: "Jul",
      total: overviewData?.["7"] || 0,
    },
    {
      name: "Aug",
      total: overviewData?.["8"] || 0,
    },
    {
      name: "Sep",
      total: overviewData?.["9"] || 0,
    },
    {
      name: "Oct",
      total: overviewData?.["10"] || 0,
    },
    {
      name: "Nov",
      total: overviewData?.["11"] || 0,
    },
    {
      name: "Dec",
      total: overviewData?.["12"] || 0,
    },
  ];
  const maxTickets = Math.max(...data.map((item) => item.total + 2));
  const ticks = Array.from({ length: maxTickets + 1 }, (_, i) => i);
  return (
    <ResponsiveContainer width="100%" height={350}>
      <BarChart data={data}>
        <XAxis
          dataKey="name"
          stroke="#888888"
          fontSize={12}
          tickLine={false}
          axisLine={false}
        />
        <YAxis
          stroke="#888888"
          fontSize={12}
          tickLine={false}
          axisLine={false}
          tickFormatter={(value) => `${value}`}
          ticks={ticks}
        />
        <Bar dataKey="total" fill="#199CCE" radius={[4, 4, 0, 0]} />
      </BarChart>
    </ResponsiveContainer>
  );
}
