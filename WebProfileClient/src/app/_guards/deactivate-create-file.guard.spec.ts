import { TestBed, async, inject } from '@angular/core/testing';

import { DeactivateCreateFileGuard } from './deactivate-create-file.guard';

describe('DeactivateCreateFileGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DeactivateCreateFileGuard]
    });
  });

  it('should ...', inject([DeactivateCreateFileGuard], (guard: DeactivateCreateFileGuard) => {
    expect(guard).toBeTruthy();
  }));
});
