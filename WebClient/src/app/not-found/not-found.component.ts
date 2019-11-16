import { Component, OnInit } from '@angular/core';
import { DynamicScriptLoaderService } from '../_services/dynamic-script-loader.service';

@Component({
    selector: 'app-not-found',
    templateUrl: './not-found.component.html',
    styleUrls: ['./not-found.component.css']
})
export class NotFoundComponent implements OnInit {
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
