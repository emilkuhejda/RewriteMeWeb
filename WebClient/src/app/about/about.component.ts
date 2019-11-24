import { Component } from '@angular/core';
import { BaseComponent } from '../base/base.component';
import { DynamicScriptLoaderService } from '../_services/dynamic-script-loader.service';

@Component({
    selector: 'app-about',
    templateUrl: './about.component.html',
    styleUrls: ['./about.component.css']
})
export class AboutComponent extends BaseComponent {
    constructor(protected dynamicScriptLoaderService: DynamicScriptLoaderService) {
        super(dynamicScriptLoaderService);
    }
}
