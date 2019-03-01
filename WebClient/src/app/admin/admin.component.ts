import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../_services/authentication.service';
import { ScriptLoaderService } from '../_services/script-loader.service';

@Component({
    selector: 'app-admin',
    templateUrl: './admin.component.html',
    styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
    private readonly scriptKey = "admin-script"
    private readonly scriptUrl = "/assets/js/sb-admin.min.js";

    constructor(
        private router: Router,
        private authenticationService: AuthenticationService,
        private scriptLoaderService: ScriptLoaderService) { }

    ngOnInit() {
        this.scriptLoaderService.loadScript(this.scriptUrl, this.scriptKey);
    }

    logout() {
        this.authenticationService.logout();
        this.router.navigate(['/login']);
    }
}
