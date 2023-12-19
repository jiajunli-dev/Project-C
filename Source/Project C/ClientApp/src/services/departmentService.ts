import { CreateDepartment } from "../models/CreateDepartment";
import { Department } from "../models/Department";
import { baseService } from "./baseService";

export class departmentService extends baseService {
    constructor(baseUrl: string = "https://localhost:7004") { super(baseUrl); }

    async getAll(token: string): Promise<Department[] | undefined> {
        const request = this.createRequest("GET", token, "Department");
        const response = await fetch(request);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        if (response.status === 204) {
            return [];
        }

        const data = await response.json();

        return data.map((ticketData: any) => {
            return Department.fromJson(ticketData);
        });
    }

    async getById(token: string, departmentId: number): Promise<Department | undefined> {
        const request = this.createRequest("GET", token, `Department/${departmentId}`);
        const response = await fetch(request);

        if (response.status === 204 || response.status === 400) {
            return undefined;
        }
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        return Department.fromJson(data);
    }

    async create(token: string, model: CreateDepartment): Promise<Department | undefined> {
        const request = this.createRequest("POST", token, "Department", model.toJSON());
        const response = await fetch(request);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        return Department.fromJson(data);
    }

    async update(token: string, model: Department): Promise<Department | undefined> {
        const request = this.createRequest("PUT", token, "Department", model.toJSON());
        const response = await fetch(request);

        if (!response.ok) {
            if (response.status === 400) {
                const data = await response.json();
                throw new Error(data);
            }
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        return Department.fromJson(data);
    }

    async delete(token: string, departmentId: number): Promise<void> {
        const request = this.createRequest("DELETE", token, `Department/${departmentId}`);
        const response = await fetch(request);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
    }
}