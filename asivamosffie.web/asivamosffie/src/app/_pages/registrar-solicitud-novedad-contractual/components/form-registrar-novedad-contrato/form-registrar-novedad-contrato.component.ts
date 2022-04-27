import { EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { NovedadContractualClausula, NovedadContractualDescripcion, NovedadContractualDescripcionMotivo } from 'src/app/_interfaces/novedadContractual';
import * as moment from 'moment';
import { truncate } from 'fs';

@Component({
  selector: 'app-form-registrar-novedad-contrato',
  templateUrl: './form-registrar-novedad-contrato.component.html',
  styleUrls: ['./form-registrar-novedad-contrato.component.scss']
})
export class FormRegistrarNovedadContratoComponent implements OnInit, OnChanges {
  @Input() tiposNovedadModificacionContractual;
  @Input() novedadDescripcion: NovedadContractualDescripcion;
  @Input() estaEditando: boolean;
  @Input() fechaSolicitudNovedad: Date;
  @Input() fechaFinSuspensionVal: Date;
  @Input() contrato: any;
  @Input() datosContratoProyectoModificadosXNovedad: any;
  @Input() estadoNovedad: string;
  @Output() guardar = new EventEmitter();

  presupuestoModificado: number;
  plazoDiasModificado: number;
  plazoMesesModificado: number;
  validaParaModificar = false;

  nombreTiposolicitud: string;
  fechaFinalizacionContrato;
  fechaEstimadaFinalizacion;
  addressForm = this.fb.group({
    novedadContractualDescripcionId: [],
    fechaSolicitudNovedad: [null, Validators.required],
    instanciaPresentoSolicitud: [null, Validators.required],
    fechaSesionInstancia: [null, Validators.required],
    tipoNovedad: [null, Validators.required],
    motivosNovedad: [null, Validators.required],
    presupuestoAdicional: [],
    plazoAdicionalDias: [null, Validators.required],
    plazoAdicionalMeses: [null, Validators.required],
    resumenJustificacionNovedad: [null, Validators.required],
    documentacion: [null, Validators.required],
    fechaInicio: [null, Validators.required],
    fechaFinal: [null, Validators.required],

    clausula: this.fb.array([
      this.fb.group({
        clausulaModificar: [null, Validators.required],
        ajusteSolicitadoClausula: [null, Validators.required]
      })
    ]),
    documentacionSuficiente: [null, Validators.required],
    conceptoTecnico: [null],
    fechaConceptoTecnico: [null],
    numeroRadicadoSolicitud: [null, Validators.maxLength(20)]
  });

  get plazoSolicitadoField() {
    return this.addressForm.get('plazoSolicitado') as FormArray;
  }

  get clausulaField() {
    return this.addressForm.get('clausula') as FormArray;
  }

  instanciaPresentoSolicitudArray = [];
  tipoNovedadArray = [];
  motivosNovedadArray: Dominio[] = [];

  // minDate: Date;
  editorStyle = {
    height: '45px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if ( texto ){
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    public commonServices: CommonService,
    private contractualNoveltyService: ContractualNoveltyService,

  ) { }

  ngOnChanges(changes: SimpleChanges): void {
    this.commonServices.listaMotivosNovedadContractual()
      .subscribe(respuesta => {
        this.motivosNovedadArray = respuesta;

        let motivosSeleccionados = [];
        if (this.novedadDescripcion.novedadContractualDescripcionMotivo) {
          this.novedadDescripcion.novedadContractualDescripcionMotivo.forEach(m => {
            let motivo = this.motivosNovedadArray.find(r => r.codigo === m.motivoNovedadCodigo)?.codigo;

            if (motivo) {
              motivosSeleccionados.push(motivo);
            }
          });
          this.addressForm.get('motivosNovedad').setValue(motivosSeleccionados);
        }

      });

    if (changes.novedadDescripcion) {

      this.addressForm.get('novedadContractualDescripcionId').setValue(this.novedadDescripcion.novedadContractualDescripcionId);
      this.addressForm.get('resumenJustificacionNovedad').setValue(this.novedadDescripcion.resumenJustificacion);
      this.addressForm.get('documentacionSuficiente').setValue(this.novedadDescripcion.esDocumentacionSoporte);
      this.addressForm.get('conceptoTecnico').setValue(this.novedadDescripcion.conceptoTecnico);
      this.addressForm.get('fechaConceptoTecnico').setValue(this.novedadDescripcion.fechaConcepto);
      this.addressForm.get('numeroRadicadoSolicitud').setValue(this.novedadDescripcion.numeroRadicado);

      this.addressForm.get('presupuestoAdicional').setValue(this.novedadDescripcion.presupuestoAdicionalSolicitado);
      this.addressForm.get('plazoAdicionalDias').setValue(this.novedadDescripcion.plazoAdicionalDias);
      this.addressForm.get('plazoAdicionalMeses').setValue(this.novedadDescripcion.plazoAdicionalMeses);
      this.addressForm.get('fechaInicio').setValue(this.novedadDescripcion.fechaInicioSuspension);
      this.addressForm.get('fechaFinal').setValue(this.novedadDescripcion.fechaFinSuspension);

      this.clausulaField.clear();

      if ( this.novedadDescripcion.tipoNovedadCodigo === '5' ){
        if (this.novedadDescripcion.novedadContractualClausula) {
          this.novedadDescripcion.novedadContractualClausula.forEach(c => {
            let grupo = this.crearClausula();
            grupo.get('novedadContractualDescripcionId').setValue(c.novedadContractualDescripcionId);
            grupo.get('novedadContractualClausulaId').setValue(c.novedadContractualClausulaId);
            grupo.get('clausulaModificar').setValue(c.clausulaAmodificar);
            grupo.get('ajusteSolicitadoClausula').setValue(c.ajusteSolicitadoAclausula);

            this.clausulaField.push(grupo);

          });
        }else{
          let grupo = this.crearClausula();
          this.clausulaField.push(grupo);
        }
      }
      if (this.estaEditando) this.addressForm.markAllAsTouched();
    }
  }

  ngOnInit(): void {

    if(this.estadoNovedad == "12" || this.estadoNovedad == "19" || this.estadoNovedad == "25" || this.estadoNovedad == "26" )
      this.validaParaModificar = true;

    if(this.fechaFinSuspensionVal != null && this.fechaFinSuspensionVal != undefined){
      this.fechaFinSuspensionVal =  moment(this.fechaFinSuspensionVal).add(1, 'days').toDate();
    }
    this.addressForm.valueChanges
      .subscribe(value => {

      });
    this.commonServices.listaInstanciasdeSeguimientoTecnico().subscribe(response => {
      this.instanciaPresentoSolicitudArray = response;
    });
    this.commonServices.listaTipoNovedadModificacionContractual().subscribe(response => {
      this.tipoNovedadArray = response;
    });
    this.fechaEstimadaFinalizacion = this.datosContratoProyectoModificadosXNovedad?.fechaEstimadaFinProyecto;
    this.updateFechaEstimada(true);
    //si el estado es en proceso y no debe quitarse aun hay que quitar esto, falta por definir
    this.presupuestoModificado =  this.datosContratoProyectoModificadosXNovedad?.valorTotalProyecto - (this.validaParaModificar == true ?  (this.addressForm.get('presupuestoAdicional').value > 0 ? this.addressForm.get('presupuestoAdicional').value : 0) : 0);
    let pDias = (this.datosContratoProyectoModificadosXNovedad?.plazoEstimadoDiasProyecto + (this.datosContratoProyectoModificadosXNovedad?.plazoEstimadoMesesProyecto * 30)) - (this.validaParaModificar == true ? ((this.addressForm.get('plazoAdicionalDias').value > 0 ? this.addressForm.get('plazoAdicionalDias').value : 0) + (this.addressForm.get('plazoAdicionalMeses').value > 0 ? this.addressForm.get('plazoAdicionalMeses').value * 30 : 0)) : 0);
    this.plazoDiasModificado =  Math.trunc(pDias%30);
    this.plazoMesesModificado =  Math.trunc(pDias/30);

  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  eliminarClausula(i: number) {
    const tema = this.addressForm.get('tema');
    this.openDialogSiNo('', '<b>¿Está seguro de eliminar este registro?</b>', i, tema);
  }

  agregarClausula() {
    this.clausulaField.push(this.crearClausula());
  }

  private crearClausula() {
    return this.fb.group({
      novedadContractualDescripcionId: [],
      novedadContractualClausulaId: [],
      clausulaModificar: [null, Validators.required],
      ajusteSolicitadoClausula: [null, Validators.required]
    });
  }

  agregarPlazoSolicitado() {
    this.plazoSolicitadoField.push(this.crearPlazoSolicitado());
  }

  private crearPlazoSolicitado() {
    return this.fb.group({
      fechaInicio: [null, Validators.required],
      fechaFinal: [null, Validators.required],
      documentacion: [null, Validators.required]
    });
  }

  openDialogSiNo(modalTitle: string, modalText: string, e: number, grupo: any) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {

      if (result === true) {
        this.deleteTema(e);
      }
    });
  }

  deleteTema(i: number) {
    const tema = this.clausulaField.controls[i];

    this.contractualNoveltyService.eliminarNovedadClausula( this.clausulaField.value[i].novedadContractualClausulaId )
      .subscribe( respuesta => {
        this.openDialog('', respuesta.message);
        if ( respuesta.code === '200' )
          this.borrarArray(this.clausulaField, i);
      });
  }

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    let listaClausulas: NovedadContractualClausula[] = [];
    let listaMotivos: NovedadContractualDescripcionMotivo[] = [];

    this.clausulaField.controls.forEach(control => {
      let clausula: NovedadContractualClausula = {
        novedadContractualDescripcionId: this.addressForm.get('novedadContractualDescripcionId').value,
        novedadContractualClausulaId: control.value.novedadContractualClausulaId,
        ajusteSolicitadoAclausula: control.value.ajusteSolicitadoClausula,
        clausulaAmodificar: control.value.clausulaModificar,

      }
      listaClausulas.push(clausula);
    });

    if (this.addressForm.get('motivosNovedad').value) {
      this.addressForm.get('motivosNovedad').value.forEach(m => {
        let motivo: NovedadContractualDescripcionMotivo = {
          novedadContractualDescripcionMotivoId: m.novedadContractualDescripcionMotivoId,
          novedadContractualDescripcionId: this.addressForm.get('novedadContractualDescripcionId').value,
          motivoNovedadCodigo: m,

        }
        listaMotivos.push(motivo);
      });
    }

    this.novedadDescripcion.resumenJustificacion = this.addressForm.get('resumenJustificacionNovedad').value;
    this.novedadDescripcion.esDocumentacionSoporte = this.addressForm.get('documentacionSuficiente').value;
    this.novedadDescripcion.conceptoTecnico = this.addressForm.get('conceptoTecnico').value;
    this.novedadDescripcion.fechaConcepto = this.addressForm.get('fechaConceptoTecnico').value;
    this.novedadDescripcion.numeroRadicado = this.addressForm.get('numeroRadicadoSolicitud').value;
    this.novedadDescripcion.presupuestoAdicionalSolicitado = this.addressForm.get('presupuestoAdicional').value;
    this.novedadDescripcion.plazoAdicionalDias = this.addressForm.get('plazoAdicionalDias').value;
    this.novedadDescripcion.plazoAdicionalMeses = this.addressForm.get('plazoAdicionalMeses').value;
    this.novedadDescripcion.fechaInicioSuspension = this.addressForm.get('fechaInicio').value;
    this.novedadDescripcion.fechaFinSuspension = this.addressForm.get('fechaFinal').value;

    this.novedadDescripcion.novedadContractualClausula = listaClausulas;
    this.novedadDescripcion.novedadContractualDescripcionMotivo = listaMotivos;
    this.novedadDescripcion.novedadContractualDescripcionId = this.addressForm.get('novedadContractualDescripcionId').value;

    this.guardar.emit(true);

  }

  updateFechaEstimada(primeravez : boolean){
    const fechaInicio = moment( new Date( this.addressForm.get('fechaInicio').value ).setHours( 0, 0, 0, 0 ) );
    const fechaFin = moment( new Date( this.addressForm.get('fechaFinal').value ).setHours( 0, 0, 0, 0 ) );
    const duracionDias = fechaFin.diff( fechaInicio, 'days' );

    if(primeravez == true && this.validaParaModificar == true)
      this.fechaEstimadaFinalizacion = moment(this.fechaEstimadaFinalizacion).add(-duracionDias, 'days');

    return  moment(this.fechaEstimadaFinalizacion).add(duracionDias, 'days').toDate()
  }

  valuePresupuesto(){
    return this.presupuestoModificado  + (this.addressForm.get('presupuestoAdicional').value > 0 ? this.addressForm.get('presupuestoAdicional').value : 0);
  }

  valuePlazoProyecto(){
    let diasAdicionales = this.plazoDiasModificado + (this.addressForm.get('plazoAdicionalDias').value > 0 ? this.addressForm.get('plazoAdicionalDias').value : 0);
    let mesesAdicionales = this.plazoMesesModificado + (this.addressForm.get('plazoAdicionalMeses').value > 0 ? this.addressForm.get('plazoAdicionalMeses').value * 30 : 0);
    let total = diasAdicionales + mesesAdicionales;
    let pDias = Math.trunc(total%30);
    let pMeses = Math.trunc(total/30);

    return pMeses + " Meses / " + pDias + " Días";
  }

}
