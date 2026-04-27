import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainFeed } from './main-feed';

describe('MainFeed', () => {
  let component: MainFeed;
  let fixture: ComponentFixture<MainFeed>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MainFeed],
    }).compileComponents();

    fixture = TestBed.createComponent(MainFeed);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
