import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { MaterialModule } from './../material/material.module';

import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { ModalDialogComponent } from './components/modal-dialog/modal-dialog.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { SpinnerLoadingComponent } from './components/spinner-loading/spinner-loading.component';
import { SubMenuComponent } from './components/sub-menu/sub-menu.component';

@NgModule({
  declarations: [
    HeaderComponent,
    FooterComponent,
    ModalDialogComponent,
    NavbarComponent,
    SpinnerLoadingComponent,
    SubMenuComponent
  ],
  exports: [
    HeaderComponent,
    FooterComponent,
    ModalDialogComponent,
    NavbarComponent,
    SpinnerLoadingComponent,
    SubMenuComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    MaterialModule,
    ReactiveFormsModule,
  ]
})
export class SharedModule { }
