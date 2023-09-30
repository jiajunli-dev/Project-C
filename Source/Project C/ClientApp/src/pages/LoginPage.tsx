import { SignIn } from "@clerk/clerk-react";
import "../loginpage.css"

function LoginPage() {
  return <div className="signin"><SignIn></SignIn></div>;
}

export default LoginPage;
