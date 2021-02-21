import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { CreateFileComponent } from '../files/create-file/create-file.component';

@Injectable({
    providedIn: 'root'
})
export class DeactivateCreateFileGuard implements CanDeactivate<CreateFileComponent> {
    canDeactivate(component: CreateFileComponent, currentRoute: ActivatedRouteSnapshot, currentState: RouterStateSnapshot, nextState?: RouterStateSnapshot): boolean {
        if (component.submitted && component.loading) {
            if (confirm("Upload is in progress. If you leave, your changes will be lost."))
                return true;

            return false;
        }

        return true;
    }
}
