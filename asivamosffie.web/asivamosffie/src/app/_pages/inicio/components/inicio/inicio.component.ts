import { Component, OnInit } from '@angular/core';
import { Validators, FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { RecoverPasswordComponent } from '../recover-password/recover-password.component';
import { Router, Routes } from '@angular/router';
@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.component.html',
  styleUrls: ['./inicio.component.scss']
})
export class InicioComponent implements OnInit {

  formLogin: FormGroup;

  verClave = true;

  modalTitle: string;
  modalText: string;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    public dialog: MatDialog
  ) {
    this.buildForm();
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

  login(event: Event) {
    event.preventDefault();
    if (this.formLogin.valid) {
      console.log(this.formLogin.value);
      // this.router.navigate(['/home']); // esto es para que lo redireccione despues de crear el login
    }
  }

  private buildForm() {
    this.formLogin = this.formBuilder.group({
      emailField: ['', [
        Validators.required,
        Validators.maxLength(50),
        Validators.minLength(4),
        Validators.email,
        Validators.pattern(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/)
      ]],
      passwordField: ['', [Validators.required]],
    });
  }

}
