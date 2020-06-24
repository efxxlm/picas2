import { Component, OnInit } from '@angular/core';
import { Validators, FormGroup, FormBuilder, ValidationErrors, ValidatorFn } from '@angular/forms';

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
  ) {
    this.buildForm();
  }

  ngOnInit(): void {
  }

  cambiarContrasena(event: Event) {
    event.preventDefault();
    if (this.formChangePassword.valid) {
      console.log(this.formChangePassword.value);
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
