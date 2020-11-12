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
    mensajeenviado: boolean=false;
    constructor(private authenticationService: AutenticacionService,public dialog: MatDialog,) { }
    private isTokenRefreshing: boolean = false;
    tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>(null);
    
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(this.attachTokenToRequest(request)).pipe(
            tap((event : HttpEvent<any>) => {
                if(event instanceof HttpResponse) 
                {
                   // console.log("Success");
                }
            }),
            catchError((err) : Observable<any> => {
                if(err instanceof HttpErrorResponse) {
                    switch((<HttpErrorResponse>err).status) 
                {
                        case 401:
                            console.log("Token expired. Attempting refresh ...");
                            //revisar esto de abajo, no debería ir aqui
                            if(!this.mensajeenviado)
                            {
                                this.openDialog("Error","Su sesión ha expirado.");
                                this.mensajeenviado=true;
                            }
                            
                            return this.handleHttpResponseError(request, next);
                        case 400:
                            return this.handleError(err);
                    }
                } else 
                {
                    return throwError(err);
                }
            })

           );
        
            // return this.authenticationService.getNewRefreshToken().pipe(
            //     tap((tokenresponse: any) => {
            //         if(tokenresponse) 
            //         {
            //             this.tokenSubject.next(tokenresponse.authToken.token); 
            //             // localStorage.setItem('loginStatus', '1');
            //             // localStorage.setItem('jwt', tokenresponse.authToken.token);
            //             // localStorage.setItem('username', tokenresponse.authToken.username);
            //             // localStorage.setItem('expiration', tokenresponse.authToken.expiration);
            //             // localStorage.setItem('userRole', tokenresponse.authToken.roles);
            //             // localStorage.setItem('refreshToken', tokenresponse.authToken.refresh_token);
            //             console.log("Token refreshed...");
            //             return next.handle(this.attachTokenToRequest(request));

            //         }
            //         return <any>this.authenticationService.logout();
            //     }),
            //     catchError((err) : Observable<any> => {
            //         if(err instanceof HttpErrorResponse) {
            //             switch((<HttpErrorResponse>err).status) 
            //         {
            //                 case 401:
            //                     console.log("Token expired. Attempting refresh ...");
            //                   //  return this.handleHttpResponseError(request, next);
            //                 case 400:
            //                     return <any>this.authenticationService.logout();
            //             }
            //         } else 
            //         {
                       
            //             return throwError(err); //handleerror
            //         } this.isTokenRefreshing = true;
            //     }) 
            
            // });
    }

     // Method to handle http error response
     private handleHttpResponseError(request : HttpRequest<any>, next : HttpHandler) 
     {
 
         // First thing to check if the token is in process of refreshing
         if(!this.isTokenRefreshing)  // If the Token Refresheing is not true
         {
             this.isTokenRefreshing = true;
 
             // Any existing value is set to null
             // Reset here so that the following requests wait until the token comes back from the refresh token API call
             this.tokenSubject.next(null);
            
             /// call the API to refresh the token
             // try {
                  return this.authenticationService.getNewRefreshToken().pipe(
                 switchMap((tokenresponse: any) => {
                     if(tokenresponse.currentUser) 
                     {
                          this.tokenSubject.next(tokenresponse.currentUser.token); 
                          //localStorage.setItem('currentUser', tokenresponse.currentUser);
                         console.log("Token refreshed...");
                         return next.handle(this.attachTokenToRequest(request));
 
                 }
                     return <any>this.authenticationService.logout();
                 }),
                  catchError(err => {
                //     //  this.authenticationService.logout();
                      return this.handleError(err);
                //     // return err;
                 }),
                 finalize(() => {
                   this.isTokenRefreshing = false;
                 })
                 );
 
            //  } catch (error) {
            //     return this.handleError(error);
            //  }
            
         }
         else 
         {
             this.isTokenRefreshing = false;
             if(this.authenticationService.currentUserValue){
             return this.tokenSubject.pipe(filter(token => token != null),
                 take(1),
                 switchMap(token => {
                 return next.handle(this.attachTokenToRequest(request));
                 }));
                }else{
                    return this.handleError(new HttpErrorResponse({error:"Authentication Fail", status: 401}));
                }
         }
 
 
     }

     openDialog(modalTitle: string, modalText: string) {
      let dialogRef= this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle, modalText }
      });
      dialogRef.afterClosed().subscribe(result => {
       this.authenticationService.logout();
      });
    }
  // Global error handler method 
  public handleError(errorResponse : HttpErrorResponse) 
  {
      let errorMsg : string;

      if (errorResponse.error instanceof Error) {
          // A client-side or network error occurred. Handle it accordingly.
          errorMsg = "An error occured : " + errorResponse.error.message;
      } else {
          // The backend returned an unsuccessful response code.
          // The response body may contain clues as to what went wrong,
          if ([401, 403].indexOf(errorResponse.status) !== -1) {
              //     // auto logout if 401 Unauthorized or 403 Forbidden response returned from api
                if(!this.mensajeenviado)
                {
                    this.openDialog("Error","Su sesión ha expirado.");
                    this.mensajeenviado=true;
                }
              
              this.authenticationService.logout(true);
              //location.reload(true);
          } else {
              // console.log(err);
              errorMsg = `Backend returned code ${errorResponse.status}, body was: ${errorResponse.error}`;
             console.log(errorMsg);
          }
      }
      return throwError(new Error(errorMsg));


    //   case 404:
    //   {
    //       Swal.fire({
    //           title: "",
    //           text: `Correo o contraseña errada`,
    //           showCancelButton: false,
    //           confirmButtonText: `X`,
    //           buttonsStyling: false,
    //           customClass: {
    //               container: 'container-class',
    //               popup: 'popup-class',
    //               header: 'header-class',
    //               title: 'title-class text-white',
    //               closeButton: 'close-button-class',
    //               icon: 'icon-class',
    //               image: 'image-class',
    //               content: 'content-class order',
    //               input: 'input-class',
    //               actions: 'actions-class mt-0',
    //               confirmButton: 'btn-confirm-class cerrar',
    //               cancelButton: 'cancel-button-class',
    //               footer: 'footer-class'
    //           }
    //           });
    //       break;
    //   }
    //   default:{
    //       this.notifier.notify(err.error, 2);
    //   }

       return throwError(errorMsg);
  }

    private attachTokenToRequest(request: HttpRequest<any>) 
    {
        // add authorization header with jwt token if available
        let currentUser = this.authenticationService.currentUserValue;
        if (currentUser && currentUser.token) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${currentUser.token.value}`
                }
            });
        }

        return next.handle(request);
  }
}
