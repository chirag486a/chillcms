import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Register from "./pages/Register";
import Login from "./pages/Login";
import Home from "./pages/Home";
import Dashboard from "./pages/Dashboard";
// sub-dashboard
import DashboardHome from "./pages/Dashboard/Home";
import ContentManagement from "./pages/Dashboard/ContentManagement";
import UserManagement from "./pages/Dashboard/UserManagement";
import Todos from "./pages/Dashboard/Todos";
import UserFeedback from "./pages/Dashboard/UserFeedback";

function App() {
  return (
    <div className="App">
      <div className="h-dvh bg-base-100 font-sans">
        <Router>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/register" element={<Register />} />
            <Route path="/login" element={<Login />} />
            <Route path="/dashboard" element={<Dashboard />}>
              <Route path="/dashboard" element={<DashboardHome />} />
              <Route
                path="/dashboard/content-management"
                element={<ContentManagement />}
              />
              <Route
                path="/dashboard/user-management"
                element={<UserManagement />}
              />
              <Route path="/dashboard/todos" element={<Todos />} />
              <Route
                path="/dashboard/user-feedback"
                element={<UserFeedback />}
              />
            </Route>
          </Routes>
        </Router>
      </div>
    </div>
  );
}

export default App;
