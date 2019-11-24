import { Component } from '@angular/core';
import { DynamicScriptLoaderService } from '../_services/dynamic-script-loader.service';
import { BaseComponent } from '../base/base.component';

@Component({
    selector: 'app-pricing',
    templateUrl: './pricing.component.html',
    styleUrls: ['./pricing.component.css']
})
export class PricingComponent extends BaseComponent {
    constructor(protected dynamicScriptLoaderService: DynamicScriptLoaderService) {
        super(dynamicScriptLoaderService);
    }
}
