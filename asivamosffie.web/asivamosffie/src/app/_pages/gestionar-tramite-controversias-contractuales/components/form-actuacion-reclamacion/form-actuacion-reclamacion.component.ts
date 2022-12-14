import { Component, EventEmitter, Input, OnInit, Output, OnDestroy } from '@angular/core';
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
export class FormActuacionReclamacionComponent implements OnInit, OnDestroy {
  @Input() isEditable;
  @Input() controversiaID;
  @Input() reclamacionID;
  @Input() idReclamacionActuacion;
  @Output() codRecalamacion = new EventEmitter<string>();
  @Output() codReclamacionActuacion = new EventEmitter<string>();
  /*
  public controversiaID = parseInt(localStorage.getItem("controversiaID"));
  public reclamacionID = parseInt(localStorage.getItem("reclamacionID"));
  */
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
  estadoAvanceTramiteArrayDom = [
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
  estaEditando = false;
  realizoPeticion: boolean = false;
  constructor(private services: ContractualControversyService, private common: CommonService, private fb: FormBuilder, public dialog: MatDialog, private router: Router) { }

  ngOnInit(): void {
    this.common.listaEstadosAvanceReclamacion().subscribe(rep => {
      this.estadoAvanceTramiteArrayDom = rep;
    });
    if (this.isEditable == true) {
      this.estaEditando = true;
      this.addressForm.markAllAsTouched();
      this.services.GetActuacionSeguimientoById(this.idReclamacionActuacion).subscribe((data: any) => {
        for (let i = 0; i < this.estadoAvanceTramiteArrayDom.length; i++) {
          const avanceTramSelected = this.estadoAvanceTramiteArrayDom.find(t => t.codigo === data.estadoReclamacionCodigo);
          console.log(this.estadoAvanceTramiteArrayDom.filter(t => t.codigo === data.estadoReclamacionCodigo));
          this.addressForm.get('estadoAvanceTramite').setValue(avanceTramSelected);
        }
        this.addressForm.get('fechaActuacionAdelantada').setValue(data.fechaActuacionAdelantada);
        this.addressForm.get('actuacionAdelantada').setValue(data.actuacionAdelantada !== undefined ? data.actuacionAdelantada : null);
        this.addressForm.get('proximaActuacionRequerida').setValue(data.proximaActuacion !== undefined ? data.proximaActuacion : null);
        this.addressForm.get('diasVencimientoTerminos').setValue(data.cantDiasVencimiento);
        this.addressForm.get('fechaVencimientoTerminos').setValue(data.fechaVencimiento);
        this.addressForm.get('definitvoAseguradora').setValue(data.esResultadoDefinitivo);
        this.addressForm.get('observaciones').setValue(data.observaciones !== undefined ? data.observaciones : null);
        this.addressForm.get('urlSoporte').setValue(data.rutaSoporte);
        this.codRecalamacion.emit(data.numeroReclamacion);
        this.codReclamacionActuacion.emit(data.numeroActuacionReclamacion);
      });
    }
  }
  ngOnDestroy(): void {
    if (this.addressForm.dirty === true && this.realizoPeticion === false) {
      this.openDialogConfirmar('', '??Desea guardar la informaci??n registrada?');
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
    if (this.isEditable == true) {
      actuacionTaiArray = {
        "ControversiaActuacionId": this.reclamacionID,
        "EstadoDerivadaCodigo": '1',
        "SeguimientoCodigo": true,
        "EstadoReclamacionCodigo": this.addressForm.value.estadoAvanceTramite.codigo,
        "ActuacionAdelantada": this.addressForm.value.actuacionAdelantada,
        "ProximaActuacion": this.addressForm.value.proximaActuacionRequerida,
        "Observaciones": this.addressForm.value.observaciones,
        "RutaSoporte": this.addressForm.value.urlSoporte,
        "EsResultadoDefinitivo": this.addressForm.value.definitvoAseguradora,
        "CantDiasVencimiento": this.addressForm.value.diasVencimientoTerminos,
        "FechaActuacionAdelantada": this.addressForm.value.fechaActuacionAdelantada,
        "FechaVencimiento": this.addressForm.value.fechaVencimientoTerminos,
        "ActuacionSeguimientoId": this.idReclamacionActuacion
      }
    }
    else {
      actuacionTaiArray = {
        "ControversiaActuacionId": this.reclamacionID,
        "EstadoDerivadaCodigo": '1',
        "SeguimientoCodigo": true,
        "EstadoReclamacionCodigo": this.addressForm.value.estadoAvanceTramite.codigo,
        "ActuacionAdelantada": this.addressForm.value.actuacionAdelantada,
        "ProximaActuacion": this.addressForm.value.proximaActuacionRequerida,
        "Observaciones": this.addressForm.value.observaciones,
        "RutaSoporte": this.addressForm.value.urlSoporte,
        "EsResultadoDefinitivo": this.addressForm.value.definitvoAseguradora,
        "CantDiasVencimiento": this.addressForm.value.diasVencimientoTerminos,
        "FechaActuacionAdelantada": this.addressForm.value.fechaActuacionAdelantada,
        "FechaVencimiento": this.addressForm.value.fechaVencimientoTerminos
      }
    }
    this.services.CreateEditarActuacionReclamacion(actuacionTaiArray).subscribe((data: any) => {
      if (data.isSuccessful == true) {
        this.realizoPeticion = true;
        this.openDialog("", data.message);
        this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarReclamoAseguradora', this.controversiaID, this.reclamacionID]);
      }
      else {
        this.openDialog("", data.message);
      }
    });
  }
}
