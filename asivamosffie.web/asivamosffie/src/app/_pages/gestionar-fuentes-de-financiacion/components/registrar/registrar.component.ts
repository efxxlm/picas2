import { Component } from '@angular/core';
import { FormBuilder, Validators, FormArray, ControlValueAccessor, FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-registrar',
  templateUrl: './registrar.component.html',
  styleUrls: ['./registrar.component.scss']
})
export class RegistrarComponent {
  addressForm = this.fb.group({
    nombreAportante: [null, Validators.required],
    documentoApropiacion: [null, Validators.required],
    numerodocumento: [null, Validators.compose([
      Validators.required, Validators.minLength(10), Validators.maxLength(10)])
    ],
    fuenteRecursos: [null, Validators.required],
    valorFuenteRecursos: [null, Validators.compose([
      Validators.required, Validators.minLength(2), Validators.maxLength(2)])
    ],
    cuantasVigencias: [null, Validators.required],
    vigencias: this.fb.array([]),
    cuentasBancaria: this.fb.array([
      this.fb.group({
        numeroCuenta: [null, Validators.compose([
          Validators.required, Validators.minLength(5), Validators.maxLength(50)])
        ],
        nombreCuenta: [null, Validators.compose([
          Validators.required, Validators.minLength(5), Validators.maxLength(100)])
        ],
        codigoSIFI: [null, Validators.compose([
          Validators.required, Validators.minLength(6), Validators.maxLength(6)])
        ],
        tipoCuenta: ['free', Validators.required],
        banco: [null, Validators.required],
        extra: ['free', Validators.required]
        })
    ]),
  });

  nombresAportantes = [ {name: 'FFIE', value: '1'}, {name: 'FFIE', value: '2'}, {name: 'FFIE', value: '3'}, {name: 'FFIE', value: '4'} ];
  fuentesDeRecursos = [ {name: 'Recursos propios', value: '1'}, {name: 'Recursos propios', value: '2'}, {name: 'Recursos propios', value: '3'}, {name: 'Recursos propios', value: '4'} ];
  bancos = [ {name: 'banco 1', value: '1'}, {name: 'banco 2', value: '2'}, {name: 'banco 3', value: '3'}, {name: 'banco 4', value: '4'} ];
  VigenciasAporte = [ 2015, 2016, 2017, 2019, 2020, 2021, 2022 ];

  constructor(private fb: FormBuilder) { }

  get vigencias() {
    return this.addressForm.get('vigencias') as FormArray;
  }
  get cuentasBancaria() {
    return this.addressForm.get('cuentasBancaria') as FormArray;
  }

  CambioNumerovigencia() {
    const FormNumvigencias = this.addressForm.value;
    if (FormNumvigencias.cuantasVigencias > this.vigencias.length && FormNumvigencias.cuantasVigencias < 100) {
      while (this.vigencias.length < FormNumvigencias.cuantasVigencias) {
        this.vigencias.push(this.createvigencia());
      }
    } else if (FormNumvigencias.cuantasVigencias <= this.vigencias.length && FormNumvigencias.cuantasVigencias >= 0) {
      while (this.vigencias.length > FormNumvigencias.cuantasVigencias) {
        this.borrarArray(this.vigencias, this.vigencias.length - 1);
      }
    }
  }

  createvigencia(): FormGroup {
    return this.fb.group({
      vigenciaAportante: [null, Validators.required],
      valorVigencia: [null, Validators.compose([
        Validators.required, Validators.minLength(10), Validators.maxLength(10)])
      ]
      });
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  onSubmit() {
    if (this.addressForm.valid) {
      alert('Thanks!');
    }
  }
}
