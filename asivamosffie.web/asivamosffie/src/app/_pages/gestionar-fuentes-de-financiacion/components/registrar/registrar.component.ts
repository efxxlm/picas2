import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, ControlValueAccessor, FormGroup, FormControl } from '@angular/forms';
import { CommonService, Dominio, Localizacion, TiposAportante } from 'src/app/core/_services/common/common.service';
import { CofinanciacionService, CofinanciacionAportante } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';
import { forkJoin } from 'rxjs';
import { ActivatedRoute } from '@angular/router'
import { FuenteFinanciacionService, FuenteFinanciacion, CuentaBancaria, RegistroPresupuestal, VigenciaAporte } from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CuentaBancariaService } from 'src/app/core/_services/cuentaBancaria/cuenta-bancaria.service';

@Component({
  selector: 'app-registrar',
  templateUrl: './registrar.component.html',
  styleUrls: ['./registrar.component.scss']
})
export class RegistrarComponent implements OnInit {
  addressForm: FormGroup;

  maxDate: Date;
  nombresAportantes: CofinanciacionAportante[] = [];
  fuentesDeRecursosLista: Dominio[] = [];
  bancos: Dominio[] = [];
  VigenciasAporte = [];
  departamentos: Localizacion[] = []
  municipios: Localizacion[] = []
  tipoAportanteId: number = 0;
  tipoAportante = TiposAportante;
  idAportante: number = 0;
  listaFuentes: FuenteFinanciacion[] = [];

  data;
  
  constructor(private fb: FormBuilder,
              private commonService: CommonService,
              private cofinanciacionService: CofinanciacionService,
              private activatedRoute: ActivatedRoute,
              public dialog: MatDialog,
              private fuenteFinanciacionService: FuenteFinanciacionService
              ) 
              { 
                this.maxDate = new Date();
              }


  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

  EditMode(){

    this.fuenteRecursosArray.clear();

    let aportante = this.nombresAportantes.find( nom => nom.cofinanciacionAportanteId == this.idAportante );
    this.addressForm.get('nombreAportante').setValue(aportante);
    this.changeNombreAportante();

    this.fuenteFinanciacionService.listaFuenteFinanciacionByAportante(this.idAportante).subscribe(lista => {
      lista.forEach( ff => {
      
        let grupo: FormGroup = this.crearFuenteEdit(ff.valorFuente);
        let fuenteRecursosSeleccionada = this.fuentesDeRecursosLista.find( f => f.codigo == ff.fuenteRecursosCodigo );
        let listaVigencias = grupo.get('vigencias') as FormArray;
        let listaCuentas = grupo.get('cuentasBancaria') as FormArray; 
        
        grupo.get('cuantasVigencias').setValue(ff.cantVigencias);
        grupo.get('fuenteRecursos').setValue(fuenteRecursosSeleccionada);
  
        // Vigencias
        ff.vigenciaAporte.forEach( v =>  {
          let grupoVigencia = this.createVigencia();
          let vigenciaSeleccionada = this.VigenciasAporte.find( vtemp => vtemp == v.tipoVigenciaCodigo );

          grupoVigencia.get('vigenciaAportante').setValue(vigenciaSeleccionada);
          grupoVigencia.get('valorVigencia').setValue(v.valorAporte);

          listaVigencias.push(grupoVigencia);
        })
  
        // Cuentas bancarias
        ff.cuentaBancaria.forEach( ba => {
          let grupoCuenta = this.createCuentaBancaria();
          let bancoSeleccionado: Dominio = this.bancos.find( b => b.codigo == ba.bancoCodigo ) ;

          grupoCuenta.get('numeroCuenta').setValue(ba.numeroCuentaBanco);
          grupoCuenta.get('nombreCuenta').setValue(ba.nombreCuentaBanco);
          grupoCuenta.get('codigoSIFI').setValue(ba.codigoSifi);
          grupoCuenta.get('tipoCuenta').setValue(ba.tipoCuentaCodigo);
          grupoCuenta.get('banco').setValue(bancoSeleccionado);
          grupoCuenta.get('extra').setValue(ba.exenta.toString());
        
          listaCuentas.push(grupoCuenta);
          console.log(listaCuentas);
         })
        
        this.fuenteRecursosArray.push(grupo);
      });
    })
  }

