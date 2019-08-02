import { Component } from '@angular/core';
import { MsalService } from './_services/msal.service';
import { CommonVariables } from './_config/common-variables';
import { UserService } from './_services/user.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    constructor() { }
}
