import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-actuacion-reclamacion',
  templateUrl: './form-actuacion-reclamacion.component.html',
  styleUrls: ['./form-actuacion-reclamacion.component.scss']
})
export class FormActuacionReclamacionComponent implements OnInit {
  @Input() isEditable;
 
  addressForm = this.fb.group({
    estadoAvanceTramite: [null, Validators.required],
    fechaActuacionAdelantada: [null, Validators.required],
    actuacionAdelantada: [null, Validators.required],
    proximaActuacionRequerida: [null, Validators.required],
    diasVencimientoTerminos: [null, Validators.required],
    fechaVencimientoTerminos: [null, Validators.required],
    observaciones: [null, Validators.required],
    definitvoAseguradora: [null, Validators.required],
    urlSoporte: [null, Validators.required]
  });
  estadoAvanceTramiteArray = [
    { name: 'Aprobación de Comunicación de Inicio de TAI', value: '1' },
  ];
  actuacionAdelantadaArray = [
    { name: 'Actuación 1', value: '1' },
  ];
  proximaActuacionRequeridaArray = [
    { name: 'Otro', value: '1' },
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
  constructor(  private fb: FormBuilder, public dialog: MatDialog) { }

  ngOnInit(): void {
    if(this.isEditable==true){
      this.addressForm.get('estadoAvanceTramite').setValue('1');
      this.addressForm.get('fechaActuacionAdelantada').setValue('10/10/2020');
      this.addressForm.get('actuacionAdelantada').setValue('Prueba');
      this.addressForm.get('proximaActuacionRequerida').setValue('Cuando sea necesario');
      this.addressForm.get('diasVencimientoTerminos').setValue('3');
      this.addressForm.get('definitvoAseguradora').setValue(true);
    }
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