import { Component } from '@angular/core';
import { DynamicScriptLoaderService } from '../_services/dynamic-script-loader.service';
import { BaseComponent } from '../base/base.component';
import { MsalService } from '../_services/msal.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
export class HomeComponent extends BaseComponent {
    constructor(
        private msalService: MsalService,
        protected dynamicScriptLoaderService: DynamicScriptLoaderService) {
        super(dynamicScriptLoaderService);
    }

    login(): void {
        this.msalService.login();
    }
}
