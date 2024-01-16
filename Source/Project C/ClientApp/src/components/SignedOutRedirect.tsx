import { SignedOut } from "@clerk/clerk-react";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

const SignedOutRedirect = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const handleSignOut = () => {
      <SignedOut>{navigate("/")}</SignedOut>;
    };

    handleSignOut();
  }, []);

  return null;
};

export default SignedOutRedirect;
