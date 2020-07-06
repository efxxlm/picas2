import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpClient
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AutenticacionService } from '../core/_services/autenticacion/autenticacion.service';

@Injectable()
export class JwtInterceptorInterceptor implements HttpInterceptor {

  constructor(private authenticationService: AutenticacionService, private http: HttpClient) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // add authorization header with jwt token if available
        
        let currentUser = this.authenticationService.actualUser;
        this.authenticationService.actualUser$.subscribe(user => {
            currentUser = user;
          });
        if (currentUser) {
            // //console.log("token de usuario");
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${currentUser.token.value}`
                }
            });
        }

        return next.handle(request);
  }
}
