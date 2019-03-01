import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CommonVariables } from '../_config/common-variables';

@Injectable({
	providedIn: 'root'
})
export class AuthGuard implements CanActivate {
	constructor(private router: Router) { }

	canActivate(
		route: ActivatedRouteSnapshot,
		state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
		if (localStorage.getItem(CommonVariables.CurrentUser)) {
			return true;
		}

		this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
		return false;
	}
}
