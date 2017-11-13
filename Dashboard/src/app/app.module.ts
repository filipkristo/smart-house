import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { InfoComponent } from './info/info.component';
import { ControlsComponent } from './controls/controls.component';

import { DashboardDataService } from './services/dashboard-data.service';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    SidebarComponent,
    InfoComponent,
    ControlsComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [DashboardDataService],
  bootstrap: [AppComponent]
})
export class AppModule { }
