import { CreateCustomer } from "../models/CreateCustomer";
import { Customer } from "../models/Customer";
import { baseService } from "./baseService";

export class employeeService extends baseService{
    constructor(baseUrl: string = "https://localhost:7004") { super(baseUrl); }

    async getAll(token: string): Promise<Customer[] | undefined> {
        const request = this.createRequest("GET", token, "Customer");
        const response = await fetch(request);
        
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        if (response.status === 204) {
            return [];
        }

        const data = await response.json();

        return data.map((ticketData: any) => {
            return Customer.fromJson(ticketData);
        });
    }

    async getById(token: string, customerId: number): Promise<Customer | undefined> {
        const request = this.createRequest("GET", token, `Customer/${customerId}`);
        const response = await fetch(request);

        if (response.status === 204 || response.status === 400) {
            return undefined;
        }
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        return Customer.fromJson(data);
    }

    async create(token: string, model: CreateCustomer): Promise<Customer | undefined> {
        const request = this.createRequest("POST", token, "Customer", model.toJSON()); 
        const response = await fetch(request);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        
        const data = await response.json();

        return Customer.fromJson(data);
    }

    async update(token: string, model: Customer): Promise<Customer | undefined> {
        const request = this.createRequest("PUT", token, "Customer", model.toJSON());
        const response = await fetch(request);

        if (!response.ok) {
            if (response.status === 400) {
                const data = await response.json();
                throw new Error(data);
            }
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        return Customer.fromJson(data);
    }

    async delete(token: string, customerId: number): Promise<void> {
        const request = this.createRequest("DELETE", token, `Customer/${customerId}`);
        const response = await fetch(request);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
    }
}