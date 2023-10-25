export class Customer {
    public id?: string;
    public createdAt?: Date;
    public createdBy?: string;
    public updatedAt?: Date;
    public updatedBy?: string;

    public username?: string;
    public firstName?: string;
    public lastName?: string;
    public email?: string;
    public phoneNumber?: string;

    public companyName?: string;
    public companyPhoneNumber?: string;
    public departmentName?: string;

    static fromJson(json: any): Customer {
        const model = new Customer();
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

            } else if (['id', 'createdBy', 'updatedBy'].includes(key) && value.length > 40) {
                errors.push(`${key} must be no longer than 40 characters.`);

            } else if (key === 'username' && value.length > 32) {
                errors.push('Username must be no longer than 32 characters.');
            } else if (['phoneNumber', 'companyPhoneNumber'].includes(key) && value.length > 16) {
                errors.push(`${key} must be no longer than 16 characters.`);
            } else if (['firstName', 'lastName', 'email'].includes(key) && value.length > 254) {
                errors.push(`${key} must be no longer than 254 characters.`);
            } else if (['companyName', 'departmentName'].includes(key) && value.length > 64) {
                errors.push(`${key} must be no longer than 64 characters.`);
            }
        });

        return errors;
    }
}
