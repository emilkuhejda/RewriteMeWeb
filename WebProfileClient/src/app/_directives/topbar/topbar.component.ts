import { Component, OnInit } from '@angular/core';
import { MsalService } from 'src/app/_services/msal.service';

@Component({
    selector: 'app-topbar',
    templateUrl: './topbar.component.html',
    styleUrls: ['./topbar.component.css']
})
export class TopbarComponent implements OnInit {
    userName: string;

    constructor(private msalService: MsalService) { }

    ngOnInit() {
        this.userName = this.msalService.getUserName();
    }
}
