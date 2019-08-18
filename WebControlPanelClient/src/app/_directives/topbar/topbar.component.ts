import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/_services/authentication.service';

@Component({
    selector: 'app-topbar',
    templateUrl: './topbar.component.html',
    styleUrls: ['./topbar.component.css']
})
export class TopbarComponent implements OnInit {
    userName: string;
    userId: string;

    constructor(private authenticationService: AuthenticationService) { }

    ngOnInit() {
        let identity = this.authenticationService.getIdentity();
        this.userName = `${identity.firstName} ${identity.lastName}`;
        this.userId = identity.id;
    }
}
