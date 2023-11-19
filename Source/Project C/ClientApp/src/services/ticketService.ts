import { Ticket } from "../models/Ticket";
import { CreateTicket } from "../models/CreateTicket";
import { Photo } from "../models/Photo";

export class TicketService {
    // TODO: Move to config
    constructor(private baseUrl: string = "https://localhost:7004") { }

    async getAll(token: string): Promise<Ticket[] | undefined> {
        const request = this.createRequest("GET", token, "Ticket");
        const response = await fetch(request);
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        if (response.status === 204) {
            return [];
        }

        const data = await response.json();

        return data.map((ticketData: any) => {
            return Ticket.fromJson(ticketData);
        });
    }

    async getById(token: string, ticketId: number): Promise<Ticket | undefined> {
        const request = this.createRequest("GET", token, `Ticket/${ticketId}`);
        const response = await fetch(request);
        if (response.status === 204 || response.status === 400) {
            return undefined;
        }
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        return Ticket.fromJson(data);
    }

    async getPhotosById(token: string, ticketId: number): Promise<Photo[] | undefined> {
        const request = this.createRequest("GET", token, `Ticket/${ticketId}/photos`);
        const response = await fetch(request);
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        return data.map((photo: any) => {
            return Photo.fromJson(photo);
        });
    }

    async create(token: string, model: CreateTicket): Promise<Ticket | undefined> {
        const request = this.createRequest("POST", token, "Ticket", model.toJSON());
        const response = await fetch(request);
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        return Ticket.fromJson(data);
    }

    async update(token: string, model: Ticket): Promise<Ticket | undefined> {
        var json = model.toJSON();
        const request = this.createRequest("PUT", token, "Ticket", json);
        const response = await fetch(request);
        if (!response.ok) {
            if (response.status === 400) {
                const data = await response.json();
                throw new Error(data);
            }
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        return Ticket.fromJson(data);
    }

    async delete(token: string, ticketId: number) {
        const request = this.createRequest("DELETE", token, `Ticket/${ticketId}`);
        const response = await fetch(request);
        if (response.status !== 204) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
    }

    private createRequest(method: string, token: string, endpoint: string, data?: string): Request {
        const tokenParts = token.split('.');
        const payload = JSON.parse(atob(tokenParts[1]));
        const expirationTime = (payload.exp + 5) * 1000;

        if (Date.now() >= expirationTime) {
            throw new Error('Token has expired');
        }

        const headers = {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`,
        };
        const body = data ? data : undefined;
        return new Request(`${this.baseUrl}/${endpoint}`, { method, headers, body });
    }
}
