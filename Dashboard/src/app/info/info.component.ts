import { DashboardDataService } from './../services/dashboard-data.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-info',
  templateUrl: './info.component.html',
  styleUrls: ['./info.component.scss']
})
export class InfoComponent implements OnInit {

  nowPlaying: any;
  volume: number;

  constructor(public dashboardDataService : DashboardDataService) { }

  ngOnInit() {
    this.getData();
  }

  private getData(){
    this.dashboardDataService.getData()
    .subscribe( result => {
      this.nowPlaying = this.convertToJSON(result)._body.nowPlaying;
      this.volume = this.convertToJSON(result)._body.volume
      console.log("FROM INFO COMPONENT",this.nowPlaying);
    });
  }

  private convertToJSON(data): any {
    return JSON.parse(JSON.stringify(data));
  }

}
