import { TestBed } from '@angular/core/testing';

import { CachService } from './cach.service';

describe('CachService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CachService = TestBed.get(CachService);
    expect(service).toBeTruthy();
  });
});
