import { Component, OnInit } from '@angular/core';
import { MsalService } from '../_services/msal.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
    constructor(
        private msalService: MsalService,
        private router: Router) { }

    ngOnInit(): void {
    }

    login(): void {
        this.msalService.login();
    }
}
