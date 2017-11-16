import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import {Observable} from 'rxjs/Rx';

@Injectable()
export class DashboardDataService {

  constructor(private http: Http) { }

  public getData(): Observable<object> {
    return this.http.get('/fake-backend/data')
  }

}
