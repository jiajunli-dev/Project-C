import React from "react";
import ReactDOM from "react-dom/client";


import "./css/index.css";
import { ClerkProvider } from "@clerk/clerk-react";
import App from "./App";

ReactDOM.createRoot(document.getElementById("root")!).render(
  
  <React.StrictMode>
    <ClerkProvider
      // TODO move to config
      publishableKey={
        "pk_test_c3RpcnJpbmctbXVza294LTMuY2xlcmsuYWNjb3VudHMuZGV2JA"
      }
    >
      <App />
    </ClerkProvider>
  </React.StrictMode>
);
