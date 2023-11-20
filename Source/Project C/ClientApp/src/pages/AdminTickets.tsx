import AdminStatCard from "../components/adminCards/ActiveTicketsCard";
import AdminTable from "../components/AdminTable";
import ClosedTicketsCard from "../components/adminCards/ClosedTicketsCard";
import MalfunctionCard from "../components/adminCards/MalfunctionCard";
import TotalTickets from "../components/adminCards/TotalTickets";

const AdminTickets = () => {
  return (
    <div className="w-full px-8">
      <div className="flex flex-col gap-8">
        <div className="grid grid-cols-[repeat(4,minmax(100px,500px))] gap-6">
          <TotalTickets count={15}></TotalTickets>
          <AdminStatCard count={8}></AdminStatCard>
          <ClosedTicketsCard count={7}></ClosedTicketsCard>
          <MalfunctionCard count={1}></MalfunctionCard>
        </div>
        <div>
          <AdminTable/>
        </div>
      </div>
    </div>
  );
};
export default AdminTickets;
