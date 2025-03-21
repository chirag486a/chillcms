import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { AuthProvider } from "./providers/AuthProvider.jsx";
import "./index.css";
import App from "./App.jsx";
import ToastProvider from "./pages/Components/ToastContainer.jsx";

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <AuthProvider>
      <ToastProvider>
        <App />
      </ToastProvider>
    </AuthProvider>
  </StrictMode>
);
