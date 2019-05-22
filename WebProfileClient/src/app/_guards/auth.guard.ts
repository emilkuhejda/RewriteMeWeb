import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { MsalService } from '../_services/msal.service';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private msalService: MsalService) { }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        if (this.msalService.isLoggedIn()) {
            return true;
        }

        let queryParams = {
            queryParams: { returnUrl: state.url }
        };
        this.router.navigate(['/login'], queryParams);
        return false;
    }
}
