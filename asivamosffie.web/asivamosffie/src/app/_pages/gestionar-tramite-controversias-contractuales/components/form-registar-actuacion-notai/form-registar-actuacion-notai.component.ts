import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-registar-actuacion-notai',
  templateUrl: './form-registar-actuacion-notai.component.html',
  styleUrls: ['./form-registar-actuacion-notai.component.scss']
})
export class FormRegistarActuacionNotaiComponent implements OnInit, OnDestroy {
  @Input() isEditable;
  @Input() idActuacionFromEdit;
  addressForm = this.fb.group({
    estadoAvanceTramite: [null, Validators.required],
    fechaActuacionAdelantada: [null, Validators.required],
    actuacionAdelantada: [null, Validators.required],
    cualOtroActuacionAdelantada: [null],
    proximaActuacionRequerida: [null, Validators.required],
    cualOtroActuacionRequerida: [null],
    diasVencimientoTerminos: [null, Validators.required],
    fechaVencimientoTerminos: [null, Validators.required],
    participacionContratista: [null, Validators.required],
    participacionInterventorContrato: [null, Validators.required],
    participacionSupervisorContrato: [null, Validators.required],
    participacionFiduciaria: [null, Validators.required],
    requiereComiteTecnico: [null, Validators.required],
    observaciones: [null, Validators.required],
    urlSoporte: [null, Validators.required],
    requiereMesaDeTrabajo: [null, Validators.required],
    resultadoDefinitivoyCerrado: [null, Validators.required]
  });
  estadoAvanceTramiteArrayDom = [

  ];
  proximaActuacionRequeridaArrayDom = [

  ];
  actuacionAdelantadaArrayDom = [

  ];
  @Input()controversiaID;
  //public controversiaID = parseInt(localStorage.getItem("controversiaID"));
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
  numReclamacion: any;
  estaEditando = false;
  realizoPeticion: boolean = false;
  constructor(private fb: FormBuilder, public dialog: MatDialog, private services: ContractualControversyService, private common: CommonService, private router: Router) { }

