import { Priority } from './Priority';
import { Status } from './Status';

export class CreateMalfunction {
    public createdBy?: string;

    public priority?: Priority;
    public status?: Status;
    public description?: string;
    public solution?: string;

    public ticketId?: number;

    static fromJson(json: any): CreateMalfunction {
        if (typeof json.status === 'number') {
            json.status = Status[json.status];
        }
        if (typeof json.priority === 'number') {
            json.priority = Priority[json.priority];
        }

        const model = new CreateMalfunction();
        Object.assign(model, json);

        return model;
    }

    toJSON(): string {
        const entries = Object.entries(this);
        const filteredEntries = entries.filter(([_, value]) => value !== undefined && value !== null);
        const obj = Object.fromEntries(filteredEntries);

        if (typeof obj.status !== 'number') {
            obj.status = Status[obj.status];
        }
        if (typeof obj.priority !== 'number') {
            obj.priority = Status[obj.priority];
        }

        return JSON.stringify(obj);
    }

    validate(): string[] {
        const errors: string[] = [];

        Object.entries(this).forEach(([key, value]) => {
            if (value === undefined || value === null) {
                errors.push(`${key} is required.`);
            } else if (typeof value === 'string' && value.trim() === '') {
                errors.push(`${key} is required.`);

            } else if (key === 'createdBy' && value.length > 40) {
                errors.push('Created by must be no longer than 40 characters.');

            } else if (['description', 'solution'].includes(key) && value.length > 2048) {
                errors.push(`${key} must be no longer than 2048 characters.`);
            }
        });

        return errors;
    }
}
