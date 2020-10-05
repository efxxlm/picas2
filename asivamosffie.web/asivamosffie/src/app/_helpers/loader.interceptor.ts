
import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

//import { LoaderDialogComponent } from '../components/loader-dialog/loader-dialog.component';
import { finalize } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { SpinnerLoadingComponent } from '../shared/components/spinner-loading/spinner-loading.component';

@Injectable()
export class LoaderInterceptor implements HttpInterceptor {
  constructor(
    //private store: Store,
    private dialog: MatDialog
  ) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const dialogRef = this.dialog.open(SpinnerLoadingComponent);
    return next.handle(req).pipe(
      finalize(() => dialogRef.close())
    );
  }
}