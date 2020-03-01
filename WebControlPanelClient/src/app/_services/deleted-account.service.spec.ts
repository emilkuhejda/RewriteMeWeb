import { TestBed } from '@angular/core/testing';

import { DeletedAccountService } from './deleted-account.service';

describe('DeletedAccountService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DeletedAccountService = TestBed.get(DeletedAccountService);
    expect(service).toBeTruthy();
  });
});
