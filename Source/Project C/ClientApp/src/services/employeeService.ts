import { CreateEmployee } from "../models/CreateEmployee";
import { Employee } from "../models/Employee";

export class employeeService {
    constructor(private baseUrl: string = "https://localhost:7004") { }

    async getAll(token: string): Promise<Employee[] | undefined> {
        const request = this.createRequest("GET", token, "Employee");
        const response = await fetch(request);
        
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        if (response.status === 204) {
            return [];
        }

        const data = await response.json();

        return data.map((ticketData: any) => {
            return Employee.fromJson(ticketData);
        });
    }

    async getById(token: string, ticketId: number): Promise<Employee | undefined> {
        const request = this.createRequest("GET", token, `Employee/${ticketId}`);
        const response = await fetch(request);

        if (response.status === 204 || response.status === 400) {
            return undefined;
        }
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        return Employee.fromJson(data);
    }

    async create(token: string, model: CreateEmployee): Promise<Employee | undefined> {
        const request = this.createRequest("POST", token, "Employee", model.toJSON()); 
        const response = await fetch(request);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        
        const data = await response.json();

        return Employee.fromJson(data);
    }

    async update(token: string, model: Employee): Promise<Employee | undefined> {
        const request = this.createRequest("PUT", token, "Employee", model.toJSON());
        const response = await fetch(request);

        if (!response.ok) {
            if (response.status === 400) {
                const data = await response.json();
                throw new Error(data);
            }
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        return Employee.fromJson(data);
    }

    async delete(token: string, ticketId: number): Promise<void> {
        const request = this.createRequest("DELETE", token, `Employee/${ticketId}`);
        const response = await fetch(request);

        if (!response.ok) {
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