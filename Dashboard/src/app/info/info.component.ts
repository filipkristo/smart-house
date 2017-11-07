import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-info',
  templateUrl: './info.component.html',
  styleUrls: ['./info.component.scss']
})
export class InfoComponent implements OnInit {

  artist = "The Smiths";
  album = "The Sound Of The Smiths (Deluxe Edition)";
  volume = -25;
  song = "There Is A Light That Never Goes Out";
  radio = "The Smiths Radio";
  loved = true;

  constructor() { }

  ngOnInit() {
  }

}
