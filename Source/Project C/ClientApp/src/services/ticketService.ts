import { Ticket } from "../models/Ticket";
import { CreateTicket } from "../models/CreateTicket";
import { Photo } from "../models/Photo";
import { baseService } from "./BaseService";

export class TicketService extends baseService {
    // TODO: Move api url to config
    constructor(baseUrl: string = "https://localhost:7004") { super(baseUrl); }

    async getAll(token: string): Promise<Ticket[] | undefined> {
        if (!token) throw new Error('Token is required.');

        const request = this.createRequest("GET", token, "Ticket");
        const response = await fetch(request);
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
        if (response.status === 204) return [];

        const data = await response.json();

        return data.map((ticketData: any) => Ticket.fromJson(ticketData));
    }

    async getById(token: string, ticketId: number): Promise<Ticket | undefined> {
        if (!token) throw new Error('Token is required.');
        if (!ticketId) throw new Error('TicketId is required.');

        const request = this.createRequest("GET", token, `Ticket/${ticketId}`);
        const response = await fetch(request);
        if (response.status === 204 || response.status === 400) return undefined;
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

        const data = await response.json();

        return Ticket.fromJson(data);
    }

    async getPhotosById(token: string, ticketId: number): Promise<Photo[] | undefined> {
        if (!token) throw new Error('Token is required.');
        if (!ticketId) throw new Error('TicketId is required.');

        const request = this.createRequest("GET", token, `Ticket/${ticketId}/photos`);
        const response = await fetch(request);
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

        const data = await response.json();
        return data.map((photo: any) => Photo.fromJson(photo));
    }

    async create(token: string, model: CreateTicket): Promise<Ticket | undefined> {
        if (!token) throw new Error('Token is required.');
        if (!model) throw new Error('Model is required.');

        const errors = model.validate();
        if (errors.length > 0) throw new Error(errors.join('\n'));

        const request = this.createRequest("POST", token, "Ticket", model.toJSON());
        const response = await fetch(request);
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

        const data = await response.json();

        return Ticket.fromJson(data);
    }

    async update(token: string, model: Ticket): Promise<Ticket | undefined> {
        if (!token) throw new Error('Token is required.');
        if (!model) throw new Error('Model is required.');

        const errors = model.validate();
        if (errors.length > 0) throw new Error(errors.join('\n'));

        const request = this.createRequest("PUT", token, "Ticket", model.toJSON());
        const response = await fetch(request);
        if (!response.ok) {
            if (response.status === 400) {
                const data = await response.json();
                throw new Error(data);
            } else if (response.status === 409) {
                const data = await response.json();
                throw new Error(data);
            }

            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        return Ticket.fromJson(data);
    }

    async delete(token: string, ticketId: number): Promise<void> {
        if (!token) throw new Error('Token is required.');
        if (!ticketId) throw new Error('TicketId is required.');

        const request = this.createRequest("DELETE", token, `Ticket/${ticketId}`);
        const response = await fetch(request);

        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
    }
}