  ngOnInit(): void {
    this.common.listaEstadosControversiaNoTAI().subscribe(rep => {
      this.estadoAvanceTramiteArrayDom = rep;
    });
    this.common.listaActuacionAdelantada().subscribe(rep1 => {
      this.actuacionAdelantadaArrayDom = rep1;
    });
    this.common.listaProximaActuacionRequerida().subscribe(rep2 => {
      this.proximaActuacionRequeridaArrayDom = rep2;
    });
    if (this.isEditable == true) {
      this.estaEditando = true;
      this.addressForm.markAllAsTouched();
      this.services.GetControversiaActuacionById(this.idActuacionFromEdit).subscribe((data: any) => {
        for (let i = 0; i < this.estadoAvanceTramiteArrayDom.length; i++) {
          const avanceTramSelected = this.estadoAvanceTramiteArrayDom.find(t => t.codigo === data.estadoAvanceTramiteCodigo);
          this.addressForm.get('estadoAvanceTramite').setValue(avanceTramSelected);
        }
        this.addressForm.get('fechaActuacionAdelantada').setValue(data.fechaActuacion);
        for (let i = 0; i < this.actuacionAdelantadaArrayDom.length; i++) {
          const actuacionAdelantadaSelected = this.actuacionAdelantadaArrayDom.find(t => t.codigo === data.actuacionAdelantadaCodigo);
          this.addressForm.get('actuacionAdelantada').setValue(actuacionAdelantadaSelected);
        }
        for (let i = 0; i < this.proximaActuacionRequeridaArrayDom.length; i++) {
          const actuacionRequeridaSelected = this.proximaActuacionRequeridaArrayDom.find(t => t.codigo === data.proximaActuacionCodigo);
          this.addressForm.get('proximaActuacionRequerida').setValue(actuacionRequeridaSelected);
        }
        this.addressForm.get('cualOtroActuacionAdelantada').setValue(data.actuacionAdelantadaOtro!== undefined ? data.actuacionAdelantadaOtro : null);
        this.addressForm.get('cualOtroActuacionRequerida').setValue(data.proximaActuacionOtro!== undefined ? data.proximaActuacionOtro : null);
        this.addressForm.get('diasVencimientoTerminos').setValue(data.cantDiasVencimiento.toString());
        this.addressForm.get('fechaVencimientoTerminos').setValue(data.fechaVencimiento);
        this.addressForm.get('participacionContratista').setValue(data.esRequiereContratista);
        this.addressForm.get('participacionInterventorContrato').setValue(data.esRequiereInterventor);
        this.addressForm.get('participacionSupervisorContrato').setValue(data.esRequiereSupervisor);
        this.addressForm.get('participacionFiduciaria').setValue(data.esRequiereFiduciaria);
        this.addressForm.get('requiereComiteTecnico').setValue(data.esRequiereComite);
        this.addressForm.get('observaciones').setValue(data.observaciones!== undefined ? data.observaciones : null);
        this.addressForm.get('urlSoporte').setValue(data.rutaSoporte);
        this.addressForm.get('requiereMesaDeTrabajo').setValue(data.esRequiereMesaTrabajo);
        this.addressForm.get('resultadoDefinitivoyCerrado').setValue(data.esprocesoResultadoDefinitivo);
        this.numReclamacion = data.numeroReclamacion;
      });
    }
  }
  ngOnDestroy(): void {
    if (this.addressForm.dirty === true && this.realizoPeticion === false) {
      this.openDialogConfirmar('', '¿Desea guardar la información registrada?');
    }
  }
  openDialogConfirmar(modalTitle: string, modalText: string) {
    const confirmarDialog = this.dialog.open(ModalDialogComponent, {
      width: '30em',
      data: { modalTitle, modalText, siNoBoton: true }
    });

    confirmarDialog.afterClosed()
      .subscribe(response => {
        if (response === true) {
          this.onSubmit();
        }
      });
  };
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  maxLength(e: any, n: number) {

    if (e.editor.getLength() > n) {
      e.editor.deleteText(n - 1, e.editor.getLength());
    }
  }
  textoLimpio(texto, n) {
    if (texto != undefined) {
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
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    let actuacionTaiArray;
    let completo: boolean;
    if (this.addressForm.valid) {
      completo = true;
    }
    else {
      completo = false;
    }
    let estadoAvanTramite;
    if (this.addressForm.value.estadoAvanceTramite != undefined || this.addressForm.value.estadoAvanceTramite != null) {
      if (!this.addressForm.value.estadoAvanceTramite.codigo) {
        estadoAvanTramite = "pendiente";
      }
      else {
        estadoAvanTramite = this.addressForm.value.estadoAvanceTramite.codigo;
      }
    }
    let actuacionAdelantada;
    if (this.addressForm.value.actuacionAdelantada != undefined || this.addressForm.value.actuacionAdelantada != null) {
      if (!this.addressForm.value.actuacionAdelantada.codigo) {
        actuacionAdelantada = "pendiente";
      }
      else {
        actuacionAdelantada = this.addressForm.value.actuacionAdelantada.codigo;
      }
    }
    let proxActuacion;
    if (this.addressForm.value.proximaActuacionRequerida != undefined || this.addressForm.value.proximaActuacionRequerida != null) {
      if (!this.addressForm.value.proximaActuacionRequerida.codigo) {
        proxActuacion = "pendiente";
      }
      else {
        proxActuacion = this.addressForm.value.proximaActuacionRequerida.codigo;
      }
    }
    if (this.isEditable == true) {
      actuacionTaiArray = {
        "ControversiaContractualId": this.controversiaID,
        "ActuacionAdelantadaCodigo": actuacionAdelantada,
        "ActuacionAdelantadaOtro": this.addressForm.value.cualOtroActuacionAdelantada,
        "ProximaActuacionCodigo": proxActuacion,
        "ProximaActuacionOtro": this.addressForm.value.cualOtroActuacionRequerida,
        "Observaciones": this.addressForm.value.observaciones,
        "ResumenPropuestaFiduciaria": "",
        "RutaSoporte": this.addressForm.value.urlSoporte,
        "EstadoAvanceTramiteCodigo": estadoAvanTramite,
        "CantDiasVencimiento": this.addressForm.value.diasVencimientoTerminos,
        "FechaVencimiento": this.addressForm.value.fechaVencimientoTerminos,
        "FechaActuacion": this.addressForm.value.fechaActuacionAdelantada,
        "EsRequiereContratista": this.addressForm.value.participacionContratista,
        "EsRequiereInterventor": this.addressForm.value.participacionInterventorContrato,
        "EsRequiereSupervisor": this.addressForm.value.participacionSupervisorContrato,
        "EsRequiereJuridico": "",
        "EsRequiereFiduciaria": this.addressForm.value.participacionFiduciaria,
        "EsRequiereComite": this.addressForm.value.requiereComiteTecnico,
        "EsRequiereAseguradora": "",
        "EsRequiereComiteReclamacion": "",
        "EsprocesoResultadoDefinitivo": this.addressForm.value.resultadoDefinitivoyCerrado,
        "EsRequiereMesaTrabajo": this.addressForm.value.requiereMesaDeTrabajo,
        "Eliminado": false,
        "ControversiaActuacionId": parseInt(this.idActuacionFromEdit)
      }
    }
    else {
      actuacionTaiArray = {
        "ControversiaContractualId": this.controversiaID,
        "ActuacionAdelantadaCodigo": actuacionAdelantada,
        "ActuacionAdelantadaOtro": this.addressForm.value.cualOtroActuacionAdelantada,
        "ProximaActuacionCodigo": proxActuacion,
        "ProximaActuacionOtro": this.addressForm.value.cualOtroActuacionRequerida,
        "Observaciones": this.addressForm.value.observaciones,
        "ResumenPropuestaFiduciaria": "",
        "RutaSoporte": this.addressForm.value.urlSoporte,
        "EstadoAvanceTramiteCodigo": estadoAvanTramite,
        "CantDiasVencimiento": this.addressForm.value.diasVencimientoTerminos,
        "FechaVencimiento": this.addressForm.value.fechaVencimientoTerminos,
        "FechaActuacion": this.addressForm.value.fechaActuacionAdelantada,
        "EsRequiereContratista": this.addressForm.value.participacionContratista,
        "EsRequiereInterventor": this.addressForm.value.participacionInterventorContrato,
        "EsRequiereSupervisor": this.addressForm.value.participacionSupervisorContrato,
        "EsRequiereJuridico": "",
        "EsRequiereFiduciaria": this.addressForm.value.participacionFiduciaria,
        "EsRequiereComite": this.addressForm.value.requiereComiteTecnico,
        "EsRequiereAseguradora": "",
        "EsRequiereComiteReclamacion": "",
        "EsprocesoResultadoDefinitivo": this.addressForm.value.resultadoDefinitivoyCerrado,
        "EsRequiereMesaTrabajo": this.addressForm.value.requiereMesaDeTrabajo,
        "Eliminado": false
      }
    }
    this.services.CreateEditControversiaOtros(actuacionTaiArray).subscribe((data: any) => {
      if (data.isSuccessful == true) {
        this.services.CambiarEstadoActuacionSeguimiento(data.data.controversiaActuacionId,"1").subscribe((data0:any)=>{
        });
        this.openDialog("", `<b>${data.message}</b>`);
        this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarTramiteControversia',this.controversiaID]);
      }
      else {
        this.openDialog("", `<b>${data.message}</b>`);
      }
    });
  }

}
