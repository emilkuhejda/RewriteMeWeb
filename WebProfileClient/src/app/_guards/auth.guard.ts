import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate } from '@angular/router';
import { Observable } from 'rxjs';
import { MsalService } from '../_services/msal.service';
import { RoutingService } from '../_services/routing.service';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {
    constructor(
        private routingService: RoutingService,
        private msalService: MsalService) { }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        if (this.msalService.isLoggedIn()) {
            return true;
        }

        window.open(this.routingService.getHomeUri(), '_self');
        return false;
    }
}
