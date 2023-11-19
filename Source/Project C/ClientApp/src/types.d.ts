export interface Ticket {
    description: string;
    triedSolutions: string;
    additionalNotes: string;
    status: Status;
}


export enum Status {
    NonCritical = 1,
    Critical = 2,

}

export interface Customer {
    username: string;
    password: string;
    email: string;
    companyName: string;
    companyPhoneNumber: string;
    departmentName: string;
}

export interface Department {
    name: string;
    description: string;
}

export interface Employee {
    username: string;
    password: string;
    email: string;
    departmentId: number;
    department: Department;
}

export interface Malfunction {
    priority: Priority;
    status: Status;
    description: string;
    solution: string;
    ticketId: number;
    ticket: Ticket;
}

export interface Photo {
    name: string;
    data: Uint8Array;
    ticketId: number;
    ticket: Ticket;
}

export interface User {
    username: string;
    phoneNumber: string;
    firstName: string;
    lastName: string;
    email: string;
}

export interface Customer {
    companyName: string;
    companyPhoneNumber: string;
    departmentName: string;
}