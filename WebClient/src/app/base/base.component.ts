import { Component, OnInit, OnDestroy } from '@angular/core';
import { DynamicScriptLoaderService } from '../_services/dynamic-script-loader.service';

@Component({
    selector: 'app-base',
    template: '',
    styleUrls: ['./base.component.css']
})
export class BaseComponent implements OnInit, OnDestroy {
    protected scriptKey: string = "script";

    constructor(protected dynamicScriptLoaderService: DynamicScriptLoaderService) { }

    ngOnInit() {
        this.loadDefaultScript();
    }

    ngOnDestroy() {
        this.unloadScripts();
    }
    
    protected loadDefaultScript() {
        this.dynamicScriptLoaderService.load(this.scriptKey);
    }

    protected unloadScripts() {
        this.dynamicScriptLoaderService.remove(this.scriptKey);
    }
}
