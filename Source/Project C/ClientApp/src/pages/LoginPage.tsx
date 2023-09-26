import { ClerkProvider } from "@clerk/clerk-react";

const clerkPubKey =
  "pk_test_c3RlcmxpbmctbW9yYXktOTAuY2xlcmsuYWNjb3VudHMuZGV2JA";

function LoginPage() {
  return (
    <ClerkProvider publishableKey={clerkPubKey}>
      <div className="bg-red-500">Hello from clerk</div>
    </ClerkProvider>
  );
}

export default LoginPage;
