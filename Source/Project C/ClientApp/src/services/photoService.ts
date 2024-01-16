import { Photo, CreatePhoto } from "../models/Photo";
import { baseService } from "./baseService";

export class PhotoService extends baseService {
    // TODO: Move api url to config
    constructor(baseUrl: string = "https://localhost:7004") { super(baseUrl); }

    async getById(token: string, photoId: number): Promise<Photo | undefined> {
        if (!token) throw new Error('Token is required.');
        if (!photoId) throw new Error('PhotoId is required.');

        const request = this.createRequest("GET", token, `Photo/${photoId}`);
        const response = await fetch(request);
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

        const data = await response.json();

        return Photo.fromJson(data);
    }

    async create(token: string, model: CreatePhoto): Promise<Photo | undefined> {
        if (!token) throw new Error('Token is required.');
        if (!model) throw new Error('Model is required.');

        const errors = model.validate();
        if (errors.length > 0) throw new Error(errors.join('\n'));

        const request = this.createRequest("POST", token, "Photo", model.toJSON());
        const response = await fetch(request);
        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

        const data = await response.json();

        return Photo.fromJson(data);
    }

    async update(token: string, model: Photo): Promise<Photo | undefined> {
        if (!token) throw new Error('Token is required.');
        if (!model) throw new Error('Model is required.');

        const errors = model.validate();
        if (errors.length > 0) throw new Error(errors.join('\n'));

        const request = this.createRequest("PUT", token, `Photo`, model.toJSON());
        const response = await fetch(request);
        if (!response.ok) {
            if (response.status === 400) {
                const data = await response.json();
                throw new Error(data);
            } else if (response.status === 409) {
                const data = await response.json();
                throw new Error(data);
            }

            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();

        return Photo.fromJson(data);
    }

    async delete(token: string, photoId: number): Promise<void> {
        if (!token) throw new Error('Token is required.');
        if (!photoId) throw new Error('PhotoId is required.');

        const request = this.createRequest("DELETE", token, `Photo/${photoId}`);
        const response = await fetch(request);

        if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);
    }
}
