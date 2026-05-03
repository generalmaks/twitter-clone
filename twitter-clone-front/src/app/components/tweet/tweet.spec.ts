import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';

import { Tweet } from './tweet';
import { LikesService } from '../../api/http/services/like.service';
import { UsersService } from '../../api/http/services/user.service';

describe('Tweet', () => {
  let component: Tweet;
  let fixture: ComponentFixture<Tweet>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Tweet],
      providers: [
        {
          provide: UsersService,
          useValue: {
            getById: () => of({
              id: 1,
              username: 'test',
              displayUsername: 'Test User',
              bio: null,
              createdAt: '2026-05-03T00:00:00Z',
            }),
          },
        },
        {
          provide: LikesService,
          useValue: {
            getPostLikesCount: () => of(0),
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(Tweet);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('post', {
      id: 1,
      authorId: 1,
      textContent: 'Test tweet',
      createdAt: '2026-05-03T00:00:00Z',
    });
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
