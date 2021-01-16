import { Component, Input, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-registrar-mesa-de-trabajo-act',
  templateUrl: './form-registrar-mesa-de-trabajo-act.component.html',
  styleUrls: ['./form-registrar-mesa-de-trabajo-act.component.scss']
})
export class FormRegistrarMesaDeTrabajoActComponent implements OnInit {

  @Input() isEditable;
  
  public actuacionID = parseInt(localStorage.getItem("reclamacionID"));
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
  constructor(private fb: FormBuilder, public dialog: MatDialog,private services: ContractualControversyService, private common: CommonService,private router: Router) { }

  ngOnInit(): void {
    if (this.isEditable == true) {
      //this.services.GetActuacionMesaByActuacionMesaId()
      this.addressForm.get('estadoAvanceTramite').setValue('1');
      this.addressForm.get('fechaActuacionAdelantada').setValue('10/10/2020');
      this.addressForm.get('actuacionAdelantada').setValue('Pruebas');
      this.addressForm.get('proximaActuacionRequerida').setValue('Pruebas');
      this.addressForm.get('diasVencimientoTerminos').setValue('3');
      this.addressForm.get('resultadoDefinitivo').setValue(true);
    }
    this.common.listaEstadoAvanceMesaTrabajo().subscribe(a=>{
      this.estadoAvanceTramiteArray = a;
    });
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
        "ControversiaActuacionMesaSeguimientoId": 1,
        "ControversiaActuacionMesaId": this.actuacionID,
        "estadoAvanceMesaCodigo": this.addressForm.value.estadoAvanceTramite,
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
        "ControversiaActuacionMesaSeguimientoId": 0,
        "ControversiaActuacionMesaId": this.actuacionID,
        "estadoAvanceMesaCodigo": this.addressForm.value.estadoAvanceTramite,
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
    this.services.CreateEditarActuacionMesa(mesaTrabajoArray).subscribe((data:any)=>{
      if (data.isSuccessful==true){
        this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
        this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarMesaTrabajo']);
      }
      else{
        this.openDialog('', data.message);
      }
    });
  }

}
