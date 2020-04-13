import { TestBed } from '@angular/core/testing';

import { UserReactionsService } from './user-reactions.service';

describe('UserReactionsService', () => {
  let service: UserReactionsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserReactionsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
