import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonVariables } from '../_config/common-variables';

@Injectable({
	providedIn: 'root'
})
export class UserService {
	constructor(private http: HttpClient) { }

	register(userData) {
		return this.http.post(CommonVariables.ApiUrl + CommonVariables.ApiRegisterPath, userData);
	}
}
