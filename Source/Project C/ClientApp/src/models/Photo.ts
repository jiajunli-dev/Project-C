export class Photo {
    public id?: number;
    public createdAt?: Date;
    public createdBy?: string;
    public updatedAt?: Date;
    public updatedBy?: string;

    public name?: string;
    public data?: string;

    public ticketId?: number;

    static fromJson(json: any): Photo {
        if (typeof json.createdAt === 'string') {
            json.createdAt = new Date(json.createdAt);
        }
        if (typeof json.updatedAt === 'string') {
            json.updatedAt = new Date(json.updatedAt);
        }
        const model = new Photo();
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

            } else if (key === 'name' && value.length > 256) {
                errors.push('File Name must be no longer than 256 characters.');
            } else if (key === 'data' && value.length > 20 * 1024 * 1024) {
                errors.push('File Size must be 20 MB or less.');
            }
        });

        return errors;
    }
}

export class CreatePhoto {
    public createdBy?: string;

    public name?: string;
    public data?: string;

    public ticketId?: number;

    static fromJson(json: any): CreatePhoto {
        const model = new CreatePhoto();
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
                errors.push(`${key} must be no longer than 40 characters.`);

            } else if (key === 'name' && value.length > 256) {
                errors.push('File Name by must be no longer than 256 characters.');
            } else if (key === 'data' && value.length > 20 * 1024 * 1024) {
                errors.push('File Size must be 20 MB or less.');
            }
        });

        return errors;
    }
}
