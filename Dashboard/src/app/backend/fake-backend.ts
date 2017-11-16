import {
  Http, BaseRequestOptions, Response, ResponseOptions,
  RequestMethod, XHRBackend, RequestOptions
} from '@angular/http';
import { MockBackend, MockConnection } from '@angular/http/testing';

import { fakeData } from './../models/data';

function fakeBackendFactory(backend: MockBackend,
  options: BaseRequestOptions,
  realBackend: XHRBackend) {

  let data: object = fakeData();

  // configure fake backend
  backend.connections.subscribe((connection: MockConnection) => {
    // wrap in timeout to simulate server api call
    setTimeout(() => {

      // TODO: Request-URL mapping to mock data
      if (connection.request.url.endsWith('/fake-backend/data') &&
        connection.request.method === RequestMethod.Get) {
        connection.mockRespond(new Response(new ResponseOptions({
          status: 200,
          body: data
        })));

        // pass through any requests not handled above
        let realHttp = new Http(realBackend, options);
        let requestOptions = new RequestOptions({
          method: connection.request.method,
          headers: connection.request.headers,
          body: connection.request.getBody(),
          url: connection.request.url,
          withCredentials: connection.request.withCredentials,
          responseType: connection.request.responseType
        });
        realHttp.request(connection.request.url, requestOptions)
          .subscribe((response: Response) => {
            connection.mockRespond(response);
          },
          (error: any) => {
            connection.mockError(error);
          });

        return;
      }

    }, 500);

  });

  return new Http(backend, options);
}

export let fakeBackendProvider = {
  // use fake backend in place of Http service
  provide: Http,
  useFactory: fakeBackendFactory,
  deps: [MockBackend, BaseRequestOptions, XHRBackend]
};
