import Parent from "./Parent";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import HomePage from "./pages/HomePage";
import LoginPage from "./pages/LoginPage";
import CreateTicket from "./pages/CreateTicket";
import ApiTest from "./pages/ApiTest";
import { ThemeProvider } from "@/components/DarkMode/DarkMode";
import TicketPage from "./pages/TicketPage";
import AdminTickets from "@/components/tickets/AdminTicketsPage";
import ViewUsers from "./components/users/ViewUsers";
import ArticlePage from "./components/Article/ArticlePage/ArticlePage";
import { ArticlesContext } from "./components/Article/ArticleContext";
import { Articles } from "./components/Article/Articles";
import ArticlesPage from "./pages/ArticlesPage";
import FAQPage from "./pages/FAQPage";
function App() {
  return (
    <>
      <ArticlesContext.Provider value={Articles}>
        <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
          <Router>
            <Routes>
              <Route path="/" element={<Parent />}>
                <Route path="/" element={<HomePage />} />
                <Route path="/authentication" element={<LoginPage />} />
                <Route path="/create-ticket" element={<CreateTicket />} />
                <Route path="/view-tickets" element={<AdminTickets />} />
                <Route path="/view-users" element={<ViewUsers />} />
                <Route path="/ApiTest" element={<ApiTest />} />
                <Route path="/ticket/:id" element={<TicketPage />} />
                <Route path="/faq" element={<FAQPage />} />
                <Route path="/article/:id" element={<ArticlePage />} />
                <Route path="/articles" element={<ArticlesPage />} />
              </Route>
            </Routes>
          </Router>
        </ThemeProvider>
      </ArticlesContext.Provider>
    </>
  );
}

export default App;
