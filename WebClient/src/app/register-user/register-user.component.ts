import { Component, OnInit, OnDestroy } from '@angular/core';
import { MsalService } from '../_services/msal.service';
import { UserService } from '../_services/user.service';
import { CommonVariables } from '../_config/common-variables';
import { Router } from '@angular/router';
import { RoutingService } from '../_services/routing.service';
import { UserRegistration } from '../_models/user-registration';
import { DynamicScriptLoaderService } from '../_services/dynamic-script-loader.service';

@Component({
    selector: 'app-register-user',
    templateUrl: './register-user.component.html',
    styleUrls: ['./register-user.component.css']
})
export class RegisterUserComponent implements OnInit, OnDestroy {
    private scriptKey: string = "script";

    constructor(
        private router: Router,
        private routingService: RoutingService,
        private userService: UserService,
        private msalService: MsalService,
        private dynamicScriptLoaderService: DynamicScriptLoaderService) { }

    ngOnInit() {
        let isLoggedIn = this.msalService.isLoggedIn();
        if (!isLoggedIn) {
            this.msalService.loginOrRedirect();
        } else {
            this.register();
        }

        this.loadScripts();
    }

    ngOnDestroy(): void {
        this.unloadScripts();
    }

    private loadScripts() {
        this.dynamicScriptLoaderService.load(this.scriptKey);
    }

    private unloadScripts() {
        this.dynamicScriptLoaderService.remove(this.scriptKey);
    }

    private register(): void {
        if (!this.msalService.isLoggedIn())
            return;

        let userData = {
            id: this.msalService.getUserId(),
            ApplicationId: CommonVariables.ApplicationId,
            Email: this.msalService.getUserEmail(),
            GivenName: this.msalService.getGivenName(),
            FamilyName: this.msalService.getFamilyName()
        };

        this.userService.register(userData).subscribe(
            (data: UserRegistration) => {
                this.msalService.saveToken(data.token);
                this.msalService.saveCurrentIdentity(JSON.stringify(data.identity));
                document.location.href = this.routingService.getProfileUri();
            },
            () => {
                this.router.navigate(['/']);
            }
        );
    }
}
