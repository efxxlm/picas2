import { Component, OnInit } from '@angular/core';
import { Validators, FormGroup, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { RecoverPasswordComponent } from '../recover-password/recover-password.component';
import { Router, Routes } from '@angular/router';
import { Usuario, AutenticacionService, Respuesta } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { first } from 'rxjs/operators';
import  sha1 from 'sha1';
import { ClassGetter } from '@angular/compiler/src/output/output_ast';
import { CofinanciacionService, Cofinanciacion, CofinanciacionAportante, CofinanciacionDocumento } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';

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
    private autenticacionService: AutenticacionService,
    private p:CofinanciacionService
  ) {
    
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

  openRecoverPass() {
    this.dialog.open(RecoverPasswordComponent);
  }

  ngOnInit(): void {
    this.autenticacionService.logout();
    this.buildForm();

    
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
      Contrasena: sha1(this.formLogin.value['passwordField']).toUpperCase()
    };

    this.autenticacionService.IniciarSesion(usuario).pipe(first()).subscribe( respuesta => {
          this.verificarRespuesta( respuesta );
         },
         err => {
            let mensaje: string;
            if (err.error.message){
              mensaje = err.error.message;
            }else {
              mensaje = err.message;
            }
            this.openDialog('Error', mensaje);
         },
         () => {
          //console.log('terminó');
         });
  }

  private verificarRespuesta( respuesta: Respuesta )
  {
    
    if (respuesta.isSuccessful) // Response witout errors
    {
      if (respuesta.isValidation) // have validations
      {
        if (respuesta.code === '301') // first time, no alert message
        {
          //this.openDialog('Validación Inicio Sesión', respuesta.message);
          localStorage.setItem('codeRecover',btoa(this.formLogin.value['passwordField']));
          this.router.navigate(['/cambiarContrasena']);
        }else
        {
          this.openDialog('', `<b>${respuesta.message}</b>`);
        }
      }else // Expected response 
      {
        this.router.navigate(['/home']);
      }
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
