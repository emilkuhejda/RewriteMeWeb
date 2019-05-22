import { Component, OnInit } from '@angular/core';
import { MsalService } from './_services/msal.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    title = 'RewriteMe';

    constructor(
        private msalService: MsalService,
        private router: Router) {
    }

    ngOnInit(): void {
        if(!this.isUserLoggedIn()) {
            this.router.navigate(['/login']);
        }
    }

    logout() {
        this.msalService.logout();
    }

    isUserLoggedIn(): boolean {
        return this.msalService.isLoggedIn();
    }
}
