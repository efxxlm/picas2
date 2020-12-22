import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { DefensaJudicial } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-ficha-estudio-dj',
  templateUrl: './form-ficha-estudio-dj.component.html',
  styleUrls: ['./form-ficha-estudio-dj.component.scss']
})
export class FormFichaEstudioDjComponent implements OnInit {

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
 
  addressForm = this.fb.group({
    antecedentes: [null, Validators.required],
    hechosRelevantes: [null, Validators.required],
    jurisprudenciaDoctrina: [null, Validators.required],
    decisionComite: [null, Validators.required],
    analisisJuridico: [null, Validators.required],
    recomendaciones: [null, Validators.required],
    procesoFichaComite: [null, Validators.required],
    fechaComiteDefensa : [null, Validators.required],
    recomendacionFinal : [null, Validators.required],
    aperturaFormalProceso: [null, Validators.required],
    tipoActuacionRecomendada: [null, Validators.required],
    actuacionRecomendadaAlComite: [null, Validators.required],
    urlSoporte: [null, Validators.required]
  });
  actuacionesArray = [
    { name: 'Actuacion 1', value: '1' },
    { name: 'Actuacion 2', value: '2' },
  ];
  constructor(  private fb: FormBuilder, public dialog: MatDialog) { }

  @Input() legitimacion:boolean;
  @Input() tipoProceso:string;
  @Input() defensaJudicial:DefensaJudicial;
  cargarRegistro() {
    //this.ngOnInit().then(() => {
      console.log("form");
      console.log(this.defensaJudicial);
      console.log(this.legitimacion);
      console.log(this.tipoProceso);      
  }

  ngOnInit(): void {
  }
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  maxLength(e: any, n: number) {
    console.log(e.editor.getLength()+" "+n);
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

  onSubmit() {
    console.log(this.addressForm.value);
    this.openDialog('', 'La información ha sido guardada exitosamente.');
  }
}
