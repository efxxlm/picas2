import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-registrar-mesa-de-trabajo',
  templateUrl: './form-registrar-mesa-de-trabajo.component.html',
  styleUrls: ['./form-registrar-mesa-de-trabajo.component.scss']
})
export class FormRegistrarMesaDeTrabajoComponent implements OnInit {
  @Input() isEditable;
  @Input() idActuacion;
  @Input() idSeguimientoMesa;
  public idMesadeTrabajo = parseInt(localStorage.getItem('idMesa'));
  addressForm = this.fb.group({
    estadoAvanceTramite: [null, Validators.required],
    fechaActuacionAdelantada: [null, Validators.required],
    actuacionAdelantada: [null, Validators.required],
    proximaActuacionRequerida: [null, Validators.required],
    diasVencimientoTerminos: [null, Validators.required],
    fechaVencimientoTerminos: [null, Validators.required],
    observaciones: [null, Validators.required],
    resultadoDefinitivo: [null, Validators.required],
    urlSoporte: [null, Validators.required]
  });
  estadoAvanceTramiteArray = [

  ];
  actuacionAdelantadaArray = [
    { name: 'Otro', value: '1' },
  ];
  proximaActuacionRequeridaArray = [
    { name: 'Firmas', value: '1' },
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
  constructor(private fb: FormBuilder, public dialog: MatDialog, private services: ContractualControversyService, private common: CommonService, private router: Router) {
    this.common.listaEstadoAvanceMesaTrabajo().subscribe(a => {
      this.estadoAvanceTramiteArray = a;
    });
  }

  ngOnInit(): void {
    if (this.isEditable == true) {
      this.services.GetMesaByMesaId(this.idMesadeTrabajo).subscribe((data: any) => {
        for (let i = 0; i < this.estadoAvanceTramiteArray.length; i++) {
          const estadoAvanceTramiteSelected = this.estadoAvanceTramiteArray.find(p => p.codigo === data.estadoAvanceMesaCodigo);
          this.addressForm.get('estadoAvanceTramite').setValue(estadoAvanceTramiteSelected);
        }
        this.addressForm.get('fechaActuacionAdelantada').setValue(data.fechaActuacionAdelantada);
        this.addressForm.get('actuacionAdelantada').setValue(data.actuacionAdelantada);
        this.addressForm.get('proximaActuacionRequerida').setValue(data.proximaActuacionRequerida);
        this.addressForm.get('diasVencimientoTerminos').setValue(data.cantDiasVencimiento);
        this.addressForm.get('fechaVencimientoTerminos').setValue(data.fechaVencimiento);
        this.addressForm.get('observaciones').setValue(data.observaciones);
        this.addressForm.get('resultadoDefinitivo').setValue(data.resultadoDefinitivo);
        this.addressForm.get('urlSoporte').setValue(data.rutaSoporte);
      });
    }
  }
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
    console.log(this.addressForm.value);
    let mesaTrabajoArray;
    if (this.isEditable == true) {
      mesaTrabajoArray =
      {
        "controversiaActuacionMesaId": this.idMesadeTrabajo,
        "controversiaActuacionId": parseInt(this.idActuacion),
        "estadoAvanceMesaCodigo": this.addressForm.value.estadoAvanceTramite.codigo,
        "fechaActuacionAdelantada": this.addressForm.value.fechaActuacionAdelantada,
        "actuacionAdelantada": this.addressForm.value.actuacionAdelantada,
        "proximaActuacionRequerida": this.addressForm.value.proximaActuacionRequerida,
        "cantDiasVencimiento": this.addressForm.value.diasVencimientoTerminos,
        "fechaVencimiento": this.addressForm.value.fechaVencimientoTerminos,
        "observaciones": this.addressForm.value.observaciones,
        "resultadoDefinitivo": this.addressForm.value.resultadoDefinitivo,
        "rutaSoporte": this.addressForm.value.urlSoporte
      }
    }
    else {
      mesaTrabajoArray =
      {
        "controversiaActuacionId": parseInt(this.idActuacion),
        "estadoAvanceMesaCodigo": this.addressForm.value.estadoAvanceTramite.codigo,
        "fechaActuacionAdelantada": this.addressForm.value.fechaActuacionAdelantada,
        "actuacionAdelantada": this.addressForm.value.actuacionAdelantada,
        "proximaActuacionRequerida": this.addressForm.value.proximaActuacionRequerida,
        "cantDiasVencimiento": this.addressForm.value.diasVencimientoTerminos,
        "fechaVencimiento": this.addressForm.value.fechaVencimientoTerminos,
        "observaciones": this.addressForm.value.observaciones,
        "resultadoDefinitivo": this.addressForm.value.resultadoDefinitivo,
        "rutaSoporte": this.addressForm.value.urlSoporte
      }
    }
    this.services.CreateEditarMesa(mesaTrabajoArray).subscribe((data: any) => {
      if (data.isSuccessful == true) {
        this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
        this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarTramiteControversia']);
      }
      else {
        this.openDialog('', data.message);
      }
    });
  }

}
