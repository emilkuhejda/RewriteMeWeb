import { Component } from '@angular/core';
import { HomeComponent } from './home/home.component';
import { RoutingService } from './_services/routing.service';
import { MsalService } from './_services/msal.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    isHomeComponent: boolean;

    constructor(
        private routingService: RoutingService,
        private msalService: MsalService) { }

    onActivate(component) {
        this.isHomeComponent = component instanceof HomeComponent;
    }

    navigateToProfile(): void {
        if (!this.msalService.isLoggedIn()) {
            location.reload(true);
        }

        document.location.href = this.routingService.getProfileUri();
    }

    login(): void {
        if (this.msalService.isLoggedIn()) {
            this.navigateToProfile();
        } else {
            this.msalService.login();
        }
    }

    logout(): void {
        this.msalService.logout();
    }

    isUserLoggedIn(): boolean {
        return this.msalService.isLoggedIn();
    }
}
