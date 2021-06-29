import { DecimalPipe } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { PlazoContratacion } from 'src/app/_interfaces/project-contracting';

@Component({
  selector: 'app-plazo-ejecucion',
  templateUrl: './plazo-ejecucion.component.html',
  styleUrls: ['./plazo-ejecucion.component.scss']
})
export class PlazoEjecucionComponent implements OnInit {

  constructor(private fb: FormBuilder, public dialog: MatDialog,) { }
  @Input() tipoSolicitudCodigo: "1" | "2" | "3";
  @Input() plazoProyecto: number;
  @Input() plazoContratacion: PlazoContratacion
  @Output() guardar = new EventEmitter<PlazoContratacion>();
  estaEditando = false;
  termLimitForm = this.fb.group({
    plazoMeses: [null, [Validators.required, Validators.maxLength(3), Validators.min(1), Validators.max(999)]],
    plazoDias: [null, [Validators.required, Validators.maxLength(2), Validators.min(1), Validators.max(29)]]
  });

  ngOnInit(): void {

    this.monthsField.setValue(this.plazoContratacion.plazoMeses);
    this.daysField.setValue(this.plazoContratacion.plazoDias);
  }

  get monthsField(){
    return this.termLimitForm.get('plazoMeses');
  }

  
  get daysField(){
    return this.termLimitForm.get('plazoDias');
  }
  
  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

   // evalua tecla a tecla
   validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : true;
  }


  validatenomore30() {
    if (this.termLimitForm.value.plazoDias > 29) {
      this.openDialog("", "<b>El valor ingresado en dias no puede ser superior a 29</b>");
      this.daysField.setValue("");
    }
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  onSubmitTermLimit(){
    if(!this.termLimitForm.valid){
      this.openDialog('', '<b>Por favor ingrese todos los campos obligatorios.</b>')
    } else {
     
      let plazo: number = 0;

      plazo = this.monthsField.value * 30;
      plazo = plazo + this.daysField.value;
      
      if (plazo < this.plazoProyecto) {
        console.log(this.plazoProyecto, plazo);
        this.openDialog('', '<b> El plazo no puede ser menor al del proyecto. </b>')
        return false;
      }else{
        const termLimit : PlazoContratacion = { plazoMeses: this.monthsField.value, plazoDias: this.daysField.value };
        this.guardar.emit(termLimit)
      }
  }

  }
}
