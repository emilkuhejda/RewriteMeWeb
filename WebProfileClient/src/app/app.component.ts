import { Component, OnInit } from '@angular/core';
import { MsalService } from './_services/msal.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    constructor(private msalService: MsalService) { }

    ngOnInit(): void {
    }

    logout(): void {
        this.msalService.logout();
    }
}
