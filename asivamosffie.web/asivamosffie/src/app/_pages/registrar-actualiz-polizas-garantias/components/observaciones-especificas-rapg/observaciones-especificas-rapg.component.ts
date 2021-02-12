import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-observaciones-especificas-rapg',
  templateUrl: './observaciones-especificas-rapg.component.html',
  styleUrls: ['./observaciones-especificas-rapg.component.scss']
})
export class ObservacionesEspecificasRapgComponent implements OnInit {
  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observacionesEspecificas: [null, Validators.required]
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
  estaEditando = false;

  constructor(private fb: FormBuilder, public dialog: MatDialog) { }

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  maxLength(e: any, n: number) {
    
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n-1, e.editor.getLength());
    }
  }

  textoLimpio(texto,n) {
    if (texto!=undefined) {
      return texto.getLength() > n ? n : texto.getLength();
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  ngOnInit(): void {
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.estaEditando = true;
  }

}
