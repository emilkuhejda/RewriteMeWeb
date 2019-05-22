import { Component, OnInit } from '@angular/core';
import { MsalService } from '../_services/msal.service';
import { Route, Router } from '@angular/router';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
    constructor(
        private msalService: MsalService,
        private router: Router) { }

    ngOnInit() {
        if(this.msalService.isLoggedIn()) {
            this.router.navigate(['/']);
        }
    }

    login() {
        this.msalService.login();
    }
}
