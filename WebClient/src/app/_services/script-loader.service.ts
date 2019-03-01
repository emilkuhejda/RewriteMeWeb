import { Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root'
})
export class ScriptLoaderService {
	private scripts: { [key: string]: string; } = {};

	constructor() { }

	public loadScript(url: string, id: string) {
		if (this.scripts.hasOwnProperty(id))
			return;

		new Promise((resolve) => {
			this.loadScriptElement(url, id);

			this.scripts[id] = url;
		});
	}

	private loadScriptElement(url: string, id: string) {
		let node = document.createElement('script');
		node.src = url;
		node.type = 'text/javascript';
		node.async = true;
		node.charset = 'utf-8';
		node.id = id;
		document.getElementsByTagName('head')[0].appendChild(node);
	}

	public removeScriptElement(id: string) {
		if (!this.scripts.hasOwnProperty(id))
			return;

		let scriptElement = document.getElementById(id);
		if (!scriptElement)
			return;

		scriptElement.parentNode.removeChild(scriptElement);
		delete this.scripts[id];
	}
}
