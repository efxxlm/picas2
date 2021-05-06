import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Dominio, InstanciasSeguimientoTecnico, TiposNovedadModificacionContractual } from 'src/app/core/_services/common/common.service';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { Proyecto } from 'src/app/core/_services/project/project.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { NovedadContractual, NovedadContractualDescripcion } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-form-registrar-novedad',
  templateUrl: './form-registrar-novedad.component.html',
  styleUrls: ['./form-registrar-novedad.component.scss']
})
export class FormRegistrarNovedadComponent implements OnInit, OnChanges {

  addressForm = this.fb.group({
    novedadContractualId: [],
    fechaSolicitudNovedad: [null, Validators.required],
    instanciaPresentoSolicitud: [null, Validators.required],
    fechaSesionInstancia: [null, Validators.required],
    tipoNovedad: [null, Validators.required],
    motivosNovedad: [null, Validators.required],
    resumenJustificacionNovedad: [null, Validators.required],
    documentacion: [null, Validators.required],
    plazoSolicitado: this.fb.array([
      this.fb.group({
        fechaInicio: [null, Validators.required],
        fechaFinal: [null, Validators.required],

      })
    ]),

    clausula: this.fb.array([
      this.fb.group({
        clausulaModificar: [null, Validators.required],
        ajusteSolicitadoClausula: [null, Validators.required]
      })
    ]),

    documentacionSuficiente: [null, Validators.required],
    conceptoTecnico: [null, Validators.required],
    fechaConceptoTecnico: [null, Validators.required],
    numeroRadicadoSolicitud: [null, Validators.compose([
      Validators.required, Validators.minLength(5), Validators.maxLength(20)])
    ]
  });

  instanciaSeguimientoTecnico = InstanciasSeguimientoTecnico;
  tiposNovedadModificacionContractual = TiposNovedadModificacionContractual;
  novedadContractual: NovedadContractual = {

  }

  get plazoSolicitadoField() {
    return this.addressForm.get('plazoSolicitado') as FormArray;
  }

  get clausulaField() {
    return this.addressForm.get('clausula') as FormArray;
  }

  estaEditando = false;
  instanciaPresentoSolicitudArray = [];
  tipoNovedadArray = [];
  motivosNovedadArray = [];

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

  @Input() proyecto: any;
  @Input() contrato: any;
  @Input() novedad: NovedadContractual;

  @Output() guardar = new EventEmitter();

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    public commonServices: CommonService,
    public contractualNoveltyService: ContractualNoveltyService,
    private router: Router,
    private activatedRoute: ActivatedRoute,

  ) {

  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.novedad) {

      

      this.commonServices.listaTipoNovedadModificacionContractual().subscribe(response => {

        response.forEach(n => {
          let novedadContractualDescripcion: NovedadContractualDescripcion = {
            tipoNovedadCodigo: n.codigo,
            nombreTipoNovedad: n.nombre,

          }

          this.tipoNovedadArray.push(novedadContractualDescripcion);
        });

          this.addressForm.get('novedadContractualId').setValue(this.novedad.novedadContractualId);
          this.addressForm.get('fechaSolicitudNovedad').setValue(this.novedad.fechaSolictud);
          this.addressForm.get('instanciaPresentoSolicitud').setValue(this.novedad.instanciaCodigo);
          this.addressForm.get('fechaSesionInstancia').setValue(this.novedad.fechaSesionInstancia);

          this.novedadContractual = this.novedad;

          let listaDescripcion: NovedadContractualDescripcion[] = [];


          this.novedad.novedadContractualDescripcion.forEach(n => {

            

            let tipoNovedadseleccionada = this.tipoNovedadArray.filter(r => r.tipoNovedadCodigo === n.tipoNovedadCodigo).shift();
            this.tipoNovedadArray = this.tipoNovedadArray.filter(r => r.tipoNovedadCodigo !== n.tipoNovedadCodigo)
            this.tipoNovedadArray.push( n );
            listaDescripcion.push( n );
            
          });

          this.addressForm.get('tipoNovedad').setValue(listaDescripcion);

        
      });


    }
  }

  ngOnInit(): void {

    // this.addressForm.valueChanges
    //   .subscribe(value => {
    //     //console.log(value);
    //   });


    this.commonServices.listaInstanciasdeSeguimientoTecnico().subscribe(response => {
      this.instanciaPresentoSolicitudArray = response;
    });


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

    this.borrarArray(this.clausulaField, i);
    this.openDialog('', '<b>La información ha sido eliminada correctamente.</b>');
  }

  changeTipoNovedad() {
    if (this.addressForm.get('tipoNovedad').value)
      this.novedadContractual.novedadContractualDescripcion = this.addressForm.get('tipoNovedad').value;

    }

  onSubmit() {

    this.estaEditando = true;
    this.addressForm.markAllAsTouched();

    let novedad: NovedadContractual = {
      novedadContractualId: this.addressForm.value ? this.addressForm.value.novedadContractualId : 0,
      fechaSolictud: this.addressForm.value ? this.addressForm.value.fechaSolicitudNovedad : null,
      instanciaCodigo: this.addressForm.value ? this.addressForm.value.instanciaPresentoSolicitud : null,
      proyectoId: this.proyecto ? this.proyecto.proyectoId : null,
      contratoId: this.contrato.contratoId,
      fechaSesionInstancia: this.addressForm.value ? this.addressForm.value.fechaSesionInstancia : null,
      esAplicadaAcontrato: this.novedad.esAplicadaAcontrato,
      urlSoporte: this.novedad.urlSoporte,

      novedadContractualDescripcion: this.novedadContractual.novedadContractualDescripcion,

    };

    this.contractualNoveltyService.createEditNovedadContractual(novedad)
      .subscribe(respuesta => {
        this.openDialog('', respuesta.message);
        if (respuesta.code === '200')
          
          if (novedad.novedadContractualId === undefined)
            this.router.navigate(['/verificarSolicitudDeNovedades/registrarSolicitudInterventoria', respuesta.data.novedadContractualId]);
          else
            this.guardar.emit(true);
      });

      

  }

  habilitarTipoNovedad(tnovedad) {
    if (((tnovedad.tipoNovedadCodigo === '2' || tnovedad.tipoNovedadCodigo === '6') && this.contrato.tieneSuspensionAprobada === false))
      return 'none'
    else {
      return 'flex'
    }
  }
  
}
