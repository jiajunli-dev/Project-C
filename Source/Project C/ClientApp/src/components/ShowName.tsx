import {  useUser } from "@clerk/clerk-react";


function capitalizeFirstLetter(str: any) {
  return str.charAt(0).toUpperCase() + str.slice(1);
}

export default function Home() {
  const { isSignedIn, user, isLoaded } = useUser();

  if (!isLoaded) {
    return null;
  }

  if (isSignedIn) {
    return (
      <div className="dark:text-white">{capitalizeFirstLetter(user.username)}</div>
    );
  }

  return null;
}
