import { Component, EventEmitter, Input, OnInit, Output, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-reclamacion-aseguradora-actuacion',
  templateUrl: './form-reclamacion-aseguradora-actuacion.component.html',
  styleUrls: ['./form-reclamacion-aseguradora-actuacion.component.scss']
})
export class FormReclamacionAseguradoraActuacionComponent implements OnInit, OnDestroy {
  @Input() isEditable;
  @Input() controversiaAct;
  @Input() controversiaID;
  @Output() numReclamacion = new EventEmitter<string>();
  @Output() actuacion = new EventEmitter<string>();
  @Output() numActuacion = new EventEmitter<string>();
  //public controversiaID = parseInt(localStorage.getItem("controversiaID"));
  addressForm = this.fb.group({
    resumenReclamacionFiduciaria: [null, Validators.required],
    requereReclamacionComiteTecnico: [null, Validators.required],
    urlSoporte: [null, Validators.required],
  });
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
  constructor(private router: Router, private services: ContractualControversyService, private fb: FormBuilder, public dialog: MatDialog) { }
  ngOnInit(): void {
    if(this.isEditable==true){
      this.estaEditando = true;
      this.addressForm.markAllAsTouched();
      this.services.GetControversiaActuacionById(this.controversiaAct).subscribe((a:any)=>{
        this.addressForm.get('resumenReclamacionFiduciaria').setValue(a.resumenPropuestaFiduciaria!== undefined ? a.resumenPropuestaFiduciaria : null);
        this.addressForm.get('requereReclamacionComiteTecnico').setValue(a.esRequiereComiteReclamacion);
        this.addressForm.get('urlSoporte').setValue(a.rutaSoporte);
        this.numReclamacion.emit(localStorage.getItem("numReclamacion"));
        this.actuacion.emit(localStorage.getItem("actuacion"));
        this.numActuacion.emit(a.numeroActuacionFormat);
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
    // console.log(this.addressForm.value);
    let arrayReclam;
    let codeState;
    if(this.isEditable==true){
      arrayReclam = { 
        "controversiaActuacionId":this.controversiaAct,
        "controversiaContractualId":this.controversiaID,
        "resumenPropuestaFiduciaria":this.addressForm.value.resumenReclamacionFiduciaria,
        "esRequiereComiteReclamacion":this.addressForm.value.requereReclamacionComiteTecnico,
        "rutaSoporte":this.addressForm.value.urlSoporte,
        };
      this.services.CreateEditarReclamacion(arrayReclam).subscribe((data:any)=>{
        this.services.CambiarEstadoActuacionReclamacion(this.controversiaAct,'2').subscribe((data:any)=>{
          
        });
        this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
        this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarTramiteControversia',this.controversiaID]);
      });
    }
    else{
      arrayReclam = { 
        "controversiaActuacionId":this.controversiaAct,
        "controversiaContractualId":this.controversiaID,
        "resumenPropuestaFiduciaria":this.addressForm.value.resumenReclamacionFiduciaria,
        "esRequiereComiteReclamacion":this.addressForm.value.requereReclamacionComiteTecnico,
        "rutaSoporte":this.addressForm.value.urlSoporte,
        };
      this.services.CreateEditarReclamacion(arrayReclam).subscribe((data:any)=>{
        this.services.CambiarEstadoActuacionReclamacion(this.controversiaAct,'2').subscribe((data:any)=>{
          
        });
        this.realizoPeticion = true;
        this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
        this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarTramiteControversia',this.controversiaID]);
      });
    }
  }
}
