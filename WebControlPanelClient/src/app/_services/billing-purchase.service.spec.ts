import { TestBed } from '@angular/core/testing';

import { BillingPurchaseService } from './billing-purchase.service';

describe('BillingPurchaseService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BillingPurchaseService = TestBed.get(BillingPurchaseService);
    expect(service).toBeTruthy();
  });
});
