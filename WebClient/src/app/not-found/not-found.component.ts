import { Component } from '@angular/core';
import { DynamicScriptLoaderService } from '../_services/dynamic-script-loader.service';
import { BaseComponent } from '../base/base.component';

@Component({
    selector: 'app-not-found',
    templateUrl: './not-found.component.html',
    styleUrls: ['./not-found.component.css']
})
export class NotFoundComponent extends BaseComponent {
    constructor(protected dynamicScriptLoaderService: DynamicScriptLoaderService) {
        super(dynamicScriptLoaderService);
    }
}
