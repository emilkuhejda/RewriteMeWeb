import { HttpErrorResponse } from '@angular/common/http';
import { ErrorCode } from '../_enums/error-code';

export class ErrorResponse {
    private _message: string;
    private _response: HttpErrorResponse
    private _errorCode: ErrorCode = ErrorCode.None;

    constructor(response: HttpErrorResponse) {
        this._message = "Error occurred, please try again later";
        this._response = response;

        if (typeof response.error === "string") {
            let errorCode = ErrorCode[response.error];
            this._errorCode = errorCode === undefined ? ErrorCode.None : errorCode;
        }
    }

    get status(): number {
        return this._response.status;
    }

    get message(): string {
        return this._message;
    }

    get errorCode(): ErrorCode {
        return this._errorCode;
    }
}
