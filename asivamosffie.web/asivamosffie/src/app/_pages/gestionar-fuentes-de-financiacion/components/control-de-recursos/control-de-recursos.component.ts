import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { TiposAportante, CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ActivatedRoute } from '@angular/router';
import { FuenteFinanciacion, FuenteFinanciacionService, CuentaBancaria, RegistroPresupuestal, ControlRecurso } from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';
import { forkJoin } from 'rxjs';
import { CofinanciacionDocumento } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';

@Component({
  selector: 'app-control-de-recursos',
  templateUrl: './control-de-recursos.component.html',
  styleUrls: ['./control-de-recursos.component.scss']
})
export class ControlDeRecursosComponent implements OnInit {
  
  addressForm: FormGroup;
  hasUnitNumber = false;
  tipoAportante = TiposAportante;
  nombreAportante: string = '';
  departamento: string = '';
  municipio: string = '';
  vigencia: string = '';
  nombreFuente: string = '';
  valorFuente: number = 0;
  idFuente: number = 0;
  listaNombres: Dominio[] = [];
  listaFuentes: Dominio[] = [];
  listaVigencias: CofinanciacionDocumento[] = [];
  fuente: FuenteFinanciacion;
  fuenteFinaciacionId: number = 0;

  NombresDeLaCuenta: CuentaBancaria[] = [] ;

  rpArray: RegistroPresupuestal[] = [];

  constructor(
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private fuenteFinanciacionServices: FuenteFinanciacionService,
              private commonService: CommonService,
             ) 
  {}

  ngOnInit(): void {
    this.addressForm = this.fb.group({
      controlRecursoId: [],
      nombreCuenta: [null, Validators.required],
      numeroCuenta: [this.fb.array([]), Validators.required],
      rp: [null],
      vigencia: [null, Validators.required],
      fechaConsignacion: [null, Validators.required],
      valorConsignacion: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(50)])
      ],
    });

    this.activatedRoute.params.subscribe( param => {
      this.idFuente = param['id'];

      forkJoin([
        this.fuenteFinanciacionServices.getFuenteFinanciacion( this.idFuente ),
        this.commonService.listaNombreAportante(),
        this.commonService.listaFuenteRecursos(),
        this.commonService.listaDepartamentos(),  
      ]).subscribe( respuesta => {
        this.fuente = respuesta[0];
        this.listaNombres = respuesta[1];
        this.listaFuentes = respuesta[2];

        let valorNombre = this.listaNombres.find( nom => nom.dominioId == this.fuente.aportante.nombreAportanteId );
        let valorFuente = this.listaFuentes.find( fue => fue.codigo == this.fuente.fuenteRecursosCodigo );

        this.nombreAportante = valorNombre.nombre;
        this.nombreFuente = valorFuente.nombre;
        this.valorFuente = this.fuente.valorFuente;
        this.fuenteFinaciacionId = this.fuente.fuenteFinanciacionId;
        //this.vigencia = this.fuente.aportante.cofinanciacion.vigenciaCofinanciacionId;
        this.NombresDeLaCuenta = this.fuente.cuentaBancaria;
        this.rpArray = this.fuente.aportante.registroPresupuestal;
        this.listaVigencias = this.fuente.aportante.cofinanciacionDocumento;

      })
    })

  }

  changeNombreCuenta(){
    let cuentaSeleccionada = this.addressForm.get('nombreCuenta').value;
    this.addressForm.get('numeroCuenta').setValue(cuentaSeleccionada.numeroCuentaBanco);
  }

  onSubmit(){
    if (this.addressForm.valid){
      let control: ControlRecurso = {
        controlRecursoId: this.addressForm.get('formControlName').value,
        cuentaBancariaId: this.addressForm.get('nombreCuenta').value.cuentaBancariaId,
        fechaConsignacion: this.addressForm.get('fechaConsignacion').value,
        fuenteFinanciacionId: this.fuenteFinaciacionId,
        registroPresupuestalId: this.addressForm.get('fechaConsignacion').value.registroPresupuestalId,
        valorConsignacion: this.addressForm.get('valorConsignacion').value,
        vigenciaAporteId: 0
      }

      console.log(control);

    }
  }

}
