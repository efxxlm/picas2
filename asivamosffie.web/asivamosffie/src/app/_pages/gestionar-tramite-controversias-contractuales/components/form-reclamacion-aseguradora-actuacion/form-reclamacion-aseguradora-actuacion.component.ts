import { Component, Input, OnInit } from '@angular/core';
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
export class FormReclamacionAseguradoraActuacionComponent implements OnInit {
  @Input() isEditable;
  @Input() controversiaAct;
  public controversiaID = parseInt(localStorage.getItem("controversiaID"));
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
  constructor(private router: Router, private services: ContractualControversyService, private fb: FormBuilder, public dialog: MatDialog) { }
  ngOnInit(): void {
    if(this.isEditable==true){
      this.addressForm.get('requiereReclamacionAseguradora').setValue(true);
      this.addressForm.get('resumenReclamacionFiduciaria').setValue('prueba');
      this.addressForm.get('requereReclamacionComiteTecnico').setValue(true);
      this.addressForm.get('definitivoyCerrado').setValue(true);
    }
  }
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  maxLength(e: any, n: number) {
    
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n-1, e.editor.getLength());
    }
  }
  textoLimpio(texto,n) {
    if (texto!=undefined) {
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
    let arrayReclam;

    if(this.isEditable==true){
      arrayReclam = { 
        "controversiaActuacionId":this.controversiaAct,
        "resumenPropuestaFiduciaria":this.addressForm.value.resumenReclamacionFiduciaria,
        "esRequiereComiteReclamacion":this.addressForm.value.requereReclamacionComiteTecnico,
        "esprocesoResultadoDefinitivo":true,
        "rutaSoporte":this.addressForm.value.urlSoporte,
        };
      this.services.CreateEditarReclamacion(arrayReclam).subscribe((data:any)=>{
        this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
        this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarTramiteControversia']);
      });
    }
    else{
      arrayReclam = { 
        "resumenPropuestaFiduciaria":this.addressForm.value.resumenReclamacionFiduciaria,
        "esRequiereComiteReclamacion":this.addressForm.value.requereReclamacionComiteTecnico,
        "esprocesoResultadoDefinitivo":true,
        "rutaSoporte":this.addressForm.value.urlSoporte,
        };
      this.services.CreateEditarReclamacion(arrayReclam).subscribe((data:any)=>{
        this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
        this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarTramiteControversia']);
      });
    }
  }
}
