const clerkPubKey =
  "pk_test_c3RlcmxpbmctbW9yYXktOTAuY2xlcmsuYWNjb3VudHMuZGV2JA";

import {
  ClerkProvider,
  SignedIn,
  SignedOut,
  UserButton,
  useUser,
  RedirectToSignIn,
} from "@clerk/clerk-react";

function LoginPage() {
  return (
    <ClerkProvider publishableKey={clerkPubKey}>
      <SignedIn>
        <Welcome />
      </SignedIn>
      <SignedOut>
        <RedirectToSignIn />
      </SignedOut>
    </ClerkProvider>
  );
}

function Welcome() {
  return <div>Hello you are signed in</div>;
}

export default LoginPage;
