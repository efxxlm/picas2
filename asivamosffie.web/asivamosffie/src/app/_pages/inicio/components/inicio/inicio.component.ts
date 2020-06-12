import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { RecoverPasswordComponent } from '../recover-password/recover-password.component';
@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.component.html',
  styleUrls: ['./inicio.component.scss']
})
export class InicioComponent implements OnInit {

  emailField: FormControl;
  verClave = true;

  modalTitle: string;
  modalText: string;

  constructor(public dialog: MatDialog) {
    this.emailField =  new FormControl('', [
      Validators.required,
      Validators.maxLength(50),
      Validators.minLength(4),
      Validators.email,
      Validators.pattern(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/)
    ]);

  }

  openDialog() {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: {
        modalTitle: `El usuario se encuentra inactivo`,
        modalText: `Contacte al administrador`
      }
    });
  }

  openRecoverPass() {
    this.dialog.open(RecoverPasswordComponent);
  }

  ngOnInit(): void {

  }

}
