import { Component } from '@angular/core';
import { BaseComponent } from '../base/base.component';
import { DynamicScriptLoaderService } from '../_services/dynamic-script-loader.service';

@Component({
    selector: 'app-how-to',
    templateUrl: './how-to.component.html',
    styleUrls: ['./how-to.component.css']
})
export class HowToComponent extends BaseComponent {
    constructor(protected dynamicScriptLoaderService: DynamicScriptLoaderService) {
        super(dynamicScriptLoaderService);
    }
}
