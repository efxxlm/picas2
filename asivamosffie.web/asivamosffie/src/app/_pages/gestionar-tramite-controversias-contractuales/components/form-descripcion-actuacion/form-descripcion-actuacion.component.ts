import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-descripcion-actuacion',
  templateUrl: './form-descripcion-actuacion.component.html',
  styleUrls: ['./form-descripcion-actuacion.component.scss']
})
export class FormDescripcionActuacionComponent implements OnInit {
  @Input() isEditable;
 
  addressForm = this.fb.group({
    estadoAvanceTramite: [null, Validators.required],
    fechaActuacionAdelantada: [null, Validators.required],
    actuacionAdelantada: [null, Validators.required],
    proximaActuacionRequerida: [null, Validators.required],
    cualOtro: [null, Validators.required],
    diasVencimientoTerminos: [null, Validators.required],
    fechaVencimientoTerminos: [null, Validators.required],
    participacionContratista: [null, Validators.required],
    participacionInterventorContrato: [null, Validators.required],
    participacionSupervisorContrato: [null, Validators.required],
    participacionFiduciaria: [null, Validators.required],
    requiereComiteTecnico: [null, Validators.required],
    observaciones: [null, Validators.required],
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

  constructor(  private fb: FormBuilder, public dialog: MatDialog, private services: ContractualControversyService) { }

  ngOnInit(): void {
    if(this.isEditable==true){
      this.addressForm.get('estadoAvanceTramite').setValue('1');
      this.addressForm.get('fechaActuacionAdelantada').setValue('10/10/2020');
      this.addressForm.get('actuacionAdelantada').setValue('1');
      this.addressForm.get('proximaActuacionRequerida').setValue('1');
      this.addressForm.get('cualOtro').setValue('Alguna observacion');
      this.addressForm.get('diasVencimientoTerminos').setValue('3');
      this.addressForm.get('participacionContratista').setValue(true);
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
    let actuacionTaiArray;
    if(this.isEditable==true){
      actuacionTaiArray={
        "ActuacionSeguimientoId": 1,
        "ControversiaActuacionId" :2,
        "SeguimientoCodigo" :false,
        "EstadoReclamacionCodigo" :"2",
       "ActuacionAdelantada":"ActuacionAdelantada 3 UPD",
        "ProximaActuacion": " ProximaActuacion 3 UPD" ,
       "Observaciones": "Observaciones 3 UPD",
       "EstadoDerivadaCodigo" : "2",
        "RutaSoporte" :"RutaSoporte UPD",
        "FechaCreacion" : "2020-01-01",
        "UsuarioCreacion" : "us cre UPPD",
       "UsuarioModificacion" : "us modif UPD",
       "EsResultadoDefinitivo ":true,
       "CantDiasVencimiento": 20
      }
    }
    else{
      actuacionTaiArray={
        "ControversiaActuacionId" :2,
        "SeguimientoCodigo" :false,
        "EstadoReclamacionCodigo" :"2",
       "ActuacionAdelantada":"ActuacionAdelantada 3 UPD",
        "ProximaActuacion": " ProximaActuacion 3 UPD" ,
       "Observaciones": "Observaciones 3 UPD",
       "EstadoDerivadaCodigo" : "2",
        "RutaSoporte" :"RutaSoporte UPD",
        "FechaCreacion" : "2020-01-01",
        "UsuarioCreacion" : "us cre UPPD",
       "UsuarioModificacion" : "us modif UPD",
       "EsResultadoDefinitivo ":true,
       "CantDiasVencimiento": 20
      }
    }
  }
}
