import { CreateMalfunction } from "../models/CreateMalfunction";
import { Malfunction } from "../models/Malfunction";
import { baseService } from "./BaseService";

export class malfunctionService extends baseService{
    constructor(baseUrl: string = "https://localhost:7004") { super(baseUrl); }

    async getAll(token: string): Promise<Malfunction[] | undefined> {
        const request = this.createRequest("GET", token, "Malfunction");
        const response = await fetch(request);
        
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        if (response.status === 204) {
            return [];
        }

        const data = await response.json();

        return data.map((ticketData: any) => {
            return Malfunction.fromJson(ticketData);
        });
    }

    async getById(token: string, malfunctionId: number): Promise<Malfunction | undefined> {
        const request = this.createRequest("GET", token, `Malfunction/${malfunctionId}`);
        const response = await fetch(request);

        if (response.status === 204 || response.status === 400) {
            return undefined;
        }
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        return Malfunction.fromJson(data);
    }

    async create(token: string, model: CreateMalfunction): Promise<Malfunction | undefined> {
        const request = this.createRequest("POST", token, "Malfunction", model.toJSON()); 
        const response = await fetch(request);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        
        const data = await response.json();

        return Malfunction.fromJson(data);
    }

    async update(token: string, model: Malfunction): Promise<Malfunction | undefined> {
        const request = this.createRequest("PUT", token, "Malfunction", model.toJSON());
        const response = await fetch(request);

        if (!response.ok) {
            if (response.status === 400) {
                const data = await response.json();
                throw new Error(data);
            }
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        return Malfunction.fromJson(data);
    }

    async delete(token: string, malfunctionId: number): Promise<void> {
        const request = this.createRequest("DELETE", token, `Malfunction/${malfunctionId}`);
        const response = await fetch(request);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
    }
}