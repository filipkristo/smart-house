import {Injectable, Injector} from '@angular/core';
import {async, fakeAsync, tick} from '@angular/core/testing';
import {BaseRequestOptions, ConnectionBackend, Http, RequestOptions} from '@angular/http';
import {Response, ResponseOptions} from '@angular/http';
import {MockBackend, MockConnection} from '@angular/http/testing';

//move this part into separate file later
const data = {
  nowPlaying: {
    artist: 'Eagles',
    song: 'Witchy Woman',
    album: 'Eagles (Remastered)',
    albumUri: 'http://cont-2.p-cdn.com/images/public/rovi/albumart/9/2/3/2/075596062329_500W_500H.jpg',
    genre: 'Santana Radio',
    loved: true,
    playedSeconds: 0,
    durationSeconds: 252
  },
  currentInput: 'Pandora',
  telemetryData: {
    temperature: 22.8,
    humidity: 68.8,
    heatIndex: 22.93,
    gasValue: 308,
    measured: '2017-11-06T21:44:22.001387Z'
  },
  isTurnOn: true,
  volume: -245
}

@Injectable()
export class DashboardDataService {

  constructor( private http: Http ) { }

  public getData() {
    //change api later
    return this.http.get('myservices.de/api/heroes')
    .toPromise()
    .then(response => response.json().data)
    .catch(e => this.handleError(e));
  }

  private handleError(error: any): Promise<any> {
    console.error('An error occurred', error);
    return Promise.reject(error.message || error);
  }

}

describe('MockBackend DashboardDataService Example', () => {
  beforeEach(() => {
    this.injector = Injector.create([
      {provide: ConnectionBackend, useClass: MockBackend},
      {provide: RequestOptions, useClass: BaseRequestOptions},
      Http,
      DashboardDataService,
    ]);
    this.heroService = this.injector.get(DashboardDataService);
    this.backend = this.injector.get(ConnectionBackend) as MockBackend;
    this.backend.connections.subscribe((connection: any) => this.lastConnection = connection);
  });

  it('getData() should query current service url', () => {
    this.heroService.getHeroes();
    expect(this.lastConnection).toBeDefined('no http service connection at all?');
    expect(this.lastConnection.request.url).toMatch(/api\/heroes$/, 'url invalid');
  });

  it('getData() should return some data', fakeAsync(() => {
       let result: String[];
       this.heroService.getHeroes().then((heroes: String[]) => result = heroes);
       this.lastConnection.mockRespond(new Response(new ResponseOptions({
         body: JSON.stringify({data: this.data}),
       })));
       tick();
       expect(result).toEqual(this.data, 'this is data');
     }));

  it('getData() while server is down', fakeAsync(() => {
       let result: object;
       let catchedError: any;
       this.heroService.getHeroes()
           .then((data: object) => result = data)
           .catch((error: any) => catchedError = error);
       this.lastConnection.mockRespond(new Response(new ResponseOptions({
         status: 404,
         statusText: 'URL not Found',
       })));
       tick();
       expect(result).toBeUndefined();
       expect(catchedError).toBeDefined();
     }));
});
