import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ActaInicioConstruccionService } from 'src/app/core/_services/actaInicioConstruccion/acta-inicio-construccion.service';
import { GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-form-validar-acta-inicio-construccion',
  templateUrl: './form-validar-acta-inicio-construccion.component.html',
  styleUrls: ['./form-validar-acta-inicio-construccion.component.scss']
})
export class FormValidarActaInicioConstruccionComponent implements OnInit, OnDestroy {
  addressForm = this.fb.group({});
  dataDialog: {
    modalTitle: string,
    modalText: string
  };
  public editable: boolean;
  public title;

  public contratoId;
  contratoCode: string;
  fechaAprobacionSupervisor: Date;
  vigenciaContrato: Date;
  fechaFirmaContrato: Date;
  numeroDRP1: string;
  fechaGeneracionDRP1: Date;
  numeroDRP2: string;
  fechaGeneracionDRP2: Date;
  fechaAprobacionGarantiaPoliza: Date;
  observacionOConsideracionesEspeciales: string;
  valorInicialContrato: string;
  valorActualContrato: string;
  valorFase1Preconstruccion: string;
  valorfase2ConstruccionObra: string;
  nombreEntidadContratistaSupervisorInterventoria: string;
  nombreEntidadContratistaObra: string;
  fechaActaInicioConstruccion: any;
  fechaPrevistaTerminacion: any;
  obsConEspeciales: string;
  plazoActualContratoMeses: number;
  plazoActualContratoDias: number;
  plazoEjecucionPreConstruccionMeses: number;
  plazoEjecucionPreConstruccionDias: number;
  plazoEjecucionConstrM: number;
  plazoEjecucionConstrD: number;
  observacionID: any;
  contrato?: any;
  contratoObservacionId?: any;

  fechaSesionString: string;
  fechaSesion: Date;
  fechaCreacion: Date;
  construccionObservacionId: any;
  contratoConstruccionId: any;
  esActa: any;
  esSupervision: any;
  tipoObservacionConstruccion: any;
  realizoPeticion: boolean = false;
  objeto: any;
  valorProponente: any;
  numeroIdentificacionRepresentanteContratistaInterventoria: any;
  estaEditando = false;
  constructor(private router: Router, private activatedRoute: ActivatedRoute, public dialog: MatDialog, private fb: FormBuilder, private services: ActaInicioConstruccionService, private gestionarActaSvc: GestionarActPreConstrFUnoService) { }
  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
    });
    this.loadConditionals();
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
  loadConditionals() {
    if (localStorage.getItem("editable") == "true") {
      this.editable = true;
      this.title = 'Ver detalle/Editar';
    }
    else {
      this.editable = false;
      this.title = 'Validar';
    }
  }
  loadData(id) {
    this.services.GetVistaGenerarActaInicio(id).subscribe((data: any) => {
      /*Titulo*/
      this.contratoCode = data.numeroContrato;
      this.fechaAprobacionSupervisor = data.fechaAprobacionRequisitosSupervisor;
      /*Cuadro 1*/
      this.vigenciaContrato = data.vigenciaContrato;
      this.fechaFirmaContrato = data.fechaFirmaContrato;
      this.numeroDRP1 = data.numeroDRP1;
      this.fechaGeneracionDRP1 = data.fechaGeneracionDRP1;
      this.numeroDRP2 = data.numeroDRP2;
      this.objeto = data.objeto;
      this.fechaGeneracionDRP2 = data.fechaGeneracionDRP2;
      this.fechaAprobacionGarantiaPoliza = data.fechaAprobacionGarantiaPoliza;
      this.observacionOConsideracionesEspeciales = data.objeto;
      this.numeroIdentificacionRepresentanteContratistaInterventoria = data.numeroIdentificacionRepresentanteContratistaInterventoria;
      this.valorInicialContrato = data.valorInicialContrato;
      this.valorActualContrato = data.valorActualContrato;
      this.valorFase1Preconstruccion = data.valorFase1Preconstruccion;
      this.valorfase2ConstruccionObra = data.valorfase2ConstruccionObra;
      this.nombreEntidadContratistaSupervisorInterventoria = data.nombreEntidadContratistaSupervisorInterventoria;
      this.nombreEntidadContratistaObra = data.nombreEntidadContratistaObra;
      this.valorProponente = data.proponenteCodigo;
      /*Campo de texto no editable*/
      this.fechaActaInicioConstruccion = data.fechaActaInicioFase2DateTime;
      this.fechaPrevistaTerminacion = data.fechaPrevistaTerminacionDateTime;
      this.plazoActualContratoMeses = data.plazoActualContratoMeses;
      this.plazoActualContratoDias = data.plazoActualContratoDias;
      this.plazoEjecucionPreConstruccionMeses = data.plazoFase1PreMeses;
      this.plazoEjecucionPreConstruccionDias = data.plazoFase1PreDias;
      this.obsConEspeciales = data.observacionOConsideracionesEspeciales;
      this.plazoEjecucionConstrM = data.plazoFase2ConstruccionDias;
      this.plazoEjecucionConstrD = data.plazoFase2ConstruccionMeses;
      this.contrato = data.contrato;
      this.contratoConstruccionId = data.contrato.contratoConstruccion[0].contratoConstruccionId;
      this.loadDataObservaciones(data.contrato.contratoConstruccion[0].contratoConstruccionId);
    });
    this.contratoId = id;
  }
  loadDataObservaciones(id) {
    
    if (localStorage.getItem("editable") == "true") {
      this.services.GetConstruccionObservacionByIdContratoConstruccionId(id,true).subscribe((data0:any)=>{
        this.addressForm.get('observaciones').setValue(data0.observaciones);
        this.addressForm.get('tieneObservaciones').setValue(data0.esActa);
        this.loadData2(data0);
      });
      /* Versión Anterior
      this.services.GetContratoObservacionByIdContratoId(id, true).subscribe(data0 => {
        //this.addressForm.get('tieneObservaciones').setValue(data0.esActa);
        this.addressForm.get('observaciones').setValue(data0.observaciones);
        this.loadData2(data0);
        this.fechaCreacion = data0.fechaCreacion;
      });
      */
    }
    else {
      this.services.GetContratoObservacionByIdContratoId(id, true).subscribe(data2 => {
        this.fechaCreacion = data2.fechaCreacion;
      });
    }
  }
  loadData2(data) {
    console.log(data);
    //this.contratoConstruccionId = data.contratoConstruccionId;
    this.contratoObservacionId = data.contratoObservacionId;
    this.esActa = data.esActa;
    this.esSupervision = data.esSupervision;
    this.fechaCreacion = data.fechaCreacion;
    this.tipoObservacionConstruccion = data.tipoObservacionConstruccion;
    this.construccionObservacionId = data.construccionObservacionId;
  }
  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '25em',
      data: { modalTitle, modalText }
    });
  }
  editorStyle = {
    height: '45px',
    overflow: 'auto'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  crearFormulario() {
    return this.fb.group({
      tieneObservaciones: [null, Validators.required],
      observaciones: [null, Validators.required],
    })
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

  generarActaSuscrita() {
    this.services.GetPlantillaActaInicio(this.contratoId).subscribe(resp => {
      const documento = `Prueba.pdf`; // Valor de prueba
      const text = documento,
        blob = new Blob([resp], { type: 'application/pdf' }),
        anchor = document.createElement('a');
      anchor.download = documento;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
      anchor.click();
    });
  }
  descargarActaDesdeTabla() {
    this.gestionarActaSvc.GetActaByIdPerfil(this.contratoId, 'True').subscribe(resp => {
      const documento = `${this.contratoCode}.pdf`; // Valor de prueba
      const text = documento,
        blob = new Blob([resp], { type: 'application/pdf' }),
        anchor = document.createElement('a');
      anchor.download = documento;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
      anchor.click();
    });
  }
  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    this.fechaSesion = new Date(this.fechaCreacion);
    this.fechaSesionString = `${this.fechaSesion.getFullYear()}-${this.fechaSesion.getMonth() + 1}-${this.fechaSesion.getDate()}`;
    
    const objetoContrato = {
      conObervacionesActaFase2: this.addressForm.value.tieneObservaciones,
      contratoId: this.contratoId,

      contratoObservacion: [{

        contratoId : this.contratoId,
        observaciones: this.addressForm.value.observaciones,
        esActaFase2: true,
        contratoObservacionId: this.contratoObservacionId,

      }]
    }
    let crearObservacionArreglo;
    let editarObservacionArreglo;
    crearObservacionArreglo=[{
      'contratoId':parseInt(this.contratoId),
      'contratoConstruccionId': this.contratoConstruccionId,
      'observaciones': this.addressForm.value.observaciones,
      'esSupervision':true,
      'esActa': true
    }];
    console.log(this.contratoConstruccionId)
    editarObservacionArreglo=[{
      'contratoId':parseInt(this.contratoId),
      'contratoConstruccionId': this.contratoConstruccionId,
      'construccionObservacionId':this.construccionObservacionId,
      'observaciones': this.addressForm.value.observaciones,
      'esSupervision':true,
      'esActa': true
    }];
    const arrayCrearContratoObservacion = {
      contratoId: parseInt(this.contratoId),
      contratoConstruccionId: this.contratoConstruccionId,
      construccionObservacion : crearObservacionArreglo
    }
    const arrayEditarContratoObservacion = {
      contratoId: parseInt(this.contratoId),
      contratoConstruccionId: this.contratoConstruccionId,
      construccionObservacion : editarObservacionArreglo
    }
    if (localStorage.getItem("editable") == "false") {
      this.services.CreateEditObservacionesActaInicioConstruccion(arrayCrearContratoObservacion).subscribe(resp => {
        if (resp.code == "200") {
          if (this.addressForm.value.tieneObservaciones == true) {
            if (localStorage.getItem("origin") == "interventoria") {
              this.services.CambiarEstadoActa(this.contratoId, "8", "usr2").subscribe(data0 => {
                this.realizoPeticion = true;
                this.openDialog("", `<b>${resp.message}</b>`);
                this.router.navigate(['/generarActaInicioConstruccion']);
              });
            }
            else {
              this.services.CambiarEstadoActa(this.contratoId, "16", "usr2").subscribe(data0 => {
                this.realizoPeticion = true;
                this.openDialog("", `<b>${resp.message}</b>`);
                this.router.navigate(['/generarActaInicioConstruccion']);
              });
            }
          }
          else {
            if (localStorage.getItem("origin") == "interventoria") {
              this.services.CambiarEstadoActa(this.contratoId, "5", "usr2").subscribe(data1 => {
                this.realizoPeticion = true;
                this.openDialog("", `<b>${resp.message}</b>`);
                this.router.navigate(['/generarActaInicioConstruccion']);
              });
            }
            else {
              this.services.CambiarEstadoActa(this.contratoId, "15", "usr2").subscribe(data1 => {
                this.realizoPeticion = true;
                this.openDialog("", `<b>${resp.message}</b>`);
                this.router.navigate(['/generarActaInicioConstruccion']);
              });
            }
          }
        }
        else {
          this.openDialog("",`<b>${resp.message}</b>`);
        }
      });
    }
    else {
      this.services.CreateEditObservacionesActaInicioConstruccion(arrayEditarContratoObservacion).subscribe(resp => {
        if (resp.code == "200") {
          if (this.addressForm.value.tieneObservaciones == true) {
            if (localStorage.getItem("origin") == "interventoria") {
              this.services.CambiarEstadoActa(this.contratoId, "8", "usr2").subscribe(data0 => {
                this.realizoPeticion = true;
                this.openDialog("", `<b>${resp.message}</b>`);
                this.router.navigate(['/generarActaInicioConstruccion']);
              });
            }
            else {
              this.services.CambiarEstadoActa(this.contratoId, "16", "usr2").subscribe(data0 => {
                this.realizoPeticion = true;
                this.openDialog("", `<b>${resp.message}</b>`);
                this.router.navigate(['/generarActaInicioConstruccion']);
              });
            }
          }
          else {
            if (localStorage.getItem("origin") == "interventoria") {
              this.services.CambiarEstadoActa(this.contratoId, "5", "usr2").subscribe(data1 => {
                this.realizoPeticion = true;
                this.openDialog("",`<b>${resp.message}</b>`);
                this.router.navigate(['/generarActaInicioConstruccion']);
              });
            }
            else {
              this.services.CambiarEstadoActa(this.contratoId, "15", "usr2").subscribe(data1 => {
                this.realizoPeticion = true;
                this.openDialog("", `<b>${resp.message}</b>`);
                this.router.navigate(['/generarActaInicioConstruccion']);
              });
            }
          }
        }
        else {
          this.openDialog("", `<b>${resp.message}</b>`);
        }
      });
    }
    console.log(this.addressForm.value);
  }

}
