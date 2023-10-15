import AdminStatCard from "../components/adminCards/ActiveTicketsCard";
import AdminTable from "../components/AdminTable";
import ClosedTicketsCard from "../components/adminCards/ClosedTicketsCard";
import MalfunctionCard from "../components/adminCards/MalfunctionCard";
import SideBar from "../components/SideBar";

const AdminDashboard = () => {
  return (
    <>
      <div className="flex gap-5 ">
        <SideBar />
        <div className="w-full">
          <div className="grid grid-rows-3 pr-5">
            <div className="grid grid-cols-3 h-min gap-6">
              <AdminStatCard></AdminStatCard>
              <ClosedTicketsCard></ClosedTicketsCard>
              <MalfunctionCard></MalfunctionCard>
            </div>
            <div></div>
            <div>
              <AdminTable></AdminTable>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
export default AdminDashboard;
