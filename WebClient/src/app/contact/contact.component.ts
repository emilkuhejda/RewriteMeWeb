import { Component } from '@angular/core';
import { DynamicScriptLoaderService } from '../_services/dynamic-script-loader.service';
import { BaseComponent } from '../base/base.component';

@Component({
	selector: 'app-contact',
	templateUrl: './contact.component.html',
	styleUrls: ['./contact.component.css']
})
export class ContactComponent extends BaseComponent {
	protected mapScriptKey: string = "map";

	constructor(protected dynamicScriptLoaderService: DynamicScriptLoaderService) {
		super(dynamicScriptLoaderService);
	}
}
