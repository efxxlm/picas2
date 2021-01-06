import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-reclamacion-aseguradora-actuacion',
  templateUrl: './form-reclamacion-aseguradora-actuacion.component.html',
  styleUrls: ['./form-reclamacion-aseguradora-actuacion.component.scss']
})
export class FormReclamacionAseguradoraActuacionComponent implements OnInit {
  @Input() isEditable;

  addressForm = this.fb.group({
    resumenReclamacionFiduciaria: [null, Validators.required],
    requereReclamacionComiteTecnico: [null, Validators.required],
    urlSoporte: [null, Validators.required],
  });
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
      this.addressForm.get('requiereReclamacionAseguradora').setValue(true);
      this.addressForm.get('resumenReclamacionFiduciaria').setValue('prueba');
      this.addressForm.get('requereReclamacionComiteTecnico').setValue(true);
      this.addressForm.get('definitivoyCerrado').setValue(true);
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
