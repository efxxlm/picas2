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
  @Input() idReclamacionActuacion;
  public controversiaID = parseInt(localStorage.getItem("controversiaID"));
  public reclamacionID = parseInt(localStorage.getItem("reclamacionID"));
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
  constructor(private services: ContractualControversyService, private common: CommonService, private fb: FormBuilder, public dialog: MatDialog, private router: Router) { }

  ngOnInit(): void {
    if (this.isEditable == true) {
      this.services.GetActuacionSeguimientoById(this.idReclamacionActuacion).subscribe((data: any) => {
        const avanceTramSelected = this.estadoAvanceTramiteArrayDom.find(t => t.codigo === data.estadoReclamacionCodigo);
        this.addressForm.get('estadoAvanceTramite').setValue(avanceTramSelected);
        this.addressForm.get('fechaActuacionAdelantada').setValue(data.fechaActuacionAdelantada);
        this.addressForm.get('actuacionAdelantada').setValue(data.actuacionAdelantada);
        this.addressForm.get('proximaActuacionRequerida').setValue(data.proximaActuacion);
        this.addressForm.get('diasVencimientoTerminos').setValue(data.cantDiasVencimiento);
        this.addressForm.get('fechaVencimientoTerminos').setValue(data.fechaVencimiento);
        this.addressForm.get('definitvoAseguradora').setValue(data.esResultadoDefinitivo);
        this.addressForm.get('observaciones').setValue(data.observaciones);
        this.addressForm.get('urlSoporte').setValue(data.rutaSoporte);
      });
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
        "ControversiaActuacionId": this.reclamacionID,
        "SeguimientoCodigo": true,
        "EstadoReclamacionCodigo": this.addressForm.value.estadoAvanceTramite.codigo,
        "ActuacionAdelantada": this.addressForm.value.actuacionAdelantada,
        "ProximaActuacion": this.addressForm.value.proximaActuacionRequerida,
        "Observaciones": this.addressForm.value.observaciones,
        "EstadoDerivadaCodigo": "1",
        "RutaSoporte": this.addressForm.value.urlSoporte,
        "FechaCreacion": "2020-01-01",
        "UsuarioCreacion": "us cre",
        "UsuarioModificacion": "us modif",
        "EsResultadoDefinitivo": this.addressForm.value.definitvoAseguradora,
        "CantDiasVencimiento": this.addressForm.value.diasVencimientoTerminos,
        "Eliminado": false,
        "FechaModificacion": "2020-01-01",
        "FechaActuacionAdelantada": this.addressForm.value.fechaActuacionAdelantada,
        "FechaVencimiento": this.addressForm.value.fechaVencimientoTerminos,
        "ActuacionSeguimientoId": this.idReclamacionActuacion
      }
    }
    else {
      actuacionTaiArray = {
        "ControversiaActuacionId": this.reclamacionID,
        "SeguimientoCodigo": true,
        "EstadoReclamacionCodigo": this.addressForm.value.estadoAvanceTramite.codigo,
        "ActuacionAdelantada": this.addressForm.value.actuacionAdelantada,
        "ProximaActuacion": this.addressForm.value.proximaActuacionRequerida,
        "Observaciones": this.addressForm.value.observaciones,
        "EstadoDerivadaCodigo": "1",
        "RutaSoporte": this.addressForm.value.urlSoporte,
        "FechaCreacion": "2020-01-01",
        "UsuarioCreacion": "us cre",
        "UsuarioModificacion": "us modif",
        "EsResultadoDefinitivo": this.addressForm.value.definitvoAseguradora,
        "CantDiasVencimiento": this.addressForm.value.diasVencimientoTerminos,
        "Eliminado": false,
        "FechaModificacion": "",
        "FechaActuacionAdelantada": this.addressForm.value.fechaActuacionAdelantada,
        "FechaVencimiento": this.addressForm.value.fechaVencimientoTerminos
      }
    }
    this.services.CreateEditarActuacionSeguimiento(actuacionTaiArray).subscribe((data: any) => {
      if (data.isSuccessful == true) {
        this.openDialog("", data.message);
        this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarReclamoAseguradora']);
      }
      else {
        this.openDialog("", data.message);
      }
    });
  }
}
