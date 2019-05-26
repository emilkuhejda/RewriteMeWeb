import { Component } from '@angular/core';
import { MsalService } from './_services/msal.service';
import { CommonVariables } from './_config/common-variables';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    title = 'WebClient';

    constructor(private msalService: MsalService) { }

    navigateToProfile(): void {
        if (!this.msalService.isLoggedIn()) {
            location.reload(true);
        }

        window.open(CommonVariables.ApiUrl + CommonVariables.Profile, '_blank');
    }

    login(): void {
        this.msalService.login();
    }

    logout(): void {
        this.msalService.logout();
    }

    isUserLoggedIn(): boolean {
        return this.msalService.isLoggedIn();
    }
}
