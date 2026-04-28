import { TestBed } from '@angular/core/testing';

import { Like } from './like.service';

describe('Like', () => {
  let service: Like;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Like);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
