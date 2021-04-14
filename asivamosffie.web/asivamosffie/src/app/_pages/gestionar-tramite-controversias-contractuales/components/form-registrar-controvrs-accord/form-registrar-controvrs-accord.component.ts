import { Component, EventEmitter, Input, OnInit, Output, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { trimTrailingNulls } from '@angular/compiler/src/render3/view/util';
import { ControversiaContractual, ControversiaMotivo } from 'src/app/_interfaces/controversiaContractual';
@Component({
  selector: 'app-form-registrar-controvrs-accord',
  templateUrl: './form-registrar-controvrs-accord.component.html',
  styleUrls: ['./form-registrar-controvrs-accord.component.scss']
})
export class FormRegistrarControvrsAccordComponent implements OnInit, OnDestroy {
  @Input() isEditable;
  @Input() contratoId;
  @Input() idControversia;
  @Output() estadoSemaforo = new EventEmitter<string>();

  estadoForm: boolean = null;
  addressForm = this.fb.group({
    tipoControversia: [null, Validators.required],
    fechaSolicitud: [null, Validators.required],
    motivosSolicitud: [null, Validators.required],
    cualOtroMotivo: [null ,Validators.required],
    fechaComitePretecnico: [null, Validators.required],
    conclusionComitePretecnico: [null, Validators.required],
    procedeSolicitud: [null, Validators.required],
    motivosRechazo: [null, Validators.required],
    requeridoComite: [null, Validators.required],
    fechaRadicadoSAC: [null, Validators.required],
    numeroRadicadoSAC: [null, Validators.required],
    resumenJustificacionSolicitud: [null, Validators.required]
  });
  tipoControversiaArrayDom = [

  ];
  motivosSolicitudArray = [

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
  estaEditando = false;
  idContrato: any;
  numeroSolicitud: any;
  userCreation: any;
  idMotivo1: number;
  idMotivo2: number;
  idMotivo3: number;
  idMotivo4: number;
  listaMotivos = {};
  arrayMotivosLoaded: any[] = [];
  estaCompleto: boolean;

  fechaSesionString: string;
  fechaSesion: Date;

  fechaSesionString2: string;
  fechaSesion2: Date;

  fechaSesionString3: string;
  fechaSesion3: Date;

  sucessInfo: string = 'La información ha sido guardada exitosamente.';
  obj1: boolean;

  realizoPeticion: boolean = false;
  constructor(private router: Router, private fb: FormBuilder, public dialog: MatDialog, private services: ContractualControversyService, private common: CommonService) {
    this.common.listaTiposDeControversiaContractual().subscribe(data => {
      this.tipoControversiaArrayDom = data;
    });
    this.common.listaMotivosSolicitudControversiaContractual().subscribe(data => {
      this.motivosSolicitudArray = data;
    });
  }
  ngOnInit(): void {
    //this.loadMotivosList();
    let lista: any[] = [];
    if (this.isEditable == true) {
      //this.loadtipoControversias();
      this.estaEditando = true;
      this.addressForm.markAllAsTouched();
      this.services.GetControversiaContractualById(this.idControversia).subscribe((resp: any) => {
        for (let i = 0; i < this.tipoControversiaArrayDom.length; i++) {
          const controversiaSelected = this.tipoControversiaArrayDom.find(p => p.codigo == resp.tipoControversiaCodigo);
          this.addressForm.get('tipoControversia').setValue(controversiaSelected);
        }
        this.addressForm.get('fechaSolicitud').setValue(resp.fechaSolicitud);
        this.addressForm.get('fechaComitePretecnico').setValue(resp.fechaComitePreTecnico);
        this.addressForm.get('conclusionComitePretecnico').setValue(resp.conclusionComitePreTecnico!== undefined ? resp.conclusionComitePreTecnico : null);
        this.addressForm.get('procedeSolicitud').setValue(resp.esProcede);
        this.addressForm.get('requeridoComite').setValue(resp.esRequiereComite);
        this.addressForm.get('fechaRadicadoSAC').setValue(resp.fechaSolicitud);
        this.addressForm.get('numeroRadicadoSAC').setValue(resp.numeroRadicadoSac);
        this.addressForm.get('resumenJustificacionSolicitud').setValue(resp.motivoJustificacionRechazo!== undefined ? resp.motivoJustificacionRechazo : null);
        if (resp.esProcede == false) {
          this.addressForm.get('motivosRechazo').setValue(resp.motivoJustificacionRechazo!== undefined ? resp.motivoJustificacionRechazo : null);
        }
        this.numeroSolicitud = resp.numeroSolicitudFormat;
        this.idContrato = resp.contratoId;
        this.userCreation = resp.usuarioCreacion;
        this.services.GetMotivosSolicitudByControversiaId(this.idControversia).subscribe((data: any) => {
          //inicio modificacion
          this.arrayMotivosLoaded = data;
          let listaSeleccionados = [];
          let motivoAuxS: any = {}
          data.forEach(p => {
            motivoAuxS = this.motivosSolicitudArray.find(m => m.codigo == p.motivoSolicitudCodigo);
            listaSeleccionados.push(motivoAuxS);
          });
          this.addressForm.get('motivosSolicitud').setValue(listaSeleccionados);
          for (let n = 0; n < listaSeleccionados.length; n++) {
            switch (listaSeleccionados[n].codigo) {
              case '4':
                this.obj1 = true;
                if (this.obj1 == true) {
                  this.addressForm.get('cualOtroMotivo').setValue(resp.cualOtroMotivo !== undefined ? resp.cualOtroMotivo : null);
                }
                break;
            }
          }
          if(this.obj1!=true){
            this.addressForm.get('cualOtroMotivo').setValue(null);
          }
          //fin modificacion
          /*
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
          }*/
          this.loadSemaforos();
        });
      });
    }
    else {
      //this.loadtipoControversias();
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
  loadtipoControversias() {

  }
  loadMotivosList() {

  }

  loadSemaforos() {
    this.estadoSemaforo.emit('sin-diligenciar');
    switch (this.addressForm.value.tipoControversia.codigo) {
      case '1':
        if (this.addressForm.value.tipoControversia.codigo == '1' && this.addressForm.value.fechaSolicitud != null && ((this.addressForm.value.motivosSolicitud.length > 0 && this.obj1!=true && this.addressForm.value.cualOtroMotivo == null) || (this.addressForm.value.motivosSolicitud.length > 0 && this.obj1==true && this.addressForm.value.cualOtroMotivo != null))
          && this.addressForm.value.fechaComitePretecnico != null && this.addressForm.value.conclusionComitePretecnico != null
          && (((this.addressForm.value.procedeSolicitud == true && this.addressForm.value.motivosRechazo == null && this.addressForm.value.requeridoComite == null) || (this.addressForm.value.procedeSolicitud == false && this.addressForm.value.motivosRechazo != null && this.addressForm.value.requeridoComite == null))
          || ((this.addressForm.value.procedeSolicitud == true && this.addressForm.value.requeridoComite != null) || (this.addressForm.value.procedeSolicitud == false && this.addressForm.value.requeridoComite == null)))) {
          this.estadoSemaforo.emit('completo');
          this.estaCompleto = true;
        }
        else {
          this.estadoSemaforo.emit('en-proceso');
          this.estaCompleto = false;
        }
        break;
      case '2':
        if (this.addressForm.value.tipoControversia.codigo == '2' && this.addressForm.value.fechaRadicadoSAC != null && ((this.addressForm.value.motivosSolicitud.length > 0 && this.obj1!=true && this.addressForm.value.cualOtroMotivo == null) || (this.addressForm.value.motivosSolicitud.length > 0 && this.obj1==true && this.addressForm.value.cualOtroMotivo != null))
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
        if (this.addressForm.value.tipoControversia.codigo == '3' && this.addressForm.value.fechaRadicadoSAC != null && ((this.addressForm.value.motivosSolicitud.length > 0 && this.obj1!=true && this.addressForm.value.cualOtroMotivo == null) || (this.addressForm.value.motivosSolicitud.length > 0 && this.obj1==true && this.addressForm.value.cualOtroMotivo != null))
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
        if (this.addressForm.value.tipoControversia.codigo == '4' && this.addressForm.value.fechaRadicadoSAC != null && ((this.addressForm.value.motivosSolicitud.length > 0 && this.obj1!=true && this.addressForm.value.cualOtroMotivo == null) || (this.addressForm.value.motivosSolicitud.length > 0 && this.obj1==true && this.addressForm.value.cualOtroMotivo != null))
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
        if (this.addressForm.value.tipoControversia.codigo == '5' && this.addressForm.value.fechaSolicitud != null && ((this.addressForm.value.motivosSolicitud.length > 0 && this.obj1!=true && this.addressForm.value.cualOtroMotivo == null) || (this.addressForm.value.motivosSolicitud.length > 0 && this.obj1==true && this.addressForm.value.cualOtroMotivo != null))
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
        if (this.addressForm.value.tipoControversia.codigo == '6' && this.addressForm.value.fechaSolicitud != null && ((this.addressForm.value.motivosSolicitud.length > 0 && this.obj1!=true && this.addressForm.value.cualOtroMotivo == null) || (this.addressForm.value.motivosSolicitud.length > 0 && this.obj1==true && this.addressForm.value.cualOtroMotivo != null))
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
        if (this.addressForm.value.tipoControversia.codigo == '7' && this.addressForm.value.fechaSolicitud != null && ((this.addressForm.value.motivosSolicitud.length > 0 && this.obj1!=true && this.addressForm.value.cualOtroMotivo == null) || (this.addressForm.value.motivosSolicitud.length > 0 && this.obj1==true && this.addressForm.value.cualOtroMotivo != null))
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
    const cualOtro = values.find(value => value.codigo == "4");
    cualOtro ? this.obj1 = true : this.obj1 = false;
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

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
      if(this.idControversia != 'undefined' && this.idControversia != null && this.addressForm.dirty === false){
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(
          () => this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleEditarControversia', this.idControversia]));  
      }else{
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(
          () => this.router.navigate(['/gestionarTramiteControversiasContractuales']));     
      }
      });
  }

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();

    let fecha1 = Date.parse(this.addressForm.get('fechaSolicitud').value);
    this.fechaSesion = new Date(fecha1);
    this.fechaSesionString = `${this.fechaSesion.getFullYear()}-${this.fechaSesion.getMonth() + 1}-${this.fechaSesion.getDate()}`;

    let fecha2 = Date.parse(this.addressForm.get('fechaComitePretecnico').value);
    this.fechaSesion2 = new Date(fecha2);
    this.fechaSesionString2 = `${this.fechaSesion2.getFullYear()}-${this.fechaSesion2.getMonth() + 1}-${this.fechaSesion2.getDate()}`;

    let fecha3 = Date.parse(this.addressForm.get('fechaRadicadoSAC').value);
    this.fechaSesion3 = new Date(fecha3);
    this.fechaSesionString3 = `${this.fechaSesion3.getFullYear()}-${this.fechaSesion3.getMonth() + 1}-${this.fechaSesion3.getDate()}`;

    let motivosDeSolicitud = this.addressForm.get('motivosSolicitud').value;

    if (this.addressForm.value.tipoControversia.codigo == '1') {
      if (this.addressForm.value.tipoControversia.codigo == '1' && this.addressForm.value.fechaSolicitud != null && this.addressForm.value.motivosSolicitud != null
        && this.addressForm.value.fechaComitePretecnico != null && this.addressForm.value.conclusionComitePretecnico != null && this.addressForm.value.procedeSolicitud != null) {
        this.estaCompleto = true;
      }
      else {
        this.estaCompleto = false;
      }
      let formArrayTai;
      let estadoControversia = "1";
      if (this.estaCompleto == true && this.addressForm.value.procedeSolicitud == false && this.addressForm.value.motivosRechazo != null) {
        estadoControversia = "2";
      }
      if ((this.estaCompleto == false && this.addressForm.value.procedeSolicitud == true && this.addressForm.value.requeridoComite == null)
        || (this.estaCompleto == true && this.addressForm.value.procedeSolicitud == true && this.addressForm.value.requeridoComite == false)) {
        estadoControversia = "3";
      }
      if (this.estaCompleto == true && this.addressForm.value.procedeSolicitud == true && this.addressForm.value.requeridoComite == true) {
        estadoControversia = "4";
      }
      if (this.estaCompleto == false) {
        estadoControversia = "1";
      }
    const controversiaContractual = [] as ControversiaContractual;
    controversiaContractual.controversiaMotivo = [];
    motivosDeSolicitud.forEach(newMotivos => {
      let controversiaMotivoId = 0;
      this.arrayMotivosLoaded.forEach(k => {
        if(k.motivoSolicitudCodigo === newMotivos.codigo){
          controversiaMotivoId = k.controversiaMotivoId;
        }
      });
      const controversiaMotivo: ControversiaMotivo = {
        controversiaContractualId: this.idControversia,
        motivoSolicitudCodigo: newMotivos.codigo,
        controversiaMotivoId: controversiaMotivoId,
        fechaCreacion: null
      };
      controversiaContractual.controversiaMotivo.push(controversiaMotivo);
    });
      if (this.isEditable == true) {
        formArrayTai = {
          "TipoControversiaCodigo": this.addressForm.value.tipoControversia.codigo,
          "FechaSolicitud": this.addressForm.value.fechaSolicitud,
          "NumeroSolicitud": this.numeroSolicitud,
          "SolicitudId": 0,
          "NumeroRadicadoSac": 0,
          "RutaSoporte": "",
          "EstadoCodigo": estadoControversia,
          "EsCompleto": this.estaCompleto,
          "ContratoId": this.idContrato,
          "CualOtroMotivo": this.addressForm.value.cualOtroMotivo,
          "ConclusionComitePreTecnico": this.addressForm.value.conclusionComitePretecnico,
          "MotivoJustificacionRechazo": this.addressForm.value.motivosRechazo,
          "FechaComitePreTecnico": this.addressForm.value.fechaComitePretecnico,
          "EsProcede": this.addressForm.value.procedeSolicitud,
          "EsRequiereComite": this.addressForm.value.requeridoComite,
          "ControversiaContractualId": parseInt(this.idControversia),
          "ControversiaMotivo": controversiaContractual.controversiaMotivo
        };
      }
      else {
        formArrayTai = {
          "TipoControversiaCodigo": this.addressForm.value.tipoControversia.codigo,
          "FechaSolicitud": this.addressForm.value.fechaSolicitud,
          "NumeroSolicitud": "",
          "SolicitudId": 0,
          "NumeroRadicadoSac": 0,
          "RutaSoporte": "",
          "EstadoCodigo": estadoControversia,
          "EsCompleto": this.estaCompleto,
          "ContratoId": this.contratoId,
          "CualOtroMotivo": this.addressForm.value.cualOtroMotivo,
          "ConclusionComitePreTecnico": this.addressForm.value.conclusionComitePretecnico,
          "MotivoJustificacionRechazo": this.addressForm.value.motivosRechazo,
          "FechaComitePreTecnico": this.addressForm.value.fechaComitePretecnico,
          "EsProcede": this.addressForm.value.procedeSolicitud,
          "EsRequiereComite": this.addressForm.value.requeridoComite,
          "ControversiaMotivo": controversiaContractual.controversiaMotivo
        };
      }
      this.services.CreateEditarControversiaTAI(formArrayTai).subscribe((resp_0: any) => {
        if (resp_0.isSuccessful == true) {
          this.services.CambiarEstadoControversiaContractual(resp_0.data.controversiaContractualId, estadoControversia).subscribe(c => {
          });
          if(this.idControversia === 'undefined' || this.idControversia == null){
            this.idControversia =  resp_0.data.controversiaContractualId;
          }
          this.openDialog('', `<b>${this.sucessInfo}</b>`);
        }
        else {
          this.openDialog('', `<b>${resp_0.message}</b>`);
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
      let formArrayNoTaiContratista;
      let estadoControversiaContratista = '1';
      let motivosArrayCollectedNoTaiContratista;
      if (this.estaCompleto == false) {
        estadoControversiaContratista = '1';
      }
      else {
        estadoControversiaContratista = '2';
      }
      const controversiaContractual = [] as ControversiaContractual;
      controversiaContractual.controversiaMotivo = [];

      motivosDeSolicitud.forEach(newMotivos => {
        let controversiaMotivoId = 0;
        this.arrayMotivosLoaded.forEach(k => {
          if(k.motivoSolicitudCodigo === newMotivos.codigo){
            controversiaMotivoId = k.controversiaMotivoId;
          }
        });
        const controversiaMotivo: ControversiaMotivo = {
          controversiaContractualId: this.idControversia,
          motivoSolicitudCodigo: newMotivos.codigo,
          controversiaMotivoId: controversiaMotivoId,
          fechaCreacion: null
        };
        controversiaContractual.controversiaMotivo.push(controversiaMotivo);
      });
      if (this.isEditable == true) {
        formArrayNoTaiContratista = {
          "TipoControversiaCodigo": this.addressForm.value.tipoControversia.codigo,
          "FechaSolicitud": this.addressForm.value.fechaRadicadoSAC,
          "NumeroSolicitud": this.numeroSolicitud,
          "SolicitudId": 0,
          "NumeroRadicadoSac": this.addressForm.value.numeroRadicadoSAC,
          "RutaSoporte": "",
          "EstadoCodigo": estadoControversiaContratista,
          "EsCompleto": this.estaCompleto,
          "ContratoId": this.idContrato,
          "CualOtroMotivo": this.addressForm.value.cualOtroMotivo,
          "ConclusionComitePreTecnico": '',
          "MotivoJustificacionRechazo": this.addressForm.value.resumenJustificacionSolicitud,
          "FechaComitePreTecnico": '',
          "EsProcede": true,
          "EsRequiereComite": true,
          "ControversiaContractualId": parseInt(this.idControversia),
          "ControversiaMotivo": controversiaContractual.controversiaMotivo
        };
      }
      else {
        formArrayNoTaiContratista = {
          "TipoControversiaCodigo": this.addressForm.value.tipoControversia.codigo,
          "FechaSolicitud": this.addressForm.value.fechaRadicadoSAC,
          "NumeroSolicitud": "",
          "SolicitudId": 0,
          "NumeroRadicadoSac": this.addressForm.value.numeroRadicadoSAC,
          "RutaSoporte": "",
          "EstadoCodigo": estadoControversiaContratista,
          "EsCompleto": this.estaCompleto,
          "ContratoId": this.contratoId,
          "CualOtroMotivo": this.addressForm.value.cualOtroMotivo,
          "ConclusionComitePreTecnico": '',
          "MotivoJustificacionRechazo": this.addressForm.value.resumenJustificacionSolicitud,
          "FechaComitePreTecnico": '',
          "EsProcede": true,
          "EsRequiereComite": true,
          "ControversiaMotivo": controversiaContractual.controversiaMotivo
        };
      }
      this.services.CreateEditarControversiaTAI(formArrayNoTaiContratista).subscribe((resp_0: any) => {
        if (resp_0.isSuccessful == true) {
          this.services.CambiarEstadoControversiaContractual(resp_0.data.controversiaContractualId, estadoControversiaContratista).subscribe(c => {
          });
          if(this.idControversia === 'undefined' || this.idControversia == null){
            this.idControversia =  resp_0.data.controversiaContractualId;
          }
          this.openDialog('', `<b>${this.sucessInfo}</b>`);
        }
        else {
          this.openDialog('', `<b>${resp_0.message}</b>`);
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
      let formArrayNoTaiContratante;
      let estadoControversiaContratante = '1';
      let motivosArrayCollectedNoTaiContratante;
      if (this.estaCompleto == false) {
        estadoControversiaContratante = '1';
      }
      else {
        estadoControversiaContratante = '2';
      }
      const controversiaContractual = [] as ControversiaContractual;
      controversiaContractual.controversiaMotivo = [];

      motivosDeSolicitud.forEach(newMotivos => {
        let controversiaMotivoId = 0;
        this.arrayMotivosLoaded.forEach(k => {
          if(k.motivoSolicitudCodigo === newMotivos.codigo){
            controversiaMotivoId = k.controversiaMotivoId;
          }
        });
        const controversiaMotivo: ControversiaMotivo = {
          controversiaContractualId: this.idControversia,
          motivoSolicitudCodigo: newMotivos.codigo,
          controversiaMotivoId: controversiaMotivoId,
          fechaCreacion: null
        };
        controversiaContractual.controversiaMotivo.push(controversiaMotivo);
      });
      if (this.isEditable == true) {
        formArrayNoTaiContratante = {
          "TipoControversiaCodigo": this.addressForm.value.tipoControversia.codigo,
          "FechaSolicitud": this.addressForm.value.fechaSolicitud,
          "NumeroSolicitud": this.numeroSolicitud,
          "SolicitudId": 0,
          "NumeroRadicadoSac": 0,
          "RutaSoporte": "",
          "EstadoCodigo": estadoControversiaContratante,
          "EsCompleto": this.estaCompleto,
          "ContratoId": this.idContrato,
          "CualOtroMotivo": this.addressForm.value.cualOtroMotivo,
          "ConclusionComitePreTecnico": '',
          "MotivoJustificacionRechazo": this.addressForm.value.resumenJustificacionSolicitud,
          "FechaComitePreTecnico": '',
          "EsProcede": true,
          "EsRequiereComite": true,
          "ControversiaContractualId": parseInt(this.idControversia),
          "ControversiaMotivo": controversiaContractual.controversiaMotivo
        };
      }
      else {
        formArrayNoTaiContratante = {
          "TipoControversiaCodigo": this.addressForm.value.tipoControversia.codigo,
          "FechaSolicitud": this.addressForm.value.fechaSolicitud,
          "NumeroSolicitud": "",
          "SolicitudId": 0,
          "NumeroRadicadoSac": 0,
          "RutaSoporte": "",
          "EstadoCodigo": estadoControversiaContratante,
          "EsCompleto": this.estaCompleto,
          "ContratoId": this.contratoId,
          "CualOtroMotivo": this.addressForm.value.cualOtroMotivo,
          "ConclusionComitePreTecnico": '',
          "MotivoJustificacionRechazo": this.addressForm.value.resumenJustificacionSolicitud,
          "FechaComitePreTecnico": '',
          "EsProcede": true,
          "EsRequiereComite": true,
          "ControversiaMotivo": controversiaContractual.controversiaMotivo
        };
      }
      this.services.CreateEditarControversiaTAI(formArrayNoTaiContratante).subscribe((resp_0: any) => {
        if (resp_0.isSuccessful == true) {
          this.services.CambiarEstadoControversiaContractual(resp_0.data.controversiaContractualId, estadoControversiaContratante).subscribe(c => {
          });
          if(this.idControversia === 'undefined' || this.idControversia == null){
            this.idControversia =  resp_0.data.controversiaContractualId;
          }
          this.openDialog('', `<b>${this.sucessInfo}</b>`);
        }
        else {
          this.openDialog('', `<b>${resp_0.message}</b>`);
        }
      });
    }
  }
}
