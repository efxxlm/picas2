import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-registrar-controvrs-accord',
  templateUrl: './form-registrar-controvrs-accord.component.html',
  styleUrls: ['./form-registrar-controvrs-accord.component.scss']
})
export class FormRegistrarControvrsAccordComponent implements OnInit {
  @Input() isEditable;
  @Input() contratoId;

  addressForm = this.fb.group({
    tipoControversia: [null, Validators.required],
    fechaSolicitud: [null, Validators.required],
    motivosSolicitud: [null, Validators.required],
    fechaComitePretecnico: [null, Validators.required],
    conclusionComitePretecnico: [null, Validators.required],
    procedeSolicitud: [null, Validators.required],
    requeridoComite: [null, Validators.required],
    fechaRadicadoSAC: [null, Validators.required],
    numeroRadicadoSAC: [null, Validators.required],
    resumenJustificacionSolicitud: [null, Validators.required]
  });
  tipoControversiaArray = [
    { name: 'Terminación anticipada por incumplimiento (TAI)', value: '1' },
    { name: 'Terminación anticipada por imposibilidad de ejecución (TAIE)', value: '2' },
    { name: 'Arreglo Directo (AD)', value: '3' },
    { name: 'Otras controversias contractuales (OCC)', value: '4' },
  ];
  motivosSolicitudArray = [
    { name: 'Incuplimiento de contratista de obra', value: '1' },
    { name: 'Incuplimiento', value: '2' }
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
  constructor(private router: Router, private fb: FormBuilder, public dialog: MatDialog, private services: ContractualControversyService) { }
  ngOnInit(): void {
    if (this.isEditable == true) {
      this.addressForm.get('tipoControversia').setValue('1');
      this.addressForm.get('fechaSolicitud').setValue('20/08/2020');
      this.addressForm.get('motivosSolicitud').setValue('1');
      this.addressForm.get('fechaComitePretecnico').setValue('10/10/2020');
      this.addressForm.get('conclusionComitePretecnico').setValue('No funciona la obra');
      this.addressForm.get('procedeSolicitud').setValue(true);
      this.addressForm.get('requeridoComite').setValue(false);
    }
  }
  // evalua tecla a tecla
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
    console.log(this.addressForm.value);
    alert(this.addressForm.value.tipoControversia.value);
    if (this.addressForm.value.tipoControversia.value == '1') {
      let formArrayTai = {
        "TipoControversiaCodigo": this.addressForm.value.tipoControversia.value,
        "FechaSolicitud": this.addressForm.value.fechaSolicitud,
        "NumeroSolicitud": "XXXww",
        "EstadoCodigo": "1",
        "EsCompleto": false,
        "SolicitudId": this.addressForm.value.motivosSolicitud.value,
        "ContratoId": this.contratoId,
        "ConclusionComitePreTecnico": this.addressForm.value.conclusionComitePretecnico,
        "UsuarioCreacion": "us cre",
        "UsuarioModificacion": "us mod",
        "FechaComitePreTecnico": this.addressForm.value.fechaComitePretecnico,
        "EsProcede": this.addressForm.value.procedeSolicitud,
        "EsRequiereComite": this.addressForm.value.requeridoComite
      };
      this.services.CreateEditarControversiaTAI(formArrayTai).subscribe(resp_0 => {
        if(resp_0.isSuccessful==true){
          this.openDialog('', 'La información ha sido guardada exitosamente.');
          this.router.navigate(['/gestionarTramiteControversiasContractuales'])
        }
        else{
          this.openDialog('', 'La información ha sido guardada exitosamente.');
        }
      });
    }
    else {
      console.log('servicio que no va el TAI');
    }
    
  }

}
