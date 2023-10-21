import AdminStatCard from "../components/adminCards/ActiveTicketsCard";
import AdminTable from "../components/AdminTable";
import ClosedTicketsCard from "../components/adminCards/ClosedTicketsCard";
import MalfunctionCard from "../components/adminCards/MalfunctionCard";
import SideBar from "../components/SideBar";
import TotalTickets from "../components/adminCards/TotalTickets";

const AdminDashboard = () => {
  return (
    <>
      <div className="flex gap-10 bg-[#F8F8F8] min-h-screen">
        <SideBar />
        <div className="w-full bg-[#F8F8F8] mt-10">
          <div className="flex flex-col gap-16 pr-5">
            <main
              className="grid grid-cols-[repeat(4,minmax(100px,500px))] h-min gap-6"
            >
              <TotalTickets count={15}></TotalTickets>
              <AdminStatCard count={8}></AdminStatCard>
              <ClosedTicketsCard count={7}></ClosedTicketsCard>
              <MalfunctionCard count={1}></MalfunctionCard>
            </main>
            <div>
              <div className="bg-white p-6 rounded-[0.4rem]">
              <AdminTable></AdminTable>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
export default AdminDashboard;
