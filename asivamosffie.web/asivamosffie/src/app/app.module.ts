// Angular
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// others
import { AppRoutingModule } from './app-routing.module';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core/core.module';
import { sha1 } from 'sha1';

// components
import { AppComponent } from './app.component';
import { TestComponent } from './_pages/test/test.component';
import { LayoutComponent } from './layout/layout.component';
import { JwtInterceptorInterceptor } from './_helpers/jwt-interceptor.interceptor';
import { SafePipe } from './_pipes/safe.pipe';
import { MatTableModule } from '@angular/material/table';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';




@NgModule({
  declarations: [
    AppComponent,
    TestComponent,
    LayoutComponent,
    SafePipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    SharedModule,
    CoreModule,
    BrowserAnimationsModule,
    MatTableModule,
    FormsModule
  ],    
  providers: [ { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptorInterceptor, multi: true },DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
