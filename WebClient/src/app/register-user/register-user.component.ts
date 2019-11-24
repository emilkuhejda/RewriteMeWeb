import { Component } from '@angular/core';
import { MsalService } from '../_services/msal.service';
import { UserService } from '../_services/user.service';
import { CommonVariables } from '../_config/common-variables';
import { Router } from '@angular/router';
import { RoutingService } from '../_services/routing.service';
import { UserRegistration } from '../_models/user-registration';
import { DynamicScriptLoaderService } from '../_services/dynamic-script-loader.service';
import { BaseComponent } from '../base/base.component';

@Component({
    selector: 'app-register-user',
    templateUrl: './register-user.component.html',
    styleUrls: ['./register-user.component.css']
})
export class RegisterUserComponent extends BaseComponent {
    constructor(
        private router: Router,
        private routingService: RoutingService,
        private userService: UserService,
        private msalService: MsalService,
        protected dynamicScriptLoaderService: DynamicScriptLoaderService) {
        super(dynamicScriptLoaderService);
    }

    ngOnInit() {
        let isLoggedIn = this.msalService.isLoggedIn();
        if (!isLoggedIn) {
            this.msalService.loginOrRedirect();
        } else {
            this.register();
        }

        this.loadScripts();
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