  createCuentaBancariaEdit(pNumeroCuenta: string, pNombreCuenta: string, pCodigoSIFI: string, 
                          pTipoCuenta: string, pBanco: Dominio, pExtra: boolean): FormGroup {
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

  crearFuenteEdit(pValorFuenteRecursos: number): FormGroup {
    return this.fb.group({
      fuenteRecursos: [null, Validators.required],
      valorFuenteRecursos: [pValorFuenteRecursos, Validators.compose([
        Validators.required, Validators.minLength(2), Validators.maxLength(2)])
      ],
      cuantasVigencias: [1, Validators.required],
      vigencias: this.fb.array([]),
      fuenteFinanciacionId: [null],
      cuentasBancaria: this.fb.array([
      ]),
    });
  }

  ngOnInit(): void {

    this.VigenciasAporte = this.commonService.vigenciasDesde2015();
    this.activatedRoute.params.subscribe( param =>  {
      this.tipoAportanteId = param['idTipoAportante'];
      this.idAportante = param['idAportante'];

      //console.log(this.tipoAportanteId, param);

      forkJoin([
        this.commonService.listaNombreAportante(),
        this.commonService.listaFuenteRecursos(),
        this.commonService.listaBancos(),
        this.commonService.listaDepartamentos(),
        this.cofinanciacionService.listaAportantesByTipoAportante(this.tipoAportanteId),
        this.fuenteFinanciacionService.listaFuenteFinanciacion()
      ]).subscribe( res => {
  
        this.nombresAportantes = res[4];

        let nombresAportantesTemp: Dominio[] = res[0];

        this.nombresAportantes.forEach( nom => {
          let s = nombresAportantesTemp.find( nomTemp => nomTemp.dominioId == nom.nombreAportanteId )
          if (s){
            nom.nombreAportante = s.nombre;
          }
        });

         this.fuentesDeRecursosLista = res[1];
         this.bancos = res[2];
         this.departamentos = res[3];
         this.listaFuentes = res[5];

         if (this.idAportante > 0)
          this.EditMode();

      });
    })

    

    this.addressForm = this.fb.group({
      nombreAportante: [null],
      documentoApropiacion: [null],
      numerodocumento: [null, Validators.compose([
        Validators.minLength(10), Validators.maxLength(10)])
      ],
      vigenciaAcuerdo:[],
      departamento: [],
      municipio: [],
      tieneRP: [null],
      cuantosRP: [null,Validators.compose([
          Validators.minLength(5), Validators.maxLength(50)])]
      ,registrosPresupuestales: this.fb.array([])
      ,fuenteRecursosArray: this.fb.array([]),
    });

    let fuentes = this.addressForm.get('fuenteRecursosArray') as FormArray;
    fuentes.push(this.crearFuente());
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

    if (this.addressForm.get('nombreAportante').value){

      this.idAportante = this.addressForm.get('nombreAportante').value.cofinanciacionAportanteId;
      
      this.cofinanciacionService.getDocumentoApropiacionByAportante(this.idAportante).subscribe( listDoc => {
        if (listDoc.length > 0)
        {
          this.addressForm.get('numerodocumento').setValue(listDoc[0].numeroActa);
          this.addressForm.get('documentoApropiacion').setValue(listDoc[0].tipoDocumento.nombre);
        }
        else
          this.openDialog("Validacion","No tiene documentos de apropiacion");
      });
    }
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
      fuenteFinanciacionId: [null],
      departamento: [],
      municipio: [],
      tieneRP: [null],
      cuantosRP: [null,Validators.compose([
          Validators.minLength(5), Validators.maxLength(50)])]
      ,registrosPresupuestales: this.fb.array([]),
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

      let lista: FuenteFinanciacion[] = [];
      let listaRP: RegistroPresupuestal[] = []; 

      let usuario = '';
      if (localStorage.getItem('actualUser')){
        usuario = localStorage.getItem('actualUser'); 
        usuario = JSON.parse(usuario).email;
      }     

      this.fuenteRecursosArray.controls.forEach( controlFR => {
        let fuente: FuenteFinanciacion = {
          aportanteId: this.idAportante,
          fuenteRecursosCodigo: controlFR.get('fuenteRecursos').value.codigo,
          valorFuente: controlFR.get('valorFuenteRecursos').value,
          cantVigencias: controlFR.get('cuantasVigencias').value,
          cuentaBancaria: [],
          vigenciaAporte:[],
          
        }

        let vigencias = controlFR.get('vigencias') as FormArray;

        if (vigencias){
          vigencias.controls.forEach( controlVi => {
            let vigenciaAporte: VigenciaAporte = {
              fuenteFinanciacionId: controlFR.get('fuenteFinanciacionId').value,
              tipoVigenciaCodigo: controlVi.get('vigenciaAportante').value,
              valorAporte: controlVi.get('valorVigencia').value,
              vigenciaAporteId:0, 
              fechaCreacion: new Date,
              usuarioCreacion: usuario
            }

            fuente.vigenciaAporte.push(vigenciaAporte);

            })
          }

        let cuentas = controlFR.get('cuentasBancaria') as FormArray;
        cuentas.controls.forEach(controlBa => {
          let cuentaBancaria: CuentaBancaria = {
            cuentaBancariaId: 0,
            bancoCodigo: controlBa.get('banco').value.codigo,
            codigoSifi: controlBa.get('codigoSIFI').value,
            exenta: controlBa.get('extra').value,
            fuenteFinanciacionId: controlFR.get('fuenteFinanciacionId').value,
            nombreCuentaBanco: controlBa.get('nombreCuenta').value,
            numeroCuentaBanco: controlBa.get('numeroCuenta').value,
            tipoCuentaCodigo: controlBa.get('tipoCuenta').value,
            fechaCreacion: new Date,
            usuarioCreacion: usuario
          };
          
          fuente.cuentaBancaria.push(cuentaBancaria); 
        });

        let registrosPresupuestales = controlFR.get('registrosPresupuestales') as FormArray;
        if (registrosPresupuestales)
            {
              registrosPresupuestales.controls.forEach( controlRP => {
                let registroPresupuestal: RegistroPresupuestal = {
                  aportanteId: this.idAportante,
                  fechaRp: controlRP.get('numeroRP').value,
                  numeroRp: controlRP.get('fecha').value,
                  registroPresupuestalId: 0,
                  fechaCreacion: new Date,
                  usuarioCreacion: usuario
                }
                listaRP.push(registroPresupuestal);
              })
            }

        lista.push(fuente);
      });
     
     

      lista.forEach( ff => {
        if (ff.fuenteFinanciacionId > 0)
          this.fuenteFinanciacionService.registrarFuenteFinanciacion(ff).subscribe( respuesta => {
          });
        else
          this.fuenteFinanciacionService.modificarFuenteFinanciacion(ff).subscribe( respuesta => {
            
          }) 
      })

      this.data = lista;

      alert('Thanks!');
    }
  }
}
