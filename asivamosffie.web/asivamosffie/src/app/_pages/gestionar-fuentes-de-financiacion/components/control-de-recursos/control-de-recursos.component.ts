import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { TiposAportante, CommonService, Dominio, Localizacion } from 'src/app/core/_services/common/common.service';
import { ActivatedRoute } from '@angular/router';
import { FuenteFinanciacion, FuenteFinanciacionService, CuentaBancaria, RegistroPresupuestal, ControlRecurso, VigenciaAporte } from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';
import { forkJoin } from 'rxjs';
import { CofinanciacionDocumento } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

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
  vigencia: number;
  nombreFuente: string = '';
  valorFuente: number = 0;
  idFuente: number = 0;
  idControl: number = 0;
  listaNombres: Dominio[] = [];
  listaFuentes: Dominio[] = [];
  listaDepartamentos: Localizacion[] = [];
  listaVigencias: VigenciaAporte[] = [];
  fuente: FuenteFinanciacion;
  fuenteFinaciacionId: number = 0;
  tipoAportanteId: string = '';

  NombresDeLaCuenta: CuentaBancaria[] = [] ;

  rpArray: RegistroPresupuestal[] = [];

  constructor(
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private fuenteFinanciacionServices: FuenteFinanciacionService,
              private commonService: CommonService,
              private dialog: MatDialog
             ) 
  {}

  ngOnInit(): void {
    this.addressForm = this.fb.group({
      controlRecursoId: [],
      nombreCuenta: [null, Validators.required],
      numeroCuenta: [this.fb.array([]), Validators.required],
      rp: [null],
      vigencia: [null],
      fechaConsignacion: [null, Validators.required],
      valorConsignacion: [null, Validators.compose([
        Validators.required, Validators.minLength(4), Validators.maxLength(50)])
      ],
    });

    this.activatedRoute.params.subscribe( param => {
      this.idFuente = param['idFuente'];
      this.idControl = param['idControl'];

      forkJoin([
        this.fuenteFinanciacionServices.getFuenteFinanciacion( this.idFuente ),
        this.commonService.listaNombreAportante(),
        this.commonService.listaFuenteTipoFinanciacion(),
        this.commonService.listaDepartamentos()
        
      ]).subscribe( respuesta => {        
        this.fuente = respuesta[0];
        this.listaNombres = respuesta[1];
        this.listaFuentes = respuesta[2];
        this.listaDepartamentos = respuesta[3];
        this.tipoAportanteId = this.fuente.aportante.tipoAportanteId;
        if(this.tipoAportante.ET.includes(this.tipoAportanteId.toString()))
        {
          let valorDepartamento = this.listaDepartamentos.find( de => de.localizacionId.toString() == 
          this.fuente.aportante.departamentoId.toString() )
          if (valorDepartamento){
            console.log("tiene departamento ");
            console.log(valorDepartamento);
            this.commonService.listaMunicipiosByIdDepartamento( this.fuente.aportante.departamentoId.toString() ).subscribe( mun => {
              if (mun){
                let valorMunicipio = mun.find( m => m.localizacionId == this.fuente.aportante.municipioId.toString() )
                this.municipio = valorMunicipio?valorMunicipio.descripcion:"";
              }
            })
            this.departamento = valorDepartamento?valorDepartamento.descripcion:"";
          }
          
          
        }
        let valorNombre = this.listaNombres.find( nom => nom.dominioId == this.fuente.aportante.nombreAportanteId );
        let valorFuente = this.listaFuentes.find( fue => fue.codigo == this.fuente.fuenteRecursosCodigo );
        let valorMunicipio: string = '';
        

        
        //this.nombreAportante = valorNombre.nombre;
        this.nombreFuente = valorFuente.nombre;
        this.valorFuente = this.fuente.valorFuente;
        this.fuenteFinaciacionId = this.fuente.fuenteFinanciacionId;
        this.vigencia = this.fuente.aportante.cofinanciacion.vigenciaCofinanciacionId;
        this.NombresDeLaCuenta = this.fuente.cuentaBancaria;
        this.rpArray = this.fuente.aportante.registroPresupuestal;
        //la lista de vigencias son los documentos registrados en acuerdos de cofinanciacion 
        this.fuente.aportante.cofinanciacionDocumento.forEach(element => {
          this.listaVigencias.push({tipoVigenciaCodigo:element.vigenciaAporte.toString(),fuenteFinanciacionId:null,valorAporte:null,vigenciaAporteId:element.cofinanciacionDocumentoId});
        });
        //this.listaVigencias = this.fuente.vigenciaAporte;

        if (this.idControl > 0 )
          this.editMode();

      })
    })

  }

  editMode(){
    this.fuenteFinanciacionServices.getResourceControlById( this.idControl ).subscribe( cr => {
      let cuentaSeleccionada = this.NombresDeLaCuenta.find( c => c.cuentaBancariaId == cr.cuentaBancariaId )
      let rpSeleccionado = this.rpArray.find( rp => rp.registroPresupuestalId == cr.registroPresupuestalId )
      let vigenciaSeleccionada = this.listaVigencias.find( vi => vi.vigenciaAporteId == cr.vigenciaAporteId )

      this.addressForm.get('nombreCuenta').setValue(cuentaSeleccionada);
      this.addressForm.get('rp').setValue(rpSeleccionado);
      this.addressForm.get('controlRecursoId').setValue(cr.controlRecursoId);
      this.addressForm.get('vigencia').setValue(vigenciaSeleccionada);
      this.addressForm.get('fechaConsignacion').setValue(cr.fechaConsignacion);
      this.addressForm.get('valorConsignacion').setValue(cr.valorConsignacion);
      this.changeNombreCuenta();
    })
  }

  changeNombreCuenta(){
    let cuentaSeleccionada = this.addressForm.get('nombreCuenta').value;
    this.addressForm.get('numeroCuenta').setValue(cuentaSeleccionada.numeroCuentaBanco);
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
    dialogRef.afterClosed().subscribe(result => {
      if(result === true)
      {
        location.reload();
      }
    });
  }

  onSubmit(){
    if (this.addressForm.valid){

      let rp = this.addressForm.get('rp').value;
console.log(this.addressForm.get('vigencia').value);
console.log(this.addressForm);
      let control: ControlRecurso = {
        controlRecursoId: this.addressForm.get('controlRecursoId').value,
        cuentaBancariaId: this.addressForm.get('nombreCuenta').value.cuentaBancariaId,
        fechaConsignacion: this.addressForm.get('fechaConsignacion').value,
        fuenteFinanciacionId: this.fuenteFinaciacionId,
        registroPresupuestalId: rp? rp.registroPresupuestalId:null,
        valorConsignacion: this.addressForm.get('valorConsignacion').value,
        vigenciaAporteId: this.addressForm.get('vigencia').value?.vigenciaAporteId
      }

      if (control.controlRecursoId > 0)
        this.fuenteFinanciacionServices.updateControlRecurso( control ).subscribe( respuesta => {
          this.openDialog( '', respuesta.message );
        })  
      else
        this.fuenteFinanciacionServices.registrarControlRecurso( control ).subscribe( respuesta => {
          this.openDialog( '', respuesta.message );
        })

    }
  }

}
