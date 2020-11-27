import { Component } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-registrar-novedad',
  templateUrl: './form-registrar-novedad.component.html',
  styleUrls: ['./form-registrar-novedad.component.scss']
})
export class FormRegistrarNovedadComponent {
  addressForm = this.fb.group({
    fechaSolicitudNovedad: [null, Validators.required],
    instanciaPresentoSolicitud: [null, Validators.required],
    fechaSesionInstancia: [null, Validators.required],
    tipoNovedad: [null, Validators.required],
    motivosNovedad: [null, Validators.required],
    resumenJustificacionNovedad: [null, Validators.required],
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

  get clausulaField() {
    return this.addressForm.get('clausula') as FormArray;
  }

  estaEditando = false;

  instanciaPresentoSolicitudArray = [
    { name: 'Comité de obra', value: '1' },
    { name: 'No aplica', value: 'noAplica' }
  ];
  tipoNovedadArray = [
    { name: 'Suspensión', value: '1' },
    { name: 'Prórroga a las Suspensión', value: '2' },
    { name: 'Adición', value: '3' },
    { name: 'Prórroga', value: '4' },
    { name: 'Modificación de Condiciones Contractuales', value: '5' }
  ];
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
  ) { }

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
    let tema = this.addressForm.get('tema');
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

  openDialogSiNo(modalTitle: string, modalText: string, e: number, grupo: any) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result === true) {
        this.deleteTema(e)
      }
    });
  }

  deleteTema(i: number) {
    let tema = this.clausulaField.controls[i];

    console.log(tema)

    this.borrarArray(this.clausulaField, i)
    this.openDialog('', '<b>La información ha sido eliminada correctamente.</b>')
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.estaEditando = true;
    this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>')
  }
}
