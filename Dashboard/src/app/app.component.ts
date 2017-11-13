import { DashboardDataService } from './services/dashboard-data.service';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  constructor(
    private dashboardDataService : DashboardDataService) {
      console.log('FROM APP.COMPONENTS', dashboardDataService.getData());
    }

}
