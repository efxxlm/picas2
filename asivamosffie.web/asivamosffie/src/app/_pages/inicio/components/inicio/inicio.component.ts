import { Component, OnInit } from '@angular/core';
import { Validators, FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { RecoverPasswordComponent } from '../recover-password/recover-password.component';
import { Router, Routes } from '@angular/router';
import { Usuario, AutenticacionService, Respuesta } from 'src/app/core/_services/autenticacion/autenticacion.service';
@Component({
  selector: 'app-inicio',
  templateUrl: './inicio.component.html',
  styleUrls: ['./inicio.component.scss']
})
export class InicioComponent implements OnInit {

  formLogin: FormGroup;
  verClave = true;
  dataDialog: {
    modalTitle: string,
    modalText: string
  };


  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    public dialog: MatDialog,
    private autenticacionService: AutenticacionService
  ) {
    this.buildForm();
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
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
      this.IniciarSesion();
    }
  }

  private IniciarSesion()
  {
    
    const usuario: Usuario = {
      Email: this.formLogin.value['emailField'],
      Contrasena: this.formLogin.value['passwordField']
    };

    this.autenticacionService.IniciarSesion(usuario)
        .subscribe( respuesta => {
          this.verificarRespuesta( respuesta );
         },
         err => {
          this.openDialog('Error', err.message);
         },
         () => {
          console.log('terminó');
         });
  }

  private verificarRespuesta( respuesta: Respuesta )
  {
    if (respuesta.codigo === 'OK') // Respuesta sin errores
    {
      if (respuesta.validation === true) // tiene validaciones
      {
        this.openDialog('Validacion Inicio Sesion', respuesta.validationmessage);
      }else // Respuesta esperada
      {
        this.router.navigate(['/home']);
      }
    }else if ( respuesta.codigo === 'PV' ) // inicio sesion primera vez
    {
      this.openDialog('Validacion Inicio Sesion', 'Será direccionado para cambiar su contraseña.');
      this.router.navigate(['/home']);
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
