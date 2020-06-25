import { Component, OnInit } from '@angular/core';
import { Validators, FormGroup, FormBuilder, FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AutenticacionService, Usuario, Respuesta } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-recover-password',
  templateUrl: './recover-password.component.html',
  styleUrls: ['./recover-password.component.scss']
})

export class RecoverPasswordComponent implements OnInit {

  formRecoverPass: FormGroup;

  constructor(
    private formBuilderRecoverPass: FormBuilder,
    public dialog: MatDialog,
    private autenticacionService: AutenticacionService,
    private router: Router
  ) {
    this.builderRecoverPass();
  }

  ngOnInit(): void {
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  RecoverPassword(event: Event) {
    event.preventDefault();
    if (this.formRecoverPass.valid) {
      const usuario: Usuario = {
        Email: this.formRecoverPass.value['emailRecoverField']
      }
      //console.log(this.formRecoverPass.value['emailRecoverField']);
      this.autenticacionService.RecuperarContrasena(usuario)
          .subscribe( respuesta => {
            this.verificarRespuesta( respuesta );
          }, 
          err => {
            let mensaje: string;
            console.log(err);
            if (err.message){
              mensaje = err.message;
            }
            else if (err.error.message){
              mensaje = err.error.message;
            }
            this.openDialog('Error', mensaje);
         },
         () => {
          //console.log('terminó');
         });
    }
  }

  private verificarRespuesta( respuesta: Respuesta )
  {
    console.log(respuesta);
    if (respuesta.isSuccessful) // Response witout errors
    {
        if (respuesta.code === '101') // Expected response 
        {
          this.openDialog('Validación Inicio Sesión', respuesta.message);
          this.router.navigate(['/inicio']);
        }
        else
        {
          this.openDialog('Validación Inicio Sesión', respuesta.message);
        }
    }else 
    {
      this.openDialog('Validación Recuperar Contraseña', respuesta.message);
    }
    
  }

  private builderRecoverPass() {
    this.formRecoverPass = this.formBuilderRecoverPass.group({
      emailRecoverField: ['', [
          Validators.required,
          Validators.maxLength(50),
          Validators.minLength(4),
          Validators.email,
          Validators.pattern(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/)
        ]]
    });
  }



}
