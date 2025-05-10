import {
  BrowserRouter as Router,
  Routes,
  Route,
  Outlet,
} from "react-router-dom";
import Register from "./pages/Register";
import Login from "./pages/Login";
import Home from "./pages/Home";
import Dashboard from "./pages/Dashboard";
import { AuthContext } from "./contexts/AuthContext";
import { useContext } from "react";
import ProtectedRoute from "./pages/Components/ProtectedRoute";

function App() {
  const { currentUser } = useContext(AuthContext);

  return (
    <div className="App">
      <div className="h-dvh bg-base-100 font-sans">
        <Router>
          <Routes>
            <Route
              path="/"
              element={
                <ProtectedRoute>
                  <Home />
                </ProtectedRoute>
              }
            />
            <Route path="/register" element={<Register />} />
            <Route path="/login" element={<Login />} />
            <Route
              path="/dashboard/*"
              element={
                <ProtectedRoute>
                  <Dashboard />
                </ProtectedRoute>
              }
            />
          </Routes>
        </Router>
        <div>
          <Outlet />
        </div>
      </div>
    </div>
  );
}

export default App;
