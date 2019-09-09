import { TestBed } from '@angular/core/testing';

import { FileItemService } from './file-item.service';

describe('FileItemService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: FileItemService = TestBed.get(FileItemService);
    expect(service).toBeTruthy();
  });
});
