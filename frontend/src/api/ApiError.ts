import type { ApiProblemDetails } from "../types";

export class ApiError extends Error {
    public readonly status: number;

    public readonly details?: ApiProblemDetails;

    public constructor(message: string, status: number, details?: ApiProblemDetails) {
        super(message);

        this.name = 'ApiError';
        this.status = status;
        this.details = details;
    }
}