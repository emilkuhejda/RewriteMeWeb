import { TestBed } from '@angular/core/testing';

import { UserSubscriptionService } from './user-subscription.service';

describe('UserSubscriptionService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UserSubscriptionService = TestBed.get(UserSubscriptionService);
    expect(service).toBeTruthy();
  });
});
