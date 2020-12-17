import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { Contrato, ContratoObservacionElement, EditContrato, GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';


@Component({
  selector: 'app-ver-detalle-editar-acta-ini-f-i-prc',
  templateUrl: './ver-detalle-editar-acta-ini-f-i-prc.component.html',
  styleUrls: ['./ver-detalle-editar-acta-ini-f-i-prc.component.scss']
})
export class VerDetalleEditarActaIniFIPreconstruccioComponent implements OnInit, OnDestroy {
  maxDate: Date;
  maxDate2: Date;
  public idContrato;
  public numContrato;
  public fechaFirmaContrato;
  public conObservaciones: boolean;

  public rolAsignado;
  public opcion;

  public contratacionId;
  public fechaTramite;
  public tipoContratoCodigo;
  public estadoDocumentoCodigo;
  public fechaEnvioFirma;
  public fechaFirmaContratista;
  public fechaFirmaFiduciaria;

  fechaSesionString: string;
  fechaSesion: Date;

  fechaSesionString2: string;
  fechaSesion2: Date;

  addressForm = this.fb.group({});
  addressForm2 = this.fb.group({});
  dataDialog: {
    modalTitle: string,
    modalText: string
  };
  public observacionesOn: boolean;
  indexContratacionID: any;
  indexObservacionFinal: any;
  fechaAprobacionRequisitos: any;
  numDRP: any;
  fechaDRP: any;
  objeto: any;
  valorIni: any;
  fechaAprobGarantiaPoliza: any;
  nitContratistaInterventoria: any;
  vigenciaContrato: any;
  valorFUno: any;
  valorFDos: any;
  nomEntidadContratistaIntervn: any;
  numIdContratistaObra: any;
  diasPlazoIni: any;
  mesPlazoIni: any;

  realizoPeticion: boolean = false;
  numIdRepresentanteLegal: any;
  nomRepresentanteLegal: any;
  tipoProponente: any;
  dataSupervisor: boolean = false;
  numIdentifiacionSupervisor: string;
  nomSupervisor: string;
  conObervacionesActa: any;
  observacionesActaFase1: any;
  fechaCreacion: any;
  elementsObservacion: any;
  tipoCodigo: any;
  esActa: any;
  anotacionesSupervisor: boolean;
  observacionSupervisor: any;
  constructor(private router: Router, public dialog: MatDialog, private fb: FormBuilder, private activatedRoute: ActivatedRoute, private service: GestionarActPreConstrFUnoService) {
    this.maxDate = new Date();
    this.maxDate2 = new Date();
  }
  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
    this.addressForm2 = this.crearFormulario2();
    this.cargarRol();
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
      this.loadObservaciones(param.id);
      this.idContrato = param.id;
    });
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
  loadData(id) {
    this.service.GetContratoByContratoId(id).subscribe((data: any) => {
      this.cargarDataParaInsercion(data);
      this.verObservaciones(data.conObervacionesActa);
      //Datos correspondientes al formulario
      this.addressForm.get('fechaActaInicioFUnoPreconstruccion').setValue(data.fechaActaInicioFase1);
      this.addressForm.get('fechaPrevistaTerminacion').setValue(data.fechaTerminacion);
      this.addressForm.get('mesPlazoEjFase1').setValue(data.plazoFase1PreMeses);
      this.addressForm.get('diasPlazoEjFase1').setValue(data.plazoFase1PreDias);
      this.addressForm.get('mesPlazoEjFase2').setValue(data.plazoFase2ConstruccionMeses);
      this.addressForm.get('diasPlazoEjFase2').setValue(data.plazoFase2ConstruccionDias);
    });
    this.idContrato = id;
  }
  loadObservaciones(id) {
    this.service.GetListContratoObservacionByContratoId(id).subscribe((data: any) => {
      this.elementsObservacion = data;
      for (let i = 0; i < data.length; i++) {
        if (data[i].esSupervision == false) {

          this.esActa = data[i].esActa;
          this.conObervacionesActa = data[i].esActaFase1;
          this.observacionesActaFase1 = data[i].observaciones;
          this.fechaCreacion = data[i].fechaCreacion;
          this.indexObservacionFinal = data[i].contratoObservacionId;
          //seteo el queseenvia jflorez
          this.indexContratacionID = data[i].contratoObservacionId;
          this.addressForm.get('observacionesEspeciales').setValue(this.observacionesActaFase1);
        }
      }
      // Anotaciones Supervisor
      for (let i = 0; i < data.length; i++) {
        if (data[i].esSupervision == true) {
          this.anotacionesSupervisor = true;
          this.observacionSupervisor = data[i].observaciones;
        }
      }
    });
  }

  cargarRol() {
    this.rolAsignado = JSON.parse(localStorage.getItem("actualUser")).rol[0].perfilId;
    //jflorez, el perfil 11 es interventor.....
    if (this.rolAsignado == 11) {
      this.opcion = 1;
    }
    else {
      this.opcion = 2;
    }
  }
  cargarDataParaInsercion(data) {
    this.numContrato = data.numeroContrato;
    this.fechaAprobacionRequisitos = data.fechaAprobacionRequisitosSupervisor;
    this.fechaFirmaContrato = data.fechaFirmaContrato;
    this.contratacionId = data.contratacionId;
    this.fechaTramite = data.fechaTramite;
    this.tipoContratoCodigo = data.tipoContratoCodigo;
    this.estadoDocumentoCodigo = data.estadoDocumentoCodigo;
    this.fechaEnvioFirma = data.fechaEnvioFirma;
    this.fechaFirmaContratista = data.fechaFirmaContratista;
    this.fechaFirmaFiduciaria = data.fechaFirmaFiduciaria;
    this.numDRP = data.contratacion.disponibilidadPresupuestal[0].numeroDrp;
    this.fechaDRP = data.contratacion.disponibilidadPresupuestal[0].fechaCreacion;
    this.objeto = data.contratacion.disponibilidadPresupuestal[0].objeto;
    this.valorIni = data.contratacion.disponibilidadPresupuestal[0].valorSolicitud;
    this.numIdRepresentanteLegal = data.contratacion.contratista.representanteLegalNumeroIdentificacion;
    this.nomRepresentanteLegal = data.contratacion.contratista.representanteLegal;
    this.nitContratistaInterventoria = data.contratacion.contratista.numeroIdentificacion;
    this.fechaAprobGarantiaPoliza = data.contratoPoliza[0].fechaAprobacion;
    this.vigenciaContrato = data.fechaTramite;
    this.valorFUno = data.valorFase1;
    this.valorFDos = data.valorFase2;
    this.nomEntidadContratistaIntervn = data.contratacion.contratista.nombre;
    this.numIdContratistaObra = data.contratacion.contratista.representanteLegalNumeroIdentificacion
    this.mesPlazoIni = data.contratacion.disponibilidadPresupuestal[0].plazoMeses;
    this.diasPlazoIni = data.contratacion.disponibilidadPresupuestal[0].plazoDias;
    this.tipoProponente = data.contratacion.contratista.tipoProponenteCodigo;
    this.numIdentifiacionSupervisor = data.usuarioInterventoria.numeroIdentificacion;
    this.nomSupervisor = data.usuarioInterventoria.nombres + " " + data.usuarioInterventoria.apellidos;
    this.tipoCodigo = data.contratacion.tipoSolicitudCodigo;
    if (this.opcion == 1) {//jflorez, el perfil 11 es interventor, no supervisor, revisar esta condición
      this.dataSupervisor = true;
      this.numIdentifiacionSupervisor = data.usuarioInterventoria.numeroIdentificacion;
      this.nomSupervisor = data.usuarioInterventoria.nombres + " " + data.usuarioInterventoria.apellidos;
    }
    //jflorez, no puededeterminarel id de esta forma si no esta seteando el formulario, podria enviar a edición a unoque no corresponde
    /*for (let i = 0; i < data.contratoObservacion.length; i++) {
      this.indexContratacionID = data.contratoObservacion[i].contratoObservacionId;
    }*/
  }

  generarActaSuscrita() {
    alert("genera PDf");
  }
  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '37em',
      data: { modalTitle, modalText }
    });
  }
  openDialog2(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '25em',
      data: { modalTitle, modalText }
    });
  }
  editorStyle = {
    height: '50%'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  verObservaciones(observaciones) {
    if (observaciones == true) {
      this.conObservaciones = true;
    }
    else {
      this.conObservaciones = false;
    }
  }
  generarFechaRestante() {
    let newdate = new Date(this.addressForm.value.fechaActaInicioFUnoPreconstruccion);
    newdate.setDate(newdate.getDate() + (this.mesPlazoIni * 30.44));
    let newDateFinal = new Date(newdate);
    newDateFinal.setDate(newDateFinal.getDate() + this.diasPlazoIni)
    console.log(newDateFinal);
    this.addressForm.get('fechaPrevistaTerminacion').setValue(newDateFinal);
  }
  crearFormulario() {
    return this.fb.group({
      fechaActaInicioFUnoPreconstruccion: [Date(), Validators.required],
      fechaPrevistaTerminacion: [Date(), Validators.required],
      mesPlazoEjFase1: ["", Validators.required],
      diasPlazoEjFase1: ["", Validators.required],
      mesPlazoEjFase2: ["", Validators.required],
      diasPlazoEjFase2: ["", Validators.required],
      observacionesEspeciales: [""]
    })
  }
  crearFormulario2() {
    return this.fb.group({
      tieneObservaciones: ['', Validators.required],
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
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p>');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li>');

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
  number(e: { keyCode: any; }) {
    const tecla = e.keyCode;
    if (tecla === 8) { return true; } // Tecla de retroceso (para poder borrar)
    if (tecla === 48) { return true; } // 0
    if (tecla === 49) { return true; } // 1
    if (tecla === 50) { return true; } // 2
    if (tecla === 51) { return true; } // 3
    if (tecla === 52) { return true; } // 4
    if (tecla === 53) { return true; } // 5
    if (tecla === 54) { return true; } // 6
    if (tecla === 55) { return true; } // 7
    if (tecla === 56) { return true; } // 8
    if (tecla === 57) { return true; } // 9
    const patron = /1/; // ver nota
    const te = String.fromCharCode(tecla);
    return patron.test(te);
  }
  onSubmit() {
    let esSupervisionBool;
    let fecha = Date.parse(this.addressForm.get('fechaActaInicioFUnoPreconstruccion').value);
    this.fechaSesion = new Date(fecha);
    this.fechaSesionString = `${this.fechaSesion.getFullYear()}-${this.fechaSesion.getMonth() + 1}-${this.fechaSesion.getDate()}`

    let fecha2 = Date.parse(this.addressForm.get('fechaPrevistaTerminacion').value);
    this.fechaSesion2 = new Date(fecha2);
    this.fechaSesionString2 = `${this.fechaSesion2.getFullYear()}-${this.fechaSesion2.getMonth() + 1}-${this.fechaSesion2.getDate()}`

    if (this.addressForm.value.observacionesEspeciales != "" || this.addressForm.value.observacionesEspeciales != null || this.addressForm.value.observacionesEspeciales != undefined) {
      this.observacionesOn = true;
    }
    else {
      this.observacionesOn = false;
    }
    if (this.opcion == 1) {//jflorez, el perfil 11 es interventor no supervisor, revisar
      esSupervisionBool = true;
    }
    else {
      esSupervisionBool = false;
    }
    //compara los meses
    var sumaMeses;
    var sumaDias;
    sumaMeses = parseInt(this.addressForm.value.mesPlazoEjFase1) + parseInt(this.addressForm.value.mesPlazoEjFase2);
    sumaDias = parseInt(this.addressForm.value.diasPlazoEjFase1) + parseInt(this.addressForm.value.diasPlazoEjFase2);
    if (sumaMeses > this.mesPlazoIni || sumaDias > this.diasPlazoIni) {
      this.openDialog('', 'Debe verificar la información ingresada en el campo <b>Plazo de ejecución - fase 1 - Preconstruccion Meses</b>, dado que no coincide con la informacion inicial registrada para el contrato');
    }
    else {
      const arrayObservacion = [{
        'ContratoId': this.idContrato,
        "ContratoObservacionId": this.indexContratacionID,
        "observaciones": this.addressForm.value.observacionesEspeciales,
        'esActa': true,
        'esActaFase1': true,
        'esSupervision': this.rolAsignado==8?true:false//perfil 8 es supervisor
        //'esSupervision': esSupervisionBool
      }];
      const arrayContrato: EditContrato = {
        contratoId: this.idContrato,
        contratacionId: this.contratacionId,
        fechaTramite: this.fechaTramite,
        tipoContratoCodigo: this.tipoContratoCodigo,
        numeroContrato: this.numContrato,
        estadoDocumentoCodigo: this.estadoDocumentoCodigo,
        estado: false,
        fechaEnvioFirma: this.fechaEnvioFirma,
        fechaFirmaContratista: this.fechaFirmaContratista,
        fechaFirmaFiduciaria: this.fechaFirmaFiduciaria,
        fechaFirmaContrato: this.fechaFirmaContrato,
        fechaActaInicioFase1: this.fechaSesionString,
        fechaTerminacion: this.fechaSesionString2,
        plazoFase1PreMeses: this.addressForm.value.mesPlazoEjFase1,
        plazoFase1PreDias: this.addressForm.value.diasPlazoEjFase1,
        plazoFase2ConstruccionMeses: this.addressForm.value.mesPlazoEjFase2,
        plazoFase2ConstruccionDias: this.addressForm.value.diasPlazoEjFase2,
        observaciones: this.addressForm.value.observacionesEspeciales,
        conObervacionesActa: true,
        registroCompleto: false,
        contratoConstruccion: [],
        contratoObservacion: arrayObservacion,
        contratoPerfil: [],
        contratoPoliza: []
      };
      this.service.EditContrato(arrayContrato).subscribe(data => {
        if (data.code == "200") {
          if (localStorage.getItem("origin") == "obra") {
            this.service.CambiarEstadoActa(this.idContrato, "14").subscribe(data0 => {
              this.openDialog2('', `<b>${data.message}</b>`);
              this.router.navigate(['/generarActaInicioFaseIPreconstruccion']);
            });
          }
          else {
            this.service.CambiarEstadoActa(this.idContrato, "2").subscribe(data1 => {
              this.openDialog2('', `<b>${data.message}</b>`);
              this.router.navigate(['/generarActaInicioFaseIPreconstruccion']);
            });
          }
        }
        else {
          this.openDialog2('', `<b>${data.message}</b>`);
        }
      });
      this.realizoPeticion = true;
    }
    console.log(this.addressForm.value);
  }

}
