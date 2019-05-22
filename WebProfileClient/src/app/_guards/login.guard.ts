import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate, Router, CanLoad } from '@angular/router';
import { Observable } from 'rxjs';
import { MsalService } from '../_services/msal.service';

@Injectable({
    providedIn: 'root'
})
export class LoginGuard implements CanActivate {
    constructor(
        private router: Router,
        private msalService: MsalService) { }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        if (!this.msalService.isLoggedIn()) {
            return true;
        }

        this.router.navigate(['/']);
        return false;
    }
}
