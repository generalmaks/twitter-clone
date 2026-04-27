import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Tweet } from './tweet';

describe('Tweet', () => {
  let component: Tweet;
  let fixture: ComponentFixture<Tweet>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Tweet],
    }).compileComponents();

    fixture = TestBed.createComponent(Tweet);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
