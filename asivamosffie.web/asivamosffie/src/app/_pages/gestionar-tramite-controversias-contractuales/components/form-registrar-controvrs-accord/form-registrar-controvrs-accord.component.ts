import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { trimTrailingNulls } from '@angular/compiler/src/render3/view/util';
@Component({
  selector: 'app-form-registrar-controvrs-accord',
  templateUrl: './form-registrar-controvrs-accord.component.html',
  styleUrls: ['./form-registrar-controvrs-accord.component.scss']
})
export class FormRegistrarControvrsAccordComponent implements OnInit {
  @Input() isEditable;

  addressForm = this.fb.group({
    tipoControversia: [null, Validators.required],
    fechaSolicitud: [null, Validators.required],
    motivosSolicitud: [null, Validators.required],
    fechaComitePretecnico: [null, Validators.required],
    conclusionComitePretecnico: [null, Validators.required],
    procedeSolicitud: [null, Validators.required],
    requeridoComite: [null, Validators.required],
    fechaRadicadoSAC: ['', Validators.required],
    numeroRadicadoSAC: [0, Validators.required],
    resumenJustificacionSolicitud: ['', Validators.required]
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
  constructor(private fb: FormBuilder, public dialog: MatDialog) { }
  ngOnInit(): void {
    if (this.isEditable == true) {
      this.services.GetControversiaContractualById(this.idControversia).subscribe((resp: any) => {
        const controversiaSelected = this.tipoControversiaArrayDom.find(t => t.codigo === resp.tipoControversiaCodigo);
        this.addressForm.get('tipoControversia').setValue(controversiaSelected);
        this.addressForm.get('fechaSolicitud').setValue(resp.fechaSolicitud);
        this.addressForm.get('fechaComitePretecnico').setValue(resp.fechaComitePreTecnico);
        this.addressForm.get('conclusionComitePretecnico').setValue(resp.conclusionComitePreTecnico);
        this.addressForm.get('procedeSolicitud').setValue(resp.esProcede);
        this.addressForm.get('requeridoComite').setValue(resp.esRequiereComite);
        this.addressForm.get('fechaRadicadoSAC').setValue(resp.esRequiereComite);
        this.addressForm.get('numeroRadicadoSAC').setValue(resp.esRequiereComite);
        if (resp.esProcede == false) {
          this.addressForm.get('motivosRechazo').setValue(resp.motivoJustificacionRechazo);
        }
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

  loadSemaforos() {
    this.estadoSemaforo.emit('sin-diligenciar');
    switch (this.addressForm.value.tipoControversia.codigo) {
      case '1':
        if (this.addressForm.value.tipoControversia.codigo == '1' && this.addressForm.value.fechaSolicitud != null && this.addressForm.value.motivosSolicitud != null
          && this.addressForm.value.fechaComitePretecnico != null && this.addressForm.value.conclusionComitePretecnico != null && this.addressForm.value.procedeSolicitud != null) {
          this.estadoSemaforo.emit('completo');
          this.estaCompleto = true;
        }
        else {
          this.estadoSemaforo.emit('en-proceso');
          this.estaCompleto = false;
        }
        break;
      case '2':
        if (this.addressForm.value.tipoControversia.codigo == '2' && this.addressForm.value.fechaRadicadoSAC != null && this.addressForm.value.motivosSolicitud != null
          && this.addressForm.value.numeroRadicadoSAC != null && this.addressForm.value.resumenJustificacionSolicitud != null) {
          this.estadoSemaforo.emit('completo');
          this.estaCompleto = true;
        }
        else {
          this.estadoSemaforo.emit('en-proceso');
          this.estaCompleto = false;
        }
        break;
      case '3':
        if (this.addressForm.value.tipoControversia.codigo == '3' && this.addressForm.value.fechaRadicadoSAC != null && this.addressForm.value.motivosSolicitud != null
          && this.addressForm.value.numeroRadicadoSAC != null && this.addressForm.value.resumenJustificacionSolicitud != null) {
          this.estadoSemaforo.emit('completo');
          this.estaCompleto = true;
        }
        else {
          this.estadoSemaforo.emit('en-proceso');
          this.estaCompleto = false;
        }
        break;
      case '4':
        if (this.addressForm.value.tipoControversia.codigo == '4' && this.addressForm.value.fechaRadicadoSAC != null && this.addressForm.value.motivosSolicitud != null
          && this.addressForm.value.numeroRadicadoSAC != null && this.addressForm.value.resumenJustificacionSolicitud != null) {
          this.estadoSemaforo.emit('completo');
          this.estaCompleto = true;
        }
        else {
          this.estadoSemaforo.emit('en-proceso');
          this.estaCompleto = false;
        }
        break;
      case '5':
        if (this.addressForm.value.tipoControversia.codigo == '5' && this.addressForm.value.fechaSolicitud != null && this.addressForm.value.motivosSolicitud != null
          && this.addressForm.value.resumenJustificacionSolicitud != null) {
          this.estadoSemaforo.emit('completo');
          this.estaCompleto = true;
        }
        else {
          this.estadoSemaforo.emit('en-proceso');
          this.estaCompleto = false;
        }
        break;
      case '6':
        if (this.addressForm.value.tipoControversia.codigo == '6' && this.addressForm.value.fechaSolicitud != null && this.addressForm.value.motivosSolicitud != null
          && this.addressForm.value.resumenJustificacionSolicitud != null) {
          this.estadoSemaforo.emit('completo');
          this.estaCompleto = true;
        }
        else {
          this.estadoSemaforo.emit('en-proceso');
          this.estaCompleto = false;
        }
        break;
      case '7':
        if (this.addressForm.value.tipoControversia.codigo == '7' && this.addressForm.value.fechaSolicitud != null && this.addressForm.value.motivosSolicitud != null
          && this.addressForm.value.resumenJustificacionSolicitud != null) {
          this.estadoSemaforo.emit('completo');
          this.estaCompleto = true;
        }
        else {
          this.estadoSemaforo.emit('en-proceso');
          this.estaCompleto = false;
        }
        break;
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
      e.editor.deleteText(n-1, e.editor.getLength());
    }
  }
  textoLimpio(texto,n) {
    if (texto!=undefined) {
      return texto.getLength() > n ? n : texto.getLength();
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

    let fecha1 = Date.parse(this.addressForm.get('fechaSolicitud').value);
    this.fechaSesion = new Date(fecha1);
    this.fechaSesionString = `${this.fechaSesion.getFullYear()}-${this.fechaSesion.getMonth() + 1}-${this.fechaSesion.getDate()}`;

    let fecha2 = Date.parse(this.addressForm.get('fechaComitePretecnico').value);
    this.fechaSesion2 = new Date(fecha2);
    this.fechaSesionString2 = `${this.fechaSesion2.getFullYear()}-${this.fechaSesion2.getMonth() + 1}-${this.fechaSesion2.getDate()}`;

    let fecha3 = Date.parse(this.addressForm.get('fechaRadicadoSAC').value);
    this.fechaSesion3 = new Date(fecha3);
    this.fechaSesionString3 = `${this.fechaSesion3.getFullYear()}-${this.fechaSesion3.getMonth() + 1}-${this.fechaSesion3.getDate()}`;

    if (this.addressForm.value.tipoControversia.codigo == '1') {
      if (this.addressForm.value.tipoControversia.codigo == '1' && this.addressForm.value.fechaSolicitud != null && this.addressForm.value.motivosSolicitud != null
        && this.addressForm.value.fechaComitePretecnico != null && this.addressForm.value.conclusionComitePretecnico != null && this.addressForm.value.procedeSolicitud != null) {
        this.estaCompleto = true;
      }
      else {
        this.estaCompleto = false;
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
      if (this.estaCompleto == true && this.addressForm.value.procedeSolicitud == false) {
        estadoControversia = "2";
      }
      else if (this.estaCompleto == false && this.addressForm.value.procedeSolicitud == true && this.addressForm.value.requeridoComite == null) {
        estadoControversia = "3";
      }
      else if (this.estaCompleto == true && this.addressForm.value.procedeSolicitud == true && this.addressForm.value.requeridoComite == true) {
        estadoControversia = "4";
      }
      else if (this.estaCompleto == false) {
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
          "FechaCreacion": "2020-11-01"
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
          "FechaCreacion": "2020-11-01"
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
    else if (this.addressForm.value.tipoControversia.codigo == '2' || this.addressForm.value.tipoControversia.codigo == '3' || this.addressForm.value.tipoControversia.codigo == '4') {
      if ((this.addressForm.value.tipoControversia.codigo == '2' || this.addressForm.value.tipoControversia.codigo == '3' || this.addressForm.value.tipoControversia.codigo == '4')
        && this.addressForm.value.fechaRadicadoSAC != null && this.addressForm.value.numeroRadicadoSAC != null && this.addressForm.value.motivosSolicitud != null
        && this.addressForm.value.resumenJustificacionSolicitud != null) {
        this.estaCompleto = true;
      }
      else {
        this.estaCompleto = false;
      }
      let motivosList1;
      if (this.addressForm.value.motivosSolicitud != undefined) {
        motivosList1 = [this.addressForm.value.motivosSolicitud[0].codigo];
        for (let i = 1; i < this.addressForm.value.motivosSolicitud.length; i++) {
          const motivoAux = motivosList1.push(this.addressForm.value.motivosSolicitud[i].codigo);
        }
      }
      let formArrayNoTaiContratista;
      let estadoControversiaContratista;
      let motivosArrayCollectedNoTaiContratista;
      if (this.estaCompleto == false) {
        estadoControversiaContratista = '1';
      }
      else {
        estadoControversiaContratista = '2';
      }
      if (this.isEditable == true) {
        formArrayNoTaiContratista = {
          "TipoControversiaCodigo": this.addressForm.value.tipoControversia.codigo,
          "FechaSolicitud": this.fechaSesionString3,
          "NumeroSolicitud": this.numeroSolicitud,
          "SolicitudId": 0,
          "NumeroRadicadoSac": this.addressForm.value.numeroRadicadoSAC,
          "RutaSoporte": "",
          "EstadoCodigo": estadoControversiaContratista,
          "EsCompleto": this.estaCompleto,
          "ContratoId": this.contratoId,
          "ConclusionComitePreTecnico": '',
          "MotivoJustificacionRechazo": this.addressForm.value.resumenJustificacionSolicitud,
          "UsuarioCreacion": "us cre",
          "UsuarioModificacion": "us mod",
          "FechaComitePreTecnico": '',
          "EsProcede": true,
          "EsRequiereComite": true,
          "ControversiaContractualId": parseInt(this.idControversia),
          "FechaCreacion": "2020-11-01"
        };
      }
      else {
        formArrayNoTaiContratista = {
          "TipoControversiaCodigo": this.addressForm.value.tipoControversia.codigo,
          "FechaSolicitud": this.fechaSesionString3,
          "NumeroSolicitud": "",
          "SolicitudId": 0,
          "NumeroRadicadoSac": this.addressForm.value.numeroRadicadoSAC,
          "RutaSoporte": "",
          "EstadoCodigo": estadoControversiaContratista,
          "EsCompleto": this.estaCompleto,
          "ContratoId": this.contratoId,
          "ConclusionComitePreTecnico": '',
          "MotivoJustificacionRechazo": this.addressForm.value.resumenJustificacionSolicitud,
          "UsuarioCreacion": "us cre",
          "UsuarioModificacion": "us mod",
          "FechaComitePreTecnico": '',
          "EsProcede": true,
          "EsRequiereComite": true,
          "FechaCreacion": "2020-11-01"
        };
      }
      this.services.CreateEditarControversiaTAI(formArrayNoTaiContratista).subscribe(resp_0 => {
        if (resp_0.isSuccessful == true) {
          this.openDialog('', 'La información ha sido guardada exitosamente.');
          if (this.isEditable == true) {
            if (this.addressForm.value.motivosSolicitud != undefined || this.addressForm.value.motivosSolicitud != null) {
              for (let i = 0; i < motivosList1.length; i++) {
                switch (motivosList1[i]) {
                  case '1':
                    motivosArrayCollectedNoTaiContratista = {
                      "ControversiaContractualId": this.idControversia,
                      'MotivoSolicitudCodigo': '1',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                      "ControversiaMotivoId": this.idMotivo1
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollectedNoTaiContratista).subscribe(r => {
                    });
                    break;
                  case '2':
                    motivosArrayCollectedNoTaiContratista = {
                      "ControversiaContractualId": this.idControversia,
                      'MotivoSolicitudCodigo': '2',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                      "ControversiaMotivoId": this.idMotivo2
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollectedNoTaiContratista).subscribe(r1 => {
                    });
                    break;
                  case '3':
                    motivosArrayCollectedNoTaiContratista = {
                      "ControversiaContractualId": this.idControversia,
                      'MotivoSolicitudCodigo': '3',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                      "ControversiaMotivoId": this.idMotivo3
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollectedNoTaiContratista).subscribe(r2 => {
                    });
                    break;
                }
              }
            }
            this.router.navigate(['/gestionarTramiteControversiasContractuales']);
          }
          else {
            if (this.addressForm.value.motivosSolicitud != undefined || this.addressForm.value.motivosSolicitud != null) {
              for (let i = 0; i < motivosList1.length; i++) {
                switch (motivosList1[i]) {
                  case '1':
                    motivosArrayCollectedNoTaiContratista = {
                      "ControversiaContractualId": resp_0.data.controversiaContractualId,
                      'MotivoSolicitudCodigo': '1',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollectedNoTaiContratista).subscribe(r => {
                    });
                    break;
                  case '2':
                    motivosArrayCollectedNoTaiContratista = {
                      "ControversiaContractualId": resp_0.data.controversiaContractualId,
                      'MotivoSolicitudCodigo': '2',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollectedNoTaiContratista).subscribe(r1 => {
                    });
                    break;
                  case '3':
                    motivosArrayCollectedNoTaiContratista = {
                      "ControversiaContractualId": resp_0.data.controversiaContractualId,
                      'MotivoSolicitudCodigo': '3',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                      "ControversiaMotivoId": 2
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollectedNoTaiContratista).subscribe(r2 => {
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
      if ((this.addressForm.value.tipoControversia.codigo == '5' || this.addressForm.value.tipoControversia.codigo == '6' || this.addressForm.value.tipoControversia.codigo == '7')
        && this.addressForm.value.fechaSolicitud != null && this.addressForm.value.motivosSolicitud != null && this.addressForm.value.resumenJustificacionSolicitud != null) {
        this.estaCompleto = true;
      }
      else {
        this.estaCompleto = false;
      }
      let motivosList2;
      if (this.addressForm.value.motivosSolicitud != undefined) {
        motivosList2 = [this.addressForm.value.motivosSolicitud[0].codigo];
        for (let i = 1; i < this.addressForm.value.motivosSolicitud.length; i++) {
          const motivoAux = motivosList2.push(this.addressForm.value.motivosSolicitud[i].codigo);
        }
      }
      let formArrayNoTaiContratante;
      let estadoControversiaContratante;
      let motivosArrayCollectedNoTaiContratante;
      if (this.estaCompleto == false) {
        estadoControversiaContratante = '1';
      }
      else {
        estadoControversiaContratante = '2';
      }
      if (this.isEditable == true) {
        formArrayNoTaiContratante = {
          "TipoControversiaCodigo": this.addressForm.value.tipoControversia.codigo,
          "FechaSolicitud": this.fechaSesionString,
          "NumeroSolicitud": this.numeroSolicitud,
          "SolicitudId": 0,
          "NumeroRadicadoSac": 0,
          "RutaSoporte": "",
          "EstadoCodigo": estadoControversiaContratante,
          "EsCompleto": this.estaCompleto,
          "ContratoId": this.contratoId,
          "ConclusionComitePreTecnico": '',
          "MotivoJustificacionRechazo": this.addressForm.value.resumenJustificacionSolicitud,
          "UsuarioCreacion": "us cre",
          "UsuarioModificacion": "us mod",
          "FechaComitePreTecnico": '',
          "EsProcede": true,
          "EsRequiereComite": true,
          "ControversiaContractualId": parseInt(this.idControversia),
          "FechaCreacion": "2020-11-01"
        };
      }
      else {
        formArrayNoTaiContratante = {
          "TipoControversiaCodigo": this.addressForm.value.tipoControversia.codigo,
          "FechaSolicitud": this.fechaSesionString,
          "NumeroSolicitud": "",
          "SolicitudId": 0,
          "NumeroRadicadoSac": 0,
          "RutaSoporte": "",
          "EstadoCodigo": estadoControversiaContratante,
          "EsCompleto": this.estaCompleto,
          "ContratoId": this.contratoId,
          "ConclusionComitePreTecnico": '',
          "MotivoJustificacionRechazo": this.addressForm.value.resumenJustificacionSolicitud,
          "UsuarioCreacion": "us cre",
          "UsuarioModificacion": "us mod",
          "FechaComitePreTecnico": '',
          "EsProcede": true,
          "EsRequiereComite": true,
          "FechaCreacion": "2020-11-01"
        };
      }
      this.services.CreateEditarControversiaTAI(formArrayNoTaiContratante).subscribe(resp_0 => {
        if (resp_0.isSuccessful == true) {
          this.openDialog('', 'La información ha sido guardada exitosamente.');
          if (this.isEditable == true) {
            if (this.addressForm.value.motivosSolicitud != undefined || this.addressForm.value.motivosSolicitud != null) {
              for (let i = 0; i < motivosList2.length; i++) {
                switch (motivosList2[i]) {
                  case '1':
                    motivosArrayCollectedNoTaiContratante = {
                      "ControversiaContractualId": this.idControversia,
                      'MotivoSolicitudCodigo': '1',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                      "ControversiaMotivoId": this.idMotivo1
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollectedNoTaiContratante).subscribe(r => {
                    });
                    break;
                  case '2':
                    motivosArrayCollectedNoTaiContratante = {
                      "ControversiaContractualId": this.idControversia,
                      'MotivoSolicitudCodigo': '2',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                      "ControversiaMotivoId": this.idMotivo2
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollectedNoTaiContratante).subscribe(r1 => {
                    });
                    break;
                  case '3':
                    motivosArrayCollectedNoTaiContratante = {
                      "ControversiaContractualId": this.idControversia,
                      'MotivoSolicitudCodigo': '3',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                      "ControversiaMotivoId": this.idMotivo3
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollectedNoTaiContratante).subscribe(r2 => {
                    });
                    break;
                }
              }
            }
            this.router.navigate(['/gestionarTramiteControversiasContractuales']);
          }
          else {
            if (this.addressForm.value.motivosSolicitud != undefined || this.addressForm.value.motivosSolicitud != null) {
              for (let i = 0; i < motivosList2.length; i++) {
                switch (motivosList2[i]) {
                  case '1':
                    motivosArrayCollectedNoTaiContratante = {
                      "ControversiaContractualId": resp_0.data.controversiaContractualId,
                      'MotivoSolicitudCodigo': '1',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollectedNoTaiContratante).subscribe(r => {
                    });
                    break;
                  case '2':
                    motivosArrayCollectedNoTaiContratante = {
                      "ControversiaContractualId": resp_0.data.controversiaContractualId,
                      'MotivoSolicitudCodigo': '2',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollectedNoTaiContratante).subscribe(r1 => {
                    });
                    break;
                  case '3':
                    motivosArrayCollectedNoTaiContratante = {
                      "ControversiaContractualId": resp_0.data.controversiaContractualId,
                      'MotivoSolicitudCodigo': '3',
                      "UsuarioCreacion": "",
                      "UsuarioModificacion": "",
                      "ControversiaMotivoId": 2
                    };
                    this.services.CreateEditarControversiaMotivo(motivosArrayCollectedNoTaiContratante).subscribe(r2 => {
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
  }

}
