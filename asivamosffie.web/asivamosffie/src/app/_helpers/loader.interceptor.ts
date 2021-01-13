
import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

//import { LoaderDialogComponent } from '../components/loader-dialog/loader-dialog.component';
import { finalize } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { SpinnerLoadingComponent } from '../shared/components/spinner-loading/spinner-loading.component';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable()
export class LoaderInterceptor implements HttpInterceptor {
  
    peticiones = 0;

    constructor( private spinner: NgxSpinnerService ) {
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        this.spinner.show();
        this.peticiones++;
        return next.handle(req)
            .pipe(
                finalize( () => {
                    this.peticiones--;
                    if ( this.peticiones === 0 ) {
                        this.spinner.hide();
                    }
                } )
            );
    }

}