import { CreateEmployee } from "../models/CreateEmployee";
import { Employee } from "../models/Employee";
import { baseService } from "./baseService";

export class employeeService extends baseService{
    constructor(baseUrl: string = "http://api.platiumx.com") { super(baseUrl); }

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
}