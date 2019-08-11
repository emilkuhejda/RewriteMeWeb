import { Component, OnInit } from '@angular/core';
import { UtilService } from '../_services/util.service';
import { AlertService } from '../_services/alert.service';
import { ErrorResponse } from '../_models/error-response';
import { RoutingService } from '../_services/routing.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
    constructor(
        private utilService: UtilService,
        private routingService: RoutingService,
        private alertService: AlertService) { }

    ngOnInit() { }

    generateHangfireAccess() {
        this.alertService.clear();

        this.utilService.generateHangfireAccess().subscribe(
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
