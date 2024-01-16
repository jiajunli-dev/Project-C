export class baseService {
    constructor(protected baseUrl: string = "http://api.platiumx.com") { }

    protected createRequest(method: string, token: string, endpoint: string, data?: string): Request {
        const tokenParts = token.split('.');
        const payload = JSON.parse(atob(tokenParts[1]));
        const expirationTime = (payload.exp + 5) * 1000;

        if (Date.now() >= expirationTime) throw new Error('Token has expired');

        const headers = {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`,
        };
        const body = data ? data : undefined;

        return new Request(`${this.baseUrl}/${endpoint}`, { method, headers, body });
    }
}
