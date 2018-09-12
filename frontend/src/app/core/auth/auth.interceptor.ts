import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

export class AuthInterceptor implements HttpInterceptor {

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token: string = localStorage.getItem('token');


    if (token) {
      request = request.clone({ headers: request.headers.set('Authorization', 'Bearer ' + token) });
    }
    request = request.clone({headers: request.headers.set('Content-Type', 'application/json')});

    request = request.clone({ headers: request.headers.set('Accept', 'application/json') });
    return next.handle(request);

  }
}
