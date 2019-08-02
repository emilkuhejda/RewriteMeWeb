import { Component, OnInit } from '@angular/core';
import { MsalService } from '../_services/msal.service';
import { CommonVariables } from '../_config/common-variables';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
    constructor(private msalService: MsalService) { }

    ngOnInit() { }

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
