import { Injectable } from '@angular/core';
import { RoutingService } from './routing.service';
import { Observable } from 'rxjs';
import { CacheItem } from '../_models/cache-item';
import { HttpClient } from '@angular/common/http';

@Injectable({
	providedIn: 'root'
})
export class CacheService {
	constructor(
		private routingService: RoutingService,
		private http: HttpClient) { }

	getCacheItem(fileItemId: string): Observable<CacheItem> {
		return this.http.get<CacheItem>(this.routingService.getCacheItemUri() + fileItemId);
	}
}
