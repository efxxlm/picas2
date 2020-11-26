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
    conclusionComitePretecnico: [null, Validators.required],
    procedeSolicitud: [null, Validators.required],
    motivosRechazo: [null, Validators.required],
    requeridoComite: [null, Validators.required],
    fechaRadicadoSAC: [null, Validators.required],
    numeroRadicadoSAC: [null, Validators.required],
    resumenJustificacionSolicitud: [null, Validators.required]
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
  numeroSolicitud: any;
  userCreation: any;
  idMotivo1: number;
  idMotivo2: number;
  idMotivo3: number;
  arrayMotivosLoaded: any[] = [];
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
        this.userCreation = resp.usuarioCreacion;
        this.loadMotivosFromService(this.idControversia);
      });
      this.loadSemaforos();
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
  loadSemaforos() {
    this.estadoSemaforo.emit('sin-diligenciar');
    /*
    PEDIR AYUDA A CARLOS ESTE CASO
    if (this.addressForm.get('tipoControversia') == null && this.addressForm.get('fechaSolicitud') == null && this.addressForm.get('motivosSolicitud') == null
      && this.addressForm.get('fechaComitePretecnico') == null && this.addressForm.get('conclusionComitePretecnico') == null && this.addressForm.get('procedeSolicitud') == null) {
      this.estadoSemaforo.emit('sin-diligenciar');
    }
    if (this.addressForm.get('tipoControversia') || this.addressForm.get('fechaSolicitud') || this.addressForm.get('motivosSolicitud')
      || this.addressForm.get('fechaComitePretecnico') || this.addressForm.get('conclusionComitePretecnico') || this.addressForm.get('procedeSolicitud')) {
      this.estadoSemaforo.emit('en-proceso');
    }
    if (this.addressForm.get('tipoControversia') != null && this.addressForm.get('fechaSolicitud') != null && this.addressForm.get('motivosSolicitud') != null
      && this.addressForm.get('fechaComitePretecnico') != null && this.addressForm.get('conclusionComitePretecnico') != null && this.addressForm.get('procedeSolicitud') != null) {
      this.estadoSemaforo.emit('completo');
    }
    */
  }
  loadMotivosFromService(controversiaId) {
    this.services.GetMotivosSolicitudByControversiaId(controversiaId).subscribe((data: any) => {
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
    });
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
    if (this.addressForm.value.tipoControversia.codigo == '1') {
      let motivosList;
      if (this.addressForm.value.motivosSolicitud != undefined) {
        motivosList = [this.addressForm.value.motivosSolicitud[0].codigo];
        for (let i = 1; i < this.addressForm.value.motivosSolicitud.length; i++) {
          const motivoAux = motivosList.push(this.addressForm.value.motivosSolicitud[i].codigo);
        }
      }
      let formArrayTai;
      let motivosArrayCollected;
      if (this.isEditable == true) {
        formArrayTai = {
          "TipoControversiaCodigo": this.addressForm.value.tipoControversia.codigo,
          "FechaSolicitud": this.addressForm.value.fechaSolicitud,
          "NumeroSolicitud": this.numeroSolicitud,
          "EstadoCodigo": "1",
          "EsCompleto": false,
          "ContratoId": this.contratoId,
          "ConclusionComitePreTecnico": this.addressForm.value.conclusionComitePretecnico,
          "MotivoJustificacionRechazo": this.addressForm.value.motivosRechazo,
          "UsuarioCreacion": "us cre",
          "UsuarioModificacion": "us mod",
          "FechaComitePreTecnico": this.addressForm.value.fechaComitePretecnico,
          "EsProcede": this.addressForm.value.procedeSolicitud,
          "EsRequiereComite": this.addressForm.value.requeridoComite,
          "ControversiaContractualId": this.idControversia
        };
      }
      else {
        formArrayTai = {
          "TipoControversiaCodigo": this.addressForm.value.tipoControversia.codigo,
          "FechaSolicitud": this.addressForm.value.fechaSolicitud,
          "NumeroSolicitud": "",
          "SolicitudId": 0, "NumeroRadicadoSac": 0,
          "RutaSoporte": "",
          "UsuarioCreacion": "",
          "EstadoCodigo": "1",
          "EsCompleto": false,
          "ContratoId": this.contratoId,
          "ConclusionComitePreTecnico": this.addressForm.value.conclusionComitePretecnico,
          "MotivoJustificacionRechazo": this.addressForm.value.motivosRechazo,
          "UsuarioModificacion": "us mod",
          "FechaComitePreTecnico": this.addressForm.value.fechaComitePretecnico,
          "EsProcede": this.addressForm.value.procedeSolicitud,
          "EsRequiereComite": this.addressForm.value.requeridoComite
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
                      "ControversiaMotivoId": 2
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
                      "ControversiaMotivoId": 2
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
