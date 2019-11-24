import { Component, OnInit, OnDestroy } from '@angular/core';
import { DynamicScriptLoaderService } from '../_services/dynamic-script-loader.service';

@Component({
    selector: 'app-base',
    template: '',
    styleUrls: ['./base.component.css']
})
export class BaseComponent implements OnInit, OnDestroy {
    private scriptKey: string = "script";

    constructor(protected dynamicScriptLoaderService: DynamicScriptLoaderService) { }

    ngOnInit() {
        this.loadScripts();
    }

    ngOnDestroy() {
        this.unloadScripts();
    }

    protected loadScripts() {
        this.dynamicScriptLoaderService.load(this.scriptKey);
    }

    protected unloadScripts() {
        this.dynamicScriptLoaderService.remove(this.scriptKey);
    }
}
