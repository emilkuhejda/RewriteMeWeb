import { TestBed } from '@angular/core/testing';

import { TranscribeItemService } from './transcribe-item.service';

describe('TranscribeItemService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TranscribeItemService = TestBed.get(TranscribeItemService);
    expect(service).toBeTruthy();
  });
});
