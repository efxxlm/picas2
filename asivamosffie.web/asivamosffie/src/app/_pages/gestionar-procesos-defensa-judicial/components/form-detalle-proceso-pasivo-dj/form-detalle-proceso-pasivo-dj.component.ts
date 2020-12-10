import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-detalle-proceso-pasivo-dj',
  templateUrl: './form-detalle-proceso-pasivo-dj.component.html',
  styleUrls: ['./form-detalle-proceso-pasivo-dj.component.scss']
})
export class FormDetalleProcesoPasivoDjComponent implements OnInit {

  addressForm = this.fb.group({
    departamentoInicio: [null, Validators.required],
    municipioInicio: [null, Validators.required],
    tipoAccion: [null, Validators.required],
    jurisdiccion: [null, Validators.required],
    pretensiones: [null, Validators.required],
    cuantiaPerjuicios: [null, Validators.required],
    requeridoParticipacionSupervisor: [null, Validators.required],
    fechaRadicado: [null, Validators.required],
    numeroRadicado: [null, Validators.required],
    canalIngreso: [null, Validators.required]
  });
  departamentoArray = [
    { name: 'Antioquia', value: '1' },
    { name: 'Atlantico', value: '2' },
  ];
  municipioArray = [
    { name: 'Soledad', value: '1' },
    { name: 'Amalfi', value: '2' },
  ];
  tipoAccionArray = [
    { name: 'Reparacion Directa', value: '1' },
    { name: 'Reparacion Indirecta', value: '2' },
  ];
  jurisdiccionArray = [
    { name: 'Ordinaria', value: '1' },
    { name: 'Extraordinaria', value: '2' },
  ];
  canalIngresoArray = [
    { name: 'Carta', value: '1' },
    { name: 'Internet', value: '2' },
  ];
  editorStyle = {
    height: '50px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  constructor(private fb: FormBuilder,public dialog: MatDialog) { }

  ngOnInit(): void {
  }
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.openDialog('', 'La información ha sido guardada exitosamente.');
  }
}
