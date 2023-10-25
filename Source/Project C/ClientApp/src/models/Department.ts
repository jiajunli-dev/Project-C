export class Department {
    public id?: number;
    public createdAt?: Date;
    public createdBy?: string;
    public updatedAt?: Date;
    public updatedBy?: string;

    public name?: string;
    public description?: string;

    static fromJson(json: any): Department {
        if (typeof json.createdAt === 'string') {
            json.createdAt = new Date(json.createdAt);
        }
        if (typeof json.updatedAt === 'string') {
            json.updatedAt = new Date(json.updatedAt);
        }

        const model = new Department();
        Object.assign(model, json);
        return model;
    }

    toJSON(): string {
        const entries = Object.entries(this);
        const filteredEntries = entries.filter(([_, value]) => value !== undefined && value !== null);
        const obj = Object.fromEntries(filteredEntries);
        if (obj.createdAt instanceof Date) {
            obj.createdAt = obj.createdAt.toISOString();
        }
        if (obj.updatedAt instanceof Date) {
            obj.updatedAt = obj.updatedAt.toISOString();
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

            } else if (['createdBy', 'updatedBy'].includes(key) && value.length > 40) {
                errors.push(`${key} must be no longer than 40 characters.`);

            } else if (key === 'name' && value.length > 64) {
                errors.push('Name must be no longer than 64 characters.');

            } else if (key === 'description' && value.length > 2048) {
                errors.push('Description must be no longer than 2048 characters.');
            }
        });

        return errors;
    }
}
