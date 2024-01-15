import { useClerk } from "@clerk/clerk-react";
import { useEffect, useState } from "react";
import { customerService } from "@/services/customerService";
export default function TotalTickets() {
  const [result, setResult] = useState<number>();
  const clerk = useClerk();
  const tokenType = "api_token";

  useEffect(() => {
    async function fetchDataAsync() {
      try {
        const token = await clerk.session?.getToken({ template: tokenType });
        const service = new customerService();
        if (token) {
          const data = await service.getAll(token);
          setResult(data?.length);
        } else {
          console.error("Token not retrieved");
        }
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    }

    fetchDataAsync();
  }, [clerk.session]);
  return result;
}
