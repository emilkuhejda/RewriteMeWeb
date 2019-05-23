import { Component } from '@angular/core';
import { MsalService } from './_services/msal.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    title = 'WebClient';

    constructor(private msalService: MsalService) { }

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
