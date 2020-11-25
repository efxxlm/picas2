import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
@Component({
  selector: 'app-form-registrar-controvrs-accord',
  templateUrl: './form-registrar-controvrs-accord.component.html',
  styleUrls: ['./form-registrar-controvrs-accord.component.scss']
})
export class FormRegistrarControvrsAccordComponent implements OnInit {
  @Input() isEditable;
  @Input() contratoId;
  @Input() idControversia;

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
    { name: 'Terminación anticipada por imposibilidad de ejecución (TAIE) a solicitud del contratista', value: '2' },
    { name: 'Arreglo Directo (AD) a solicitud del contratista', value: '3' },
    { name: 'Otras controversias contractuales (OCC) a solicitud del contratista', value: '4' },
    { name: 'Terminación anticipada por imposibilidad de ejecución (TAIE) a solicitud del contratante', value: '5' },
    { name: 'Arreglo Directo (AD) a solicitud del contratante', value: '6' },
    { name: 'Otras controversias contractuales (OCC) a solicitud del contratante', value: '7' },
  ];
  motivosSolicitudArray: Dominio[] = [];
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
  numeroSolicitud: any;
  userCreation: any;
  constructor(private router: Router, private fb: FormBuilder, public dialog: MatDialog, private services: ContractualControversyService, private common: CommonService) { }
  ngOnInit(): void {
    this.loadMotivosList();
    if (this.isEditable == true) {
      this.services.GetControversiaContractualById(this.idControversia).subscribe((resp:any)=>{
        const controversiaSelected = this.tipoControversiaArray.find( t => t.value === resp.tipoControversiaCodigo);
        this.addressForm.get('tipoControversia').setValue(controversiaSelected);
        this.addressForm.get('fechaSolicitud').setValue(resp.fechaSolicitud);
        this.addressForm.get('motivosSolicitud').setValue('1');
        this.addressForm.get('fechaComitePretecnico').setValue(resp.fechaComitePreTecnico);
        this.addressForm.get('conclusionComitePretecnico').setValue(resp.conclusionComitePreTecnico);
        this.addressForm.get('procedeSolicitud').setValue(resp.esProcede);
        this.addressForm.get('requeridoComite').setValue(false);
        this.numeroSolicitud = resp.numeroSolicitudFormat;
        this.userCreation = resp.usuarioCreacion;
      })
    }
  }
  loadMotivosList(){
    this.common.listaMotivosSolicitudControversiaContractual().subscribe(data=>{
      this.motivosSolicitudArray = data;
    });
  }
  getvalues(values:Dominio[]) {
    const buenManejo = values.find(value => value.codigo == "1");
    const garantiaObra = values.find(value => value.codigo == "2");
    const pCumplimiento = values.find(value => value.codigo == "3");
    const polizasYSeguros = values.find(value => value.codigo == "4");


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
      let formArrayTai
      if(this.isEditable == true){
        formArrayTai = {
          "TipoControversiaCodigo": this.addressForm.value.tipoControversia.value,
          "FechaSolicitud": this.addressForm.value.fechaSolicitud,
          "NumeroSolicitud": this.numeroSolicitud,
          "EstadoCodigo": "1",
          "EsCompleto": false,
          "numeroSolicitudFormat": this.addressForm.value.motivosSolicitud.value,
          "ContratoId": this.contratoId,
          "ConclusionComitePreTecnico": this.addressForm.value.conclusionComitePretecnico,
          "UsuarioCreacion": "us cre",
          "UsuarioModificacion": "us mod",
          "FechaComitePreTecnico": this.addressForm.value.fechaComitePretecnico,
          "EsProcede": this.addressForm.value.procedeSolicitud,
          "EsRequiereComite": this.addressForm.value.requeridoComite,
          "ControversiaContractualId":this.idControversia
        };
      }
      else{
        formArrayTai = {
          "TipoControversiaCodigo": this.addressForm.value.tipoControversia.value,
          "FechaSolicitud": this.addressForm.value.fechaSolicitud,
          "NumeroSolicitud": this.numeroSolicitud,
          "EstadoCodigo": "1",
          "EsCompleto": false,
          "numeroSolicitudFormat": this.addressForm.value.motivosSolicitud.value,
          "ContratoId": this.contratoId,
          "ConclusionComitePreTecnico": this.addressForm.value.conclusionComitePretecnico,
          "UsuarioCreacion": this.userCreation,
          "UsuarioModificacion": "us mod",
          "FechaComitePreTecnico": this.addressForm.value.fechaComitePretecnico,
          "EsProcede": this.addressForm.value.procedeSolicitud,
          "EsRequiereComite": this.addressForm.value.requeridoComite
        };
      }
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
