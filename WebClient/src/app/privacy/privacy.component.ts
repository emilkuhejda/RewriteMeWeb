import { Component, OnInit, OnDestroy } from '@angular/core';
import { DynamicScriptLoaderService } from '../_services/dynamic-script-loader.service';

@Component({
    selector: 'app-privacy',
    templateUrl: './privacy.component.html',
    styleUrls: ['./privacy.component.css']
})
export class PrivacyComponent implements OnInit, OnDestroy {
    private scriptKey: string = "script";

    constructor(private dynamicScriptLoaderService: DynamicScriptLoaderService) { }

    ngOnInit() {
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
}
