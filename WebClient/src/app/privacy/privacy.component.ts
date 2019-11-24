import { Component } from '@angular/core';
import { DynamicScriptLoaderService } from '../_services/dynamic-script-loader.service';
import { BaseComponent } from '../base/base.component';

@Component({
    selector: 'app-privacy',
    templateUrl: './privacy.component.html',
    styleUrls: ['./privacy.component.css']
})
export class PrivacyComponent extends BaseComponent {
    constructor(protected dynamicScriptLoaderService: DynamicScriptLoaderService) {
        super(dynamicScriptLoaderService);
    }

    ngOnInit() {
        this.loadScripts();

        window.scrollTo(0, 0);
    }
}
