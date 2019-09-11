import { Component, OnInit } from '@angular/core';
import { AlertService } from '../_services/alert.service';
import { ErrorResponse } from '../_models/error-response';
import { RoutingService } from '../_services/routing.service';
import { UtilsService } from '../_services/utils.service';
import { AuthenticationService } from '../_services/authentication.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
    isDeploymentSuccessful: boolean;

    constructor(
        private utilsService: UtilsService,
        private routingService: RoutingService,
        private authenticationService: AuthenticationService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.utilsService.hasAccess().subscribe(
            access => {
                if (!access) {
                    this.authenticationService.logout();
                    location.reload(true);
                }
            }
        );

        this.utilsService.isDeploymentSuccessful().subscribe(
            () => {
                this.isDeploymentSuccessful = true;
            },
            () => {
                this.isDeploymentSuccessful = false;
            });
    }

    generateHangfireAccess() {
        this.alertService.clear();

        this.utilsService.generateHangfireAccess().subscribe(
            () => {
                this.alertService.success("Access token was generated");
            },
            (error: ErrorResponse) => {
                this.alertService.error(error.message);
            });
    }

    navigateToHangfire() {
        window.open(this.routingService.getHangfireUri(), '_blank');
    }
}
