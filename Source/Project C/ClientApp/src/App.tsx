import Parent from "./Parent";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import HomePage from "./pages/HomePage";
import LoginPage from "./pages/LoginPage";
import CreateTicket from "./pages/CreateTicket";
import ApiTest from "./pages/ApiTest";
import { ThemeProvider } from "@/components/DarkMode";
import AdminTickets from "@/components/tickets/page";
import MachineInfoForm from "./pages/MachineInfoForm";

function App() {
  return (
    <>
      <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
        <Router>
          <Routes>
            <Route path="/" element={<Parent />}>
              <Route path="/" element={<HomePage />} />
              <Route path="/authentication" element={<LoginPage />} />
              <Route path="/create-ticket" element={<CreateTicket />} />
              <Route path="/view-tickets" element={<AdminTickets />} />
              <Route path="/ApiTest" element={<ApiTest />} />
              <Route path="/MachineInfoForm" element={<MachineInfoForm />} />
            </Route>
          </Routes>
        </Router>
      </ThemeProvider>
    </>
  );
}

export default App;
