import { TestBed } from '@angular/core/testing';

import { InformationMessageService } from './information-message.service';

describe('InformationMessageService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: InformationMessageService = TestBed.get(InformationMessageService);
    expect(service).toBeTruthy();
  });
});
