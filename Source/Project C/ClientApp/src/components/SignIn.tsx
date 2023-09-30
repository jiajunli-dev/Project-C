import { SignIn } from "@clerk/clerk-react";
import "../header.css"

const SignInPage = () => (
  <SignIn
    path="/sign-in"
    routing="path"
    signUpUrl="/sign-up"
    appearance={{ elements: { footer: { display: "none" } }, }}
  />
);
export default SignInPage;
