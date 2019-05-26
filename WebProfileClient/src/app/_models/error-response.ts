import { HttpErrorResponse } from '@angular/common/http';

export class ErrorResponse {
    private _message: string;
    private _response: HttpErrorResponse

    constructor(response: HttpErrorResponse) {
        this._message = "Error occurred, please try again later";
        this._response = response;
    }

    get status(): number {
        return this._response.status;
    }

    get message(): string {
        return this._message;
    }
}
