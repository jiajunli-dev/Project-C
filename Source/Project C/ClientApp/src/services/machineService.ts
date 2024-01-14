import { CreateMachine } from "../models/CreateMachine";
import { Machine } from "../models/Machine";
import { baseService } from "./baseService";

export class machineService extends baseService{
    constructor(baseUrl: string = "https://localhost:7004") { super(baseUrl); }

    async getAll(token: string): Promise<Machine[] | undefined> {
        const request = this.createRequest("GET", token, "Machine");
        const response = await fetch(request);
        
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        if (response.status === 204) {
            return [];
        }

        const data = await response.json();

        return data.map((ticketData: any) => {
            return Machine.fromJson(ticketData);
        });
    }

    async getById(token: string, machineId: number): Promise<Machine | undefined> {
        const request = this.createRequest("GET", token, `Machine/${machineId}`);
        const response = await fetch(request);

        if (response.status === 204 || response.status === 400) {
            return undefined;
        }
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        return Machine.fromJson(data);
    }

    async create(token: string, model: CreateMachine): Promise<Machine | undefined> {
        const request = this.createRequest("POST", token, "Machine", model.toJSON()); 
        const response = await fetch(request);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        
        const data = await response.json();

        return Machine.fromJson(data);
    }

    async update(token: string, model: Machine): Promise<Machine | undefined> {
        const request = this.createRequest("PUT", token, "Machine", model.toJSON());
        const response = await fetch(request);

        if (!response.ok) {
            if (response.status === 400) {
                const data = await response.json();
                throw new Error(data);
            }
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        return Machine.fromJson(data);
    }

    async delete(token: string, machineId: number): Promise<void> {
        const request = this.createRequest("DELETE", token, `Machine/${machineId}`);
        const response = await fetch(request);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
    }
}