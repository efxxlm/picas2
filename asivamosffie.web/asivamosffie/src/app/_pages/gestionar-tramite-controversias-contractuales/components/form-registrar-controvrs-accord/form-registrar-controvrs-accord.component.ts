import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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
  @Output() estadoSemaforo = new EventEmitter<string>();

  estadoForm: boolean = null;
  addressForm = this.fb.group({
    tipoControversia: [null, Validators.required],
    fechaSolicitud: [null, Validators.required],
    motivosSolicitud: [null, Validators.required],
    fechaComitePretecnico: [null, Validators.required],
    conclusionComitePretecnico: ['', Validators.required],
    procedeSolicitud: [null, Validators.required],
    motivosRechazo: ['', Validators.required],
    requeridoComite: [null, Validators.required],
    fechaRadicadoSAC: ['', Validators.required],
    numeroRadicadoSAC: ['', Validators.required],
    resumenJustificacionSolicitud: ['', Validators.required]
  });
  tipoControversiaArrayDom: Dominio[] = [];
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
  idContrato: any;
  numeroSolicitud: any;
  userCreation: any;
  idMotivo1: number;
  idMotivo2: number;
  idMotivo3: number;
  arrayMotivosLoaded: any[] = [];
  estaCompleto:boolean;

  fechaSesionString: string;
  fechaSesion: Date;

  fechaSesionString2: string;
  fechaSesion2: Date;

  constructor(private router: Router, private fb: FormBuilder, public dialog: MatDialog, private services: ContractualControversyService, private common: CommonService) { }
  ngOnInit(): void {
    this.loadtipoControversias();
    this.loadMotivosList();
    if (this.isEditable == true) {
      this.services.GetControversiaContractualById(this.idControversia).subscribe((resp: any) => {
        const controversiaSelected = this.tipoControversiaArrayDom.find(t => t.codigo === resp.tipoControversiaCodigo);
        this.addressForm.get('tipoControversia').setValue(controversiaSelected);
        this.addressForm.get('fechaSolicitud').setValue(resp.fechaSolicitud);
        this.addressForm.get('fechaComitePretecnico').setValue(resp.fechaComitePreTecnico);
        this.addressForm.get('conclusionComitePretecnico').setValue(resp.conclusionComitePreTecnico);
        this.addressForm.get('procedeSolicitud').setValue(resp.esProcede);
        this.addressForm.get('requeridoComite').setValue(false);
        this.numeroSolicitud = resp.numeroSolicitudFormat;
        this.idContrato = resp.contratoId;
        this.userCreation = resp.usuarioCreacion;
        this.services.GetMotivosSolicitudByControversiaId(this.idControversia).subscribe((data: any) => {
          const motivoSolicitudCod = [];
          this.arrayMotivosLoaded = data;
          if (this.arrayMotivosLoaded.length > 0) {
            const motivosListRead = [this.arrayMotivosLoaded[0].motivoSolicitudCodigo];
            for (let i = 1; i < this.arrayMotivosLoaded.length; i++) {
              const Motivoaux = motivosListRead.push(this.arrayMotivosLoaded[i].motivoSolicitudCodigo);
            }
            for (let i = 0; i < motivosListRead.length; i++) {
              const motivoSeleccionado = this.motivosSolicitudArray.filter(t => t.codigo === motivosListRead[i]);
              if (motivoSeleccionado.length > 0) { motivoSolicitudCod.push(motivoSeleccionado[0]) };
            }
            this.addressForm.get('motivosSolicitud').setValue(motivoSolicitudCod);
            console.log(this.addressForm.value.motivosSolicitud);
            for (let j = 0; j < motivosListRead.length; j++) {
              switch (motivosListRead[j]) {
                case '1':
                  this.idMotivo1 = this.arrayMotivosLoaded[j].controversiaMotivoId;
                  break;
                case '2':
                  this.idMotivo2 = this.arrayMotivosLoaded[j].controversiaMotivoId;
                  break;
                case '3':
                  this.idMotivo3 = this.arrayMotivosLoaded[j].controversiaMotivoId;
                  break;
              }
            }
          }
          this.loadSemaforos();
        });
      });
    }
  }
  loadtipoControversias() {
    this.common.listaTiposDeControversiaContractual().subscribe(data => {
      this.tipoControversiaArrayDom = data;
    });
  }
  loadMotivosList() {
    this.common.listaMotivosSolicitudControversiaContractual().subscribe(data => {
      this.motivosSolicitudArray = data;
    });
  }
  loadMotivosFromService(controversiaId) {

  }
  loadSemaforos() {
    this.estadoSemaforo.emit('sin-diligenciar');
      if (this.addressForm.value.tipoControversia.codigo == '1' && this.addressForm.value.fechaSolicitud != null && this.addressForm.value.motivosSolicitud != null
        && this.addressForm.value.fechaComitePretecnico != null && this.addressForm.value.conclusionComitePretecnico != null && this.addressForm.value.procedeSolicitud != null) {
        this.estadoSemaforo.emit('completo');
        this.estaCompleto=true;
      }
      else {
        this.estadoSemaforo.emit('en-proceso');
        this.estaCompleto=false;
      }
  }


  getvalues(values: Dominio[]) {
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
    let fecha1 = Date.parse(this.addressForm.get( 'fechaSolicitud' ).value);
    this.fechaSesion = new Date(fecha1);
    this.fechaSesionString = `${this.fechaSesion.getFullYear()}-${this.fechaSesion.getMonth() + 1}-${this.fechaSesion.getDate()}`;
    let fecha2 = Date.parse(this.addressForm.get( 'fechaComitePretecnico' ).value);
    this.fechaSesion2 = new Date(fecha2);
    this.fechaSesionString2 = `${this.fechaSesion2.getFullYear()}-${this.fechaSesion2.getMonth() + 1}-${this.fechaSesion2.getDate()}`;
    if (this.addressForm.value.tipoControversia.codigo == '1') {
      if (this.addressForm.value.tipoControversia.codigo == '1' && this.addressForm.value.fechaSolicitud != null && this.addressForm.value.motivosSolicitud != null
      && this.addressForm.value.fechaComitePretecnico != null && this.addressForm.value.conclusionComitePretecnico != null && this.addressForm.value.procedeSolicitud != null){
        this.estaCompleto=true;
      }
      else{
        this.estaCompleto=false;
      }
      let motivosList;
      if (this.addressForm.value.motivosSolicitud != undefined) {
        motivosList = [this.addressForm.value.motivosSolicitud[0].codigo];
        for (let i = 1; i < this.addressForm.value.motivosSolicitud.length; i++) {
          const motivoAux = motivosList.push(this.addressForm.value.motivosSolicitud[i].codigo);
        }
      }
      let formArrayTai;
      let motivosArrayCollected;
      let estadoControversia;
      if(this.estaCompleto==true&&this.addressForm.value.procedeSolicitud==false){
        estadoControversia = "2";
      }
      else if (this.estaCompleto==false&&this.addressForm.value.procedeSolicitud==true&&this.addressForm.value.requeridoComite==null){
        estadoControversia = "3";
      }
      else if (this.estaCompleto==true&&this.addressForm.value.procedeSolicitud==true&&this.addressForm.value.requeridoComite==true){
        estadoControversia = "4";
      }
      else if(this.estaCompleto==false){
        estadoControversia = "1";
      }
      if (this.isEditable == true) {
        formArrayTai = {
          "TipoControversiaCodigo": this.addressForm.value.tipoControversia.codigo,
          "FechaSolicitud": this.fechaSesionString,
          "NumeroSolicitud": this.numeroSolicitud,
          "SolicitudId": 0,
          "NumeroRadicadoSac": 0,
          "RutaSoporte": "",
          "EstadoCodigo": estadoControversia,
          "EsCompleto": this.estaCompleto,
          "ContratoId": this.idContrato,
          "ConclusionComitePreTecnico": this.addressForm.value.conclusionComitePretecnico,
          "MotivoJustificacionRechazo": this.addressForm.value.motivosRechazo,
          "UsuarioCreacion": "us cre",
          "UsuarioModificacion": "us mod",
          "FechaComitePreTecnico": this.fechaSesionString2,
          "EsProcede": this.addressForm.value.procedeSolicitud,
          "EsRequiereComite": this.addressForm.value.requeridoComite,
          "ControversiaContractualId": parseInt(this.idControversia),
          "FechaCreacion":"2020-11-01"
        };
      }
      else {
        formArrayTai = {
          "TipoControversiaCodigo": this.addressForm.value.tipoControversia.codigo,
          "FechaSolicitud": this.fechaSesionString,
          "NumeroSolicitud": "",
          "SolicitudId": 0,
          "NumeroRadicadoSac": 0,
          "RutaSoporte": "",
          "UsuarioCreacion": "",
          "EstadoCodigo": estadoControversia,
          "EsCompleto": this.estaCompleto,
          "ContratoId": this.contratoId,
          "ConclusionComitePreTecnico": this.addressForm.value.conclusionComitePretecnico,
          "MotivoJustificacionRechazo": this.addressForm.value.motivosRechazo,
          "UsuarioModificacion": "us mod",
          "FechaComitePreTecnico": this.fechaSesionString2,
          "EsProcede": this.addressForm.value.procedeSolicitud,
          "EsRequiereComite": this.addressForm.value.requeridoComite,
          "FechaCreacion":"2020-11-01"
        };
      }
      this.services.CreateEditarControversiaTAI(formArrayTai).subscribe(resp_0 => {
        if (resp_0.isSuccessful == true) {
          this.openDialog('', 'La información ha sido guardada exitosamente.');
          if (this.isEditable == true) {
            if (this.addressForm.value.motivosSolicitud != undefined || this.addressForm.value.motivosSolicitud != null) {
              for (let i = 0; i < motivosList.length; i++) {
                switch (motivosList[i]) {
                  case '1':
                    motivosArrayCollected = {
                      "ControversiaContractualId": this.idControversia,
                      'MotivoSolicitudCodigo': '1',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                      "ControversiaMotivoId": this.idMotivo1
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollected).subscribe(r => {
                    });
                    break;
                  case '2':
                    motivosArrayCollected = {
                      "ControversiaContractualId": this.idControversia,
                      'MotivoSolicitudCodigo': '2',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                      "ControversiaMotivoId": this.idMotivo2
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollected).subscribe(r1 => {
                    });
                    break;
                  case '3':
                    motivosArrayCollected = {
                      "ControversiaContractualId": this.idControversia,
                      'MotivoSolicitudCodigo': '3',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                      "ControversiaMotivoId": this.idMotivo3
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollected).subscribe(r2 => {
                    });
                    break;
                }
              }
            }
            this.router.navigate(['/gestionarTramiteControversiasContractuales']);
          }
          else {
            if (this.addressForm.value.motivosSolicitud != undefined || this.addressForm.value.motivosSolicitud != null) {
              for (let i = 0; i < motivosList.length; i++) {
                switch (motivosList[i]) {
                  case '1':
                    motivosArrayCollected = {
                      "ControversiaContractualId": resp_0.data.controversiaContractualId,
                      'MotivoSolicitudCodigo': '1',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollected).subscribe(r => {
                    });
                    break;
                  case '2':
                    motivosArrayCollected = {
                      "ControversiaContractualId": resp_0.data.controversiaContractualId,
                      'MotivoSolicitudCodigo': '2',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollected).subscribe(r1 => {
                    });
                    break;
                  case '3':
                    motivosArrayCollected = {
                      "ControversiaContractualId": resp_0.data.controversiaContractualId,
                      'MotivoSolicitudCodigo': '3',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                      "ControversiaMotivoId": 2
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollected).subscribe(r2 => {
                    });
                    break;
                }
              }
            }
            this.router.navigate(['/gestionarTramiteControversiasContractuales']);
          }
        }
        else {
          this.openDialog('', resp_0.message);
        }
      });
    }
    else {
      console.log('servicio que no va el TAI');
    }
  }
}
