import Parent from "./Parent";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import HomePage from "./pages/HomePage";
import LoginPage from "./pages/LoginPage";
import CreateTicket from "./pages/CreateTicket";
import AdminDashboard from "./pages/AdminTickets";
import Users from "./pages/Users";
import ApiTest from "./pages/ApiTest";
import { ThemeProvider } from "@/components/DarkMode";

function App() {
  return (
    <>
      <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
        <Router>
          <Routes>
            <Route path="/" element={<Parent />}>
              <Route path="/" element={<HomePage />} />
              <Route path="/authentication" element={<LoginPage />} />
              <Route path="/users" element={<Users />} />
              <Route path="/tickets" element={<AdminDashboard />} />
              <Route path="/create-ticket" element={<CreateTicket />} />
              <Route path="/ApiTest" element={<ApiTest />} />
            </Route>
          </Routes>
        </Router>
      </ThemeProvider>
    </>
  );
}

export default App;
