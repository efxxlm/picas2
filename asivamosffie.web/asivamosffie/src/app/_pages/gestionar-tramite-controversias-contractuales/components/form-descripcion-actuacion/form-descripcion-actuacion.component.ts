import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
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
  estadoAvanceTramiteArrayDom: Dominio[] = [];
  proximaActuacionRequeridaArrayDom: Dominio[] = [];
  actuacionAdelantadaArrayDom: Dominio[] = [];

  public controversiaID = parseInt(localStorage.getItem("controversiaID"));
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

  constructor(private fb: FormBuilder, public dialog: MatDialog, private services: ContractualControversyService, private common: CommonService) { }

  ngOnInit(): void {
    if (this.isEditable == true) {
      this.addressForm.get('estadoAvanceTramite').setValue('1');
      this.addressForm.get('fechaActuacionAdelantada').setValue('10/10/2020');
      this.addressForm.get('actuacionAdelantada').setValue('1');
      this.addressForm.get('proximaActuacionRequerida').setValue('1');
      this.addressForm.get('cualOtro').setValue('Alguna observacion');
      this.addressForm.get('diasVencimientoTerminos').setValue('3');
      this.addressForm.get('participacionContratista').setValue(true);
    }
    this.common.listaEstadosAvanceTramite().subscribe(rep => {
      this.estadoAvanceTramiteArrayDom = rep;
    });
    this.common.listaActuacionAdelantada().subscribe(rep1=>{
      this.actuacionAdelantadaArrayDom = rep1;
    });
    this.common.listaProximaActuacionRequerida().subscribe(rep2 => {
      this.proximaActuacionRequeridaArrayDom = rep2;
    });
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
    if (this.isEditable == true) {
      actuacionTaiArray = {
        "ControversiaContractualId": this.controversiaID,
        "ActuacionAdelantadaCodigo": this.addressForm.value.actuacionAdelantada.codigo,
        "ActuacionAdelantadaOtro": "",
        "ProximaActuacionCodigo": this.addressForm.value.proximaActuacionRequerida.codigo,
        "ProximaActuacionOtro": this.addressForm.value.cualOtro,
        "Observaciones": this.addressForm.value.observaciones,
        "ResumenPropuestaFiduciaria": "",
        "RutaSoporte": this.addressForm.value.urlSoporte,
        "EstadoAvanceTramiteCodigo": this.addressForm.value.estadoAvanceTramite.codigo,
        "FechaCreacion": "2020-3-3",
        "UsuarioCreacion": "US CRE w",
        "UsuarioModificacion": "US MODIF w",
        "EsCompleto": true,
        "CantDiasVencimiento": this.addressForm.value.diasVencimientoTerminos,
        "FechaVencimiento": this.addressForm.value.fechaVencimientoTerminos,
        "FechaActuacion":this.addressForm.value.fechaActuacionAdelantada,
        "EsRequiereContratista":this.addressForm.value.participacionContratista,
        "EsRequiereInterventor": this.addressForm.value.participacionInterventorContrato,
        "EsRequiereSupervisor": this.addressForm.value.participacionSupervisorContrato,
        "EsRequiereJuridico": "",
        "EsRequiereFiduciaria": this.addressForm.value.participacionFiduciaria,
        "EsRequiereComite": this.addressForm.value.requiereComiteTecnico,
        "EsRequiereAseguradora": "",
        "EsRequiereComiteReclamacion": "",
        "EsprocesoResultadoDefinitivo": "",
        "EsRequiereMesaTrabajo": "",
        "Eliminado": false,
        "ControversiaActuacionId": 7
      }
    }
    else {
      actuacionTaiArray = {
        "ControversiaContractualId": this.controversiaID,
        "ActuacionAdelantadaCodigo": this.addressForm.value.actuacionAdelantada.codigo,
        "ActuacionAdelantadaOtro": "",
        "ProximaActuacionCodigo": this.addressForm.value.proximaActuacionRequerida.codigo,
        "ProximaActuacionOtro": this.addressForm.value.cualOtro,
        "Observaciones": this.addressForm.value.observaciones,
        "ResumenPropuestaFiduciaria": "ResumenPropuestaFiduciaria w",
        "RutaSoporte": this.addressForm.value.urlSoporte,
        "EstadoAvanceTramiteCodigo": this.addressForm.value.estadoAvanceTramite.codigo,
        "FechaCreacion": "2020-3-3",
        "UsuarioCreacion": "US CRE w",
        "UsuarioModificacion": "US MODIF w",
        "EsCompleto": true,
        "CantDiasVencimiento": this.addressForm.value.diasVencimientoTerminos,
        "FechaVencimiento": this.addressForm.value.fechaVencimientoTerminos,
        "FechaActuacion":this.addressForm.value.fechaActuacionAdelantada,
        "EsRequiereContratista":this.addressForm.value.participacionContratista,
        "EsRequiereInterventor": this.addressForm.value.participacionInterventorContrato,
        "EsRequiereSupervisor": this.addressForm.value.participacionSupervisorContrato,
        "EsRequiereJuridico": "",
        "EsRequiereFiduciaria": this.addressForm.value.participacionFiduciaria,
        "EsRequiereComite": this.addressForm.value.requiereComiteTecnico,
        "EsRequiereAseguradora": "",
        "EsRequiereComiteReclamacion": "",
        "EsprocesoResultadoDefinitivo": "",
        "EsRequiereMesaTrabajo": "",
        "Eliminado": false
      }
    }
    this.services.CreateEditControversiaOtros(actuacionTaiArray).subscribe((data: any) => {
      
    });
  }
}
