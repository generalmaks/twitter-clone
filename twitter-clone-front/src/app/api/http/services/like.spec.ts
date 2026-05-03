import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';

import { LikesService } from './like.service';

describe('LikesService', () => {
  let service: LikesService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient()],
    });
    service = TestBed.inject(LikesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
