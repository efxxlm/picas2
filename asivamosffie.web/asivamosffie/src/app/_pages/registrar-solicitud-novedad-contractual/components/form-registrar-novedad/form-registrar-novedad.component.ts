import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CommonService, InstanciasSeguimientoTecnico, TiposNovedadModificacionContractual } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-registrar-novedad',
  templateUrl: './form-registrar-novedad.component.html',
  styleUrls: ['./form-registrar-novedad.component.scss']
})
export class FormRegistrarNovedadComponent implements OnInit {
  addressForm = this.fb.group({
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
  tiposNovedadModificacionContractual=TiposNovedadModificacionContractual;

  get plazoSolicitadoField() {
    return this.addressForm.get('plazoSolicitado') as FormArray;
  }

  get clausulaField() {
    return this.addressForm.get('clausula') as FormArray;
  }

  estaEditando = false;
  instanciaPresentoSolicitudArray = [];
  tipoNovedadArray = [];
  motivosNovedadArray = [
    { name: 'Alabama', value: 'AL' },
    { name: 'Alaska', value: 'AK' },
    { name: 'Vermont', value: 'VT' },
    { name: 'Virgin Islands', value: 'VI' },
    { name: 'Virginia', value: 'VA' },
    { name: 'Washington', value: 'WA' },
    { name: 'West Virginia', value: 'WV' },
    { name: 'Wisconsin', value: 'WI' },
    { name: 'Wyoming', value: 'WY' }
  ];

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
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    }
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
    public commonServices: CommonService
  ) { }

  ngOnInit(): void {
    this.addressForm.valueChanges
      .subscribe(value => {
        console.log(value);
      });
      this.commonServices.listaInstanciasdeSeguimientoTecnico().subscribe(response=>{
        this.instanciaPresentoSolicitudArray=response;
      });
      this.commonServices.listaTipoNovedadModificacionContractual().subscribe(response=>{
        this.tipoNovedadArray=response;
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
      console.log(`Dialog result: ${result}`);
      if (result === true) {
        this.deleteTema(e);
      }
    });
  }

  deleteTema(i: number) {
    const tema = this.clausulaField.controls[i];

    console.log(tema);

    this.borrarArray(this.clausulaField, i);
    this.openDialog('', '<b>La información ha sido eliminada correctamente.</b>');
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.estaEditando = true;
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
  }
}
