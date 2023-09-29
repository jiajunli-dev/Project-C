import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import "./index.css";
import { ClerkProvider } from "@clerk/clerk-react";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <ClerkProvider
      publishableKey={
        "pk_test_c3RpcnJpbmctbXVza294LTMuY2xlcmsuYWNjb3VudHMuZGV2JA"
      }
    >
      <App />
    </ClerkProvider>
  </React.StrictMode>
);
