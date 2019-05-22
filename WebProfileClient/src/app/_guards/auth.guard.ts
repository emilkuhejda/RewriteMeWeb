import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CanActivate } from '@angular/router/src/utils/preactivation';
import { MsalService } from '../_services/msal.service';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {
    path: ActivatedRouteSnapshot[];
    route: ActivatedRouteSnapshot;
    
    constructor(
        private router: Router,
        private msalService: MsalService) { }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
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
