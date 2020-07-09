import { Component } from '@angular/core';
import { FormBuilder, Validators, FormArray, ControlValueAccessor, FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-registrar',
  templateUrl: './registrar.component.html',
  styleUrls: ['./registrar.component.scss']
})
export class RegistrarComponent 
{
  addressForm: FormGroup;

  nombresAportantes: any;
  fuentesDeRecursosLista: any;
  bancos: any;
  VigenciasAporte: any;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {

    this.nombresAportantes = [ {name: 'FFIE', value: '1'}, {name: 'FFIE', value: '2'}, {name: 'FFIE', value: '3'}, {name: 'FFIE', value: '4'} ];
    this.fuentesDeRecursosLista = [ {name: 'Recursos propios', value: '1'}, {name: 'Recursos propios', value: '2'}, {name: 'Recursos propios', value: '3'}, {name: 'Recursos propios', value: '4'} ];
    this.bancos = [ {name: 'banco 1', value: '1'}, {name: 'banco 2', value: '2'}, {name: 'banco 3', value: '3'}, {name: 'banco 4', value: '4'} ];
    this.VigenciasAporte = [ 2015, 2016, 2017, 2019, 2020, 2021, 2022 ];


    this.addressForm = this.fb.group({
      nombreAportante: [null, Validators.required],
      documentoApropiacion: [null, Validators.required],
      numerodocumento: [null, Validators.compose([
        Validators.required, Validators.minLength(10), Validators.maxLength(10)])
      ],
      fuenteRecursosArray: this.fb.array([
        this.fb.group({
          fuenteRecursos: [null, Validators.required],
          valorFuenteRecursos: [null, Validators.compose([
            Validators.required, Validators.minLength(2), Validators.maxLength(2)])
          ],
          cuantasVigencias: [null, Validators.required],
          vigencias: this.fb.array([
            this.fb.group({
              vigenciaAportante: [null, Validators.required],
              valorVigencia: [null, Validators.compose([
                Validators.required, Validators.minLength(10), Validators.maxLength(10)])
              ]
              })
          ]),
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
        })]),
    });
    console.log(this.addressForm);
  }

  get vigencias1() {    
    let control=this.addressForm.get('fuenteRecursosArray') as FormArray;    
    return control.controls[0].get('vigencias') as FormArray;
  }
  get fuenteRecursosArray() {
    return this.addressForm.get('fuenteRecursosArray') as FormArray;
  }
  get cuentasBancaria() {
    let control=this.addressForm.get('fuenteRecursosArray') as FormArray;
    return control.controls[0].get('cuentasBancaria') as FormArray;
  }

  CambioNumerovigencia() {
    const FormNumvigencias = this.addressForm.value;
    if (FormNumvigencias.cuantasVigencias > this.vigencias1.length && FormNumvigencias.cuantasVigencias < 100) {
      while (this.vigencias1.length < FormNumvigencias.cuantasVigencias) {
        this.vigencias1.push(this.createVigencia());
      }
    } else if (FormNumvigencias.cuantasVigencias <= this.vigencias1.length && FormNumvigencias.cuantasVigencias >= 0) {
      while (this.vigencias1.length > FormNumvigencias.cuantasVigencias) {
        this.borrarArray(this.vigencias1, this.vigencias1.length - 1);
      }
    }
  }

  agregarCuentaBancaria() {
    this.cuentasBancaria.push(this.createCuentaBancaria());
  }

  agregaFuente() {
    this.fuenteRecursosArray.push(this.crearFuente());
  }

  createCuentaBancaria(): FormGroup {
    return this.fb.group({
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
      });
  }

  crearFuente():FormGroup{
    return this.fb.group({
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
    })
  }

  createVigencia(): FormGroup {
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
