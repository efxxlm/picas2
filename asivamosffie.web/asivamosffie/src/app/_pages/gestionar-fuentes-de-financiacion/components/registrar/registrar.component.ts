import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, ControlValueAccessor, FormGroup, FormControl } from '@angular/forms';
import { CommonService, Dominio, Localizacion, TiposAportante } from 'src/app/core/_services/common/common.service';
import { CofinanciacionService } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';
import { forkJoin } from 'rxjs';
import { ActivatedRoute } from '@angular/router'

@Component({
  selector: 'app-registrar',
  templateUrl: './registrar.component.html',
  styleUrls: ['./registrar.component.scss']
})
export class RegistrarComponent implements OnInit {
  addressForm: FormGroup;

  maxDate: Date;
  nombresAportantes: Dominio[] = [];
  fuentesDeRecursosLista: Dominio[] = [];
  bancos: Dominio[] = [];
  VigenciasAporte = [];
  departamentos: Localizacion[] = []
  municipios: Localizacion[] = []
  tipoAportanteId: number = 0;
  tipoAportante = TiposAportante;
  
  
  constructor(private fb: FormBuilder,
              private commonService: CommonService,
              private cofinanciacionService: CofinanciacionService,
              private activatedRoute: ActivatedRoute
              ) 
              { 
                this.maxDate = new Date();
              }

  ngOnInit(): void {

    this.VigenciasAporte = this.commonService.vigenciasDesde2015();
    this.activatedRoute.params.subscribe( param =>  {
      this.tipoAportanteId = param['idTipoAportante'];
      console.log(this.tipoAportanteId, param);

      forkJoin([
        this.commonService.listaNombreAportante(),
        this.commonService.listaFuenteRecursos(),
        this.commonService.listaBancos(),
        this.commonService.listaDepartamentos(),
        this.cofinanciacionService.listaAportantesByTipoAportante(this.tipoAportanteId)
      ]).subscribe( res => {
  
        if (this.tipoAportante.Tercero.includes(this.tipoAportanteId.toString())){
          let nombresAportantesTemp: Dominio[] = res[0];  

          let listaTemp: Dominio[] = res[4];

          listaTemp.forEach( apo => {
            let s = nombresAportantesTemp.find( temp => temp.dominioId == apo.dominioId);
            if(s){
              console.log(s, listaTemp, apo.dominioId);
              this.nombresAportantes.push(s);    
            }
          });

        }else{
          this.nombresAportantes = res[0];
        }
        
  
         this.fuentesDeRecursosLista = res[1];
         this.bancos = res[2];
         this.departamentos = res[3];
      });
    })

    

    this.addressForm = this.fb.group({
      nombreAportante: [null, Validators.required],
      documentoApropiacion: [null, Validators.required],
      numerodocumento: [null, Validators.compose([
        Validators.required, Validators.minLength(10), Validators.maxLength(10)])
      ],
      vigenciaAcuerdo:[],
      departamento: [],
      municipio: [],
      tieneRP: [null, Validators.required],
      cuantosRP: [null,Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(50)])]
      ,registrosPresupuestales: this.fb.array([
        // this.fb.group({
        //   numeroRP:[null],
        //   fecha: [null]
        // })
      ])
      ,fuenteRecursosArray: this.fb.array([
        this.fb.group({
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
        })]),
    });
    console.log(this.addressForm);
  }

  CambioNumeroRP(){
    const FormNumRP = this.addressForm.get('cuantosRP').value;
    console.log(FormNumRP);
     if (FormNumRP > this.registrosPresupuestales.length && FormNumRP < 100) {
       while (this.registrosPresupuestales.length < FormNumRP) {
        this.registrosPresupuestales.push(this.fb.group({ 
            numeroRP:[null],
            fecha: [null]
        }))
       }
     } else if (FormNumRP <= this.registrosPresupuestales.length && FormNumRP >= 0) {
       while (this.registrosPresupuestales.length > FormNumRP) {
         this.borrarArray(this.registrosPresupuestales, this.registrosPresupuestales.length - 1);
       }
     }
  }

  changeDepartamento(){
    let idDepartamento = this.addressForm.get('departamento').value.localizacionId
    this.commonService.listaMunicipiosByIdDepartamento(idDepartamento).subscribe( mun => {
      this.municipios = mun;
    });
  }
  
  changeNombreAportante(){
    this.cofinanciacionService.getDocumentoApropiacionByAportante(45).subscribe( listDoc => {
      this.addressForm.get('documentoApropiacion').setValue(listDoc[0].numeroActa);
    });
  }

  get fuenteRecursosArray() {
    return this.addressForm.get('fuenteRecursosArray') as FormArray;
  }
  get vigencias1() {
    const control = this.addressForm.get('fuenteRecursosArray') as FormArray;
    return control.controls[0].get('vigencias') as FormArray;
  }
  get cuentasBancaria() {
    const control = this.addressForm.get('fuenteRecursosArray') as FormArray;
    return control.controls[0].get('cuentasBancaria') as FormArray;
  }

  get registrosPresupuestales(){
    return this.addressForm.get('registrosPresupuestales') as FormArray;
  }


  CambioNumerovigencia(j: number) {
    const FormNumvigencias = this.addressForm.value.fuenteRecursosArray[j];
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

  crearFuente(): FormGroup {
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
    });
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
