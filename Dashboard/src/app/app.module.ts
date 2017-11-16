import { fakeBackendProvider } from './backend/fake-backend';
import { Http, BaseRequestOptions, XHRBackend, BrowserXhr, ResponseOptions } from '@angular/http';

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule }   from '@angular/forms';
import { HttpModule } from  '@angular/http';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { InfoComponent } from './info/info.component';
import { ControlsComponent } from './controls/controls.component';

import { DashboardDataService } from './services/dashboard-data.service';
import { MockBackend } from '@angular/http/testing';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    SidebarComponent,
    InfoComponent,
    ControlsComponent
  ],
  imports: [
    BrowserModule,
    HttpModule
  ],
  providers: [
    DashboardDataService,
    MockBackend,
    BaseRequestOptions,
    fakeBackendProvider,
    XHRBackend,
    BrowserXhr
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
