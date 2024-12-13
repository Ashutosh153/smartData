import { TestBed } from '@angular/core/testing';

import { DetailsOfUsersService } from './details-of-users.service';

describe('DetailsOfUsersService', () => {
  let service: DetailsOfUsersService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DetailsOfUsersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
