import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';

import { MainFeed } from './main-feed';
import { PostsService } from '../../api/http/services/post.service';

describe('MainFeed', () => {
  let component: MainFeed;
  let fixture: ComponentFixture<MainFeed>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MainFeed],
      providers: [
        {
          provide: PostsService,
          useValue: {
            getAll: () => of([]),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(MainFeed);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
