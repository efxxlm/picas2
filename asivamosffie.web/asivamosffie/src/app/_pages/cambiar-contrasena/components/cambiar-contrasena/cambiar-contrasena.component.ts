import { Component, OnInit } from '@angular/core';
import { Validators, FormGroup, FormBuilder, ValidationErrors, ValidatorFn } from '@angular/forms';
import { Usuario, AutenticacionService, Respuesta } from 'src/app/core/_services/autenticacion/autenticacion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { first } from 'rxjs/operators';
import  sha1 from 'sha1';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';


export const validateConfirmPassword: ValidatorFn = (
  control: FormGroup
): ValidationErrors | null => {
  const password = control.get('newPassword');
  const confirmarPassword = control.get('confirmPassword');

  return password.value === confirmarPassword.value
    ? null
    : { noSonIguales: true };
};

@Component({
  selector: 'app-cambiar-contrasena',
  templateUrl: './cambiar-contrasena.component.html',
  styleUrls: ['./cambiar-contrasena.component.scss']
})
export class CambiarContrasenaComponent implements OnInit {

  verAyuda = false;

  verClaveActual = true;
  verClaveNueva = true;
  verClaveConfirmar = true;

  formChangePassword: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private autenticacionService: AutenticacionService,
    private router: Router,
    public dialog: MatDialog
  ) {
    this.buildForm();
  }

  ngOnInit(): void {
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  cambiarContrasena(event: Event) {
    event.preventDefault();
    if (this.formChangePassword.valid) {
      this.autenticacionService.changePass(sha1(this.formChangePassword.value.currentPassword),sha1(this.formChangePassword.value.newPassword)).pipe(first()).subscribe( respuesta => {
        console.log(respuesta);
        console.log(respuesta.data);
        
        this.openDialog('Validación cambio de contraseña', respuesta.message);
        if(respuesta.code=="200")
        {
            this.router.navigate(['/home']);
        }
       },
       err => {
          let mensaje: string;
          //console.log(err)
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
  }

  private buildForm() {
    this.formChangePassword = this.formBuilder.group(
      {
        currentPassword: ['', [Validators.required]],
        newPassword: ['', [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(15),
          Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])([A-Za-z\d$@$!%*?&]|[^ ]){8,15}$/)
        ]],
        confirmPassword: ['', [Validators.required]],
      },
      {
        validators: validateConfirmPassword,
      }
    );
  }

  validarSiSonIguales(): boolean  {
    return this.formChangePassword.hasError('noSonIguales')  &&
      this.formChangePassword.get('newPassword').dirty &&
      this.formChangePassword.get('confirmPassword').dirty;
  }

}
