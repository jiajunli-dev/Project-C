import { Priority } from './Priority';
import { Status } from './Status';

export class CreateTicket {
    public createdBy?: string;

    public description?: string;
    public triedSolutions?: string[];
    public additionalNotes?: string;
    public priority?: Priority;
    public status?: Status;

    constructor(data?: Partial<CreateTicket>) {
        if (data) Object.assign(this, data);
    }

    static fromJson(json: any): CreateTicket {
        if (typeof json.status === 'number') {
            json.status = Status[json.status];
        }
        if (typeof json.priority === 'number') {
            json.priority = Priority[json.priority];
        }
        if (typeof json.triedSolutions === 'string') {
            json.triedSolutions = json.triedSolutions.split(',');
        }

        const model = new CreateTicket();
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
        if (typeof obj.triedSolutions !== 'string') {
            obj.triedSolutions = obj.triedSolutions.join(',');
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

            } else if (['description', 'triedSolutions', 'additionalNotes'].includes(key) && value.length > 2048) {
                errors.push(`${key} must be no longer than 2048 characters.`);
            } else if (key === 'triedSolutions' && value.join(',').length > 2048) {
                errors.push(`${key} must be no longer than 2048 items.`);
            }
        });

        return errors;
    }
}
