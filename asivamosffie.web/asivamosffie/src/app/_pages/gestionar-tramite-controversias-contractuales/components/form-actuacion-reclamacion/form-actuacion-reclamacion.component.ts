import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-actuacion-reclamacion',
  templateUrl: './form-actuacion-reclamacion.component.html',
  styleUrls: ['./form-actuacion-reclamacion.component.scss']
})
export class FormActuacionReclamacionComponent implements OnInit {
  @Input() isEditable;
  public controversiaID = parseInt(localStorage.getItem("controversiaID"));
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
  estadoAvanceTramiteArrayDom: Dominio[] = [];
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
  constructor( private services: ContractualControversyService, private common: CommonService, private fb: FormBuilder, public dialog: MatDialog, private router: Router) { }

  ngOnInit(): void {
    if(this.isEditable==true){
      if (this.isEditable == true) {
        this.services.GetControversiaActuacionById(1).subscribe((data:any)=>{
          const avanceTramSelected = this.estadoAvanceTramiteArrayDom.find(t => t.codigo === data.estadoAvanceTramiteCodigo);
          this.addressForm.get('estadoAvanceTramite').setValue(avanceTramSelected);
          this.addressForm.get('fechaActuacionAdelantada').setValue(data.fechaActuacion);
          this.addressForm.get('actuacionAdelantada').setValue(data.actuacionAdelantadaCodigo);
          this.addressForm.get('proximaActuacionRequerida').setValue(data.proximaActuacionCodigo);
          this.addressForm.get('cualOtro').setValue(data.actuacionAdelantadaOtro);
          this.addressForm.get('diasVencimientoTerminos').setValue(data.cantDiasVencimiento.toString());
          this.addressForm.get('fechaVencimientoTerminos').setValue(data.fechaVencimiento);
          this.addressForm.get('participacionContratista').setValue(data.esRequiereContratista);
          this.addressForm.get('participacionInterventorContrato').setValue(data.esRequiereInterventor);
          this.addressForm.get('participacionSupervisorContrato').setValue(data.esRequiereSupervisor);
          this.addressForm.get('participacionFiduciaria').setValue(data.esRequiereFiduciaria);
          this.addressForm.get('requiereComiteTecnico').setValue(data.esRequiereComite);
          this.addressForm.get('observaciones').setValue(data.observaciones);
          this.addressForm.get('urlSoporte').setValue(data.rutaSoporte);
        });
      }
    }
    this.common.listaEstadosAvanceTramite().subscribe(rep => {
      this.estadoAvanceTramiteArrayDom = rep;
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
      if(data.isSuccessful==true){
        this.openDialog("",data.message);
        this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarReclamoAseguradora']);
      }
      else{
        this.openDialog("",data.message);
      }
    });
  }
}
