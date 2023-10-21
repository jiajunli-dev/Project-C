import { useClerk } from '@clerk/clerk-react';

const baseUrl = "https://localhost:7004";
const tokenType = 'dev_token';

async function getToken(name: string): Promise<string | null> {
    const clerk = useClerk();
    const token = await clerk.session?.getToken({ template: name })

    if (token == null) {
        return null;
    } else {
        return token;
    }
}

export async function getAllTickets() {
    try {
        const accessToken = await getToken(tokenType);
        const request = new Request(`${baseUrl}/tickets`, {
            headers: {
                Authorization: `Bearer ${accessToken}`,
            },
        });

        const response = await fetch(request);
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        console.log(data);
        return data;
    } catch (error) {
        console.error(error);
    }
}
