export class CreateDepartment {
    public createdBy?: string;

    public name?: string;
    public description?: string;

    constructor(data?: Partial<CreateDepartment>) {
        if (data) Object.assign(this, data);
    }

    static fromJson(json: any): CreateDepartment {
        const model = new CreateDepartment();
        Object.assign(model, json);
        return model;
    }

    toJSON(): string {
        const entries = Object.entries(this);
        const filteredEntries = entries.filter(([_, value]) => value !== undefined && value !== null);
        const obj = Object.fromEntries(filteredEntries);

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

            } else if (key === 'name' && value.length > 64) {
                errors.push('Name must be no longer than 64 characters.');

            } else if (key === 'description' && value.length > 2048) {
                errors.push('Description must be no longer than 2048 characters.');
            }
        });

        return errors;
    }
}
