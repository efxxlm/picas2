import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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
  @Output() numReclamacion = new EventEmitter<string>();
  @Output() actuacion = new EventEmitter<string>();
  @Output() numActuacion = new EventEmitter<string>();
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
      this.services.GetControversiaActuacionById(this.controversiaAct).subscribe((a:any)=>{
        this.addressForm.get('resumenReclamacionFiduciaria').setValue(a.resumenPropuestaFiduciaria);
        this.addressForm.get('requereReclamacionComiteTecnico').setValue(true);
        this.addressForm.get('urlSoporte').setValue(a.rutaSoporte);
        this.numReclamacion.emit(localStorage.getItem("numReclamacion"));
        this.actuacion.emit(localStorage.getItem("actuacion"));
        this.numActuacion.emit(a.numeroActuacionFormat);
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
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if ( texto ){
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) { 
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
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
        this.services.CambiarEstadoActuacionSeguimiento(this.controversiaAct,'2').subscribe((data:any)=>{
          
        });
        this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
        this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarTramiteControversia']);
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
        this.services.CambiarEstadoActuacionSeguimiento(this.controversiaAct,'2').subscribe((data:any)=>{
          
        });
        this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
        this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarTramiteControversia']);
      });
    }
  }
}
