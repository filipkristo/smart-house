import { TestBed, inject } from '@angular/core/testing';

import { DashboardDataService } from './dashboard-data.service';

describe('DashboardDataService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DashboardDataService]
    });
  });

  it('should be created', inject([DashboardDataService], (service: DashboardDataService) => {
    expect(service).toBeTruthy();
  }));
});
