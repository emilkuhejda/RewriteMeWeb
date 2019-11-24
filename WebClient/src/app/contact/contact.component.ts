import { Component, OnInit, OnDestroy } from '@angular/core';
import { DynamicScriptLoaderService } from '../_services/dynamic-script-loader.service';
import { BaseComponent } from '../base/base.component';

@Component({
	selector: 'app-contact',
	templateUrl: './contact.component.html',
	styleUrls: ['./contact.component.css']
})
export class ContactComponent extends BaseComponent {
	constructor(protected dynamicScriptLoaderService: DynamicScriptLoaderService) {
		super(dynamicScriptLoaderService);
	}
}
