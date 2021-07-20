import { delay } from 'rxjs/operators';
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
  contrato: any;
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
  plazoDiasContrato = 0;
  plazoMesesFase1 = 0;
  plazoMesesFase2 = 0;
  estaEditando = false;
  obsSupervisor: any;
  observacionesUltimas: any;

  constructor(private router: Router, public dialog: MatDialog, private fb: FormBuilder, private activatedRoute: ActivatedRoute, private service: GestionarActPreConstrFUnoService) {
    this.maxDate = new Date();
    this.maxDate2 = new Date();
    this.addressForm = this.crearFormulario();
    this.addressForm2 = this.crearFormulario2();

    this.cargarRol();
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
      this.loadObservaciones(param.id);
      this.idContrato = param.id;
    });

    /*this.addressForm.get( 'mesPlazoEjFase1' ).valueChanges
    .pipe(
      delay( 1000 )
    )
    .subscribe(
      value => {
        this.addressForm.get( 'fechaPrevistaTerminacion' ).setValue( null );
        if ( this.addressForm.get( 'fechaActaInicioFUnoPreconstruccion' ).value !== null ) {
          let newdate = new Date( this.addressForm.get( 'fechaActaInicioFUnoPreconstruccion' ).value );
          newdate.setDate( newdate.getDate() + ( Number( value ) * 30 ) );
          this.addressForm.get( 'fechaPrevistaTerminacion' ).setValue( newdate );
        }
        if ( this.contrato !== undefined && value !== null ) {
          const mesesPlazoInicial = this.contrato.contratacion.disponibilidadPresupuestal[0].plazoMeses;
          const diasPlazoInicial = this.contrato.contratacion.disponibilidadPresupuestal[0].plazoDias;
          this.plazoMesesFase1 = value;
          this.plazoMesesFase2 = this.addressForm.get( 'diasPlazoEjFase1' ).value;
          this.service.getFiferenciaMesesDias( mesesPlazoInicial, diasPlazoInicial, this.plazoMesesFase1, this.plazoMesesFase2 )
            .subscribe(
              response => {
                this.addressForm.get( 'mesPlazoEjFase2' ).setValue( response[0] );
                this.addressForm.get('diasPlazoEjFase2').setValue( response[1] );
              }
            );
        }
      }
    );
    this.addressForm.get( 'diasPlazoEjFase1' ).valueChanges
    .pipe(
      delay( 1000 )
    )
    .subscribe(
      value => {
        if ( this.addressForm.get( 'mesPlazoEjFase1' ).value !== null ) {
          if ( this.addressForm.get( 'mesPlazoEjFase1' ).value === 0 && value === 0 ) {
            this.addressForm.get( 'diasPlazoEjFase1' ).setValue( 1 );
            this.openDialog( '', '<b>No se puede tener 0 meses y 0 días de ejecución, para continuar verifique por favor.</b>' )
          }
        }
        if ( this.addressForm.get( 'fechaActaInicioFUnoPreconstruccion' ).value !== null && this.addressForm.get( 'mesPlazoEjFase1' ).value !== null ) {
          let newdate = new Date( this.addressForm.get( 'fechaActaInicioFUnoPreconstruccion' ).value );
          newdate.setDate(newdate.getDate() + ( ( this.addressForm.get( 'mesPlazoEjFase1' ).value * 30 ) + Number( value ) ));
          this.addressForm.get( 'fechaPrevistaTerminacion' ).setValue( newdate );
        }
        if ( this.contrato !== undefined && value !== null ) {
          const mesesPlazoInicial = this.contrato.contratacion.disponibilidadPresupuestal[0].plazoMeses;
          const diasPlazoInicial = this.contrato.contratacion.disponibilidadPresupuestal[0].plazoDias;
          this.plazoMesesFase1 = this.addressForm.get( 'mesPlazoEjFase1' ).value;
          this.plazoMesesFase2 = value;
          this.service.getFiferenciaMesesDias( mesesPlazoInicial, diasPlazoInicial, this.plazoMesesFase1, this.plazoMesesFase2 )
            .subscribe(
              response => {
                this.addressForm.get( 'mesPlazoEjFase2' ).setValue( response[0] );
                this.addressForm.get('diasPlazoEjFase2').setValue( response[1] );
              }
            );
        }
      }
    );*/
  }
  ngOnInit(): void {
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
      this.contrato = data;
      // console.log( this.contrato );
      this.cargarDataParaInsercion(data);
      this.verObservaciones(data.conObervacionesActa);
      //Datos correspondientes al formulario
      this.addressForm.get('fechaActaInicioFUnoPreconstruccion').setValue(data.fechaActaInicioFase1);
      this.addressForm.get('fechaPrevistaTerminacion').setValue(data.fechaTerminacion);
      this.addressForm.get('mesPlazoEjFase1').setValue(data.plazoFase1PreMeses);
      this.addressForm.get('diasPlazoEjFase1').setValue(data.plazoFase1PreDias);
      this.addressForm.get( 'observacionesEspeciales' ).setValue( data.observacionConsideracionesEspeciales !== undefined ? data.observacionConsideracionesEspeciales : null );
    });
    this.idContrato = id;
  }
  loadObservaciones(id) {
    this.service.GetListContratoObservacionByContratoId(id).subscribe((data: any) => {
      const obsArraySupervisor = [];
      const obsArrayConsideraciones = [];
      data.forEach( obs => {
        if ( obs.esSupervision === true ) {
          obsArraySupervisor.push( obs );
        }
        if ( obs.esSupervision === false ) {
          obsArrayConsideraciones.push( obs );
        }
      } );

      if ( obsArraySupervisor.length > 0 ) {
        this.obsSupervisor = obsArraySupervisor[ obsArraySupervisor.length - 1 ];
      }

      if ( obsArrayConsideraciones.length > 0 ) {
        this.observacionesUltimas = obsArrayConsideraciones[ obsArrayConsideraciones.length - 1 ];
      }
    });
  }

  cargarRol() {
    this.rolAsignado = JSON.parse(localStorage.getItem("actualUser")).rol[0].perfilId;
    //jflorez, el perfil 11 es interventor.....
    if (this.rolAsignado == 11 ) {
      this.opcion = 1;
    }
    else {
      this.opcion = 2;
    }
  }
  cargarDataParaInsercion(data) {
    console.log( data );
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
    this.mesPlazoIni = data.contratacion.plazoContratacion.plazoMeses;
    this.diasPlazoIni = data.contratacion.plazoContratacion.plazoDias;
    this.tipoProponente = data.contratacion.contratista.tipoProponenteCodigo;
    this.numIdentifiacionSupervisor = data.usuarioInterventoria.numeroIdentificacion;
    this.nomSupervisor = data.usuarioInterventoria.primerNombre + " " + data.usuarioInterventoria.primerApellido;
    this.tipoCodigo = data.contratacion.tipoSolicitudCodigo;
    if (this.opcion == 1) {//jflorez, el perfil 11 es interventor, no supervisor, revisar esta condición
      this.dataSupervisor = true;
      this.numIdentifiacionSupervisor = data.usuarioInterventoria.numeroIdentificacion;
      this.nomSupervisor = data.usuarioInterventoria.primerNombre + " " + data.usuarioInterventoria.primerApellido;
    }
    //jflorez, no puededeterminarel id de esta forma si no esta seteando el formulario, podria enviar a edición a unoque no corresponde
    /*for (let i = 0; i < data.contratoObservacion.length; i++) {
      this.indexContratacionID = data.contratoObservacion[i].contratoObservacionId;
    }*/
  }

  getValueMeses( value: number ) {
    if ( value !== null && this.mesPlazoIni !== undefined ) {
      if ( value > this.mesPlazoIni ) {
        this.addressForm.get( 'mesPlazoEjFase1' ).setValue( this.mesPlazoIni );
      }
    }
  }

  getValueDias( value: number ) {
    if ( value !== null && this.diasPlazoIni !== undefined ) {
      if ( value > this.diasPlazoIni ) {
        this.addressForm.get( 'diasPlazoEjFase1' ).setValue( this.diasPlazoIni );
      }
    }
  }

  getMonth( value: number ) {
    this.addressForm.get( 'fechaPrevistaTerminacion' ).setValue( null );
    if ( this.addressForm.get( 'fechaActaInicioFUnoPreconstruccion' ).value !== null ) {
      let newdate = new Date( this.addressForm.get( 'fechaActaInicioFUnoPreconstruccion' ).value );
      newdate.setDate( newdate.getDate() + ( Number( value ) * 30 ) );
      this.addressForm.get( 'fechaPrevistaTerminacion' ).setValue( newdate );
    }
    if ( this.contrato !== undefined && value !== null ) {
      const mesesPlazoInicial = this.contrato.contratacion.plazoContratacion.plazoMeses;
      const diasPlazoInicial = this.contrato.contratacion.plazoContratacion.plazoDias;
      this.plazoMesesFase1 = value;
      this.plazoMesesFase2 = this.addressForm.get( 'diasPlazoEjFase1' ).value;
      this.service.getFiferenciaMesesDias( mesesPlazoInicial, diasPlazoInicial, this.plazoMesesFase1, this.plazoMesesFase2 )
        .subscribe(
          response => {
            this.addressForm.get( 'mesPlazoEjFase2' ).setValue( response[0] );
            this.addressForm.get('diasPlazoEjFase2').setValue( response[1] );
          }
        );
    }
  }

  getDays( value: number ) {
    if ( this.addressForm.get( 'mesPlazoEjFase1' ).value !== null ) {
      if ( this.addressForm.get( 'mesPlazoEjFase1' ).value === 0 && value === 0 ) {
        this.addressForm.get( 'diasPlazoEjFase1' ).setValue( 1 );
        this.openDialog( '', '<b>No se puede tener 0 meses y 0 días de ejecución, para continuar verifique por favor.</b>' )
      }
    }
    if ( this.addressForm.get( 'fechaActaInicioFUnoPreconstruccion' ).value !== null && this.addressForm.get( 'mesPlazoEjFase1' ).value !== null ) {
      let newdate = new Date( this.addressForm.get( 'fechaActaInicioFUnoPreconstruccion' ).value );
      newdate.setDate(newdate.getDate() + ( ( this.addressForm.get( 'mesPlazoEjFase1' ).value * 30 ) + Number( value ) ));
      this.addressForm.get( 'fechaPrevistaTerminacion' ).setValue( newdate );
    }
    if ( this.contrato !== undefined && value !== null ) {
      const mesesPlazoInicial = this.contrato.contratacion.plazoContratacion.plazoMeses;
      const diasPlazoInicial = this.contrato.contratacion.plazoContratacion.plazoDias;
      this.plazoMesesFase1 = this.addressForm.get( 'mesPlazoEjFase1' ).value;
      this.plazoMesesFase2 = value;
      this.service.getFiferenciaMesesDias( mesesPlazoInicial, diasPlazoInicial, this.plazoMesesFase1, this.plazoMesesFase2 )
        .subscribe(
          response => {
            this.addressForm.get( 'mesPlazoEjFase2' ).setValue( response[0] );
            this.addressForm.get('diasPlazoEjFase2').setValue( response[1] );
          }
        );
    }
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
    // console.log(newDateFinal);
    this.addressForm.get('fechaPrevistaTerminacion').setValue(newDateFinal);
  }
  crearFormulario() {
    return this.fb.group({
      fechaActaInicioFUnoPreconstruccion: [Date(), Validators.required],
      fechaPrevistaTerminacion: [Date(), Validators.required],
      mesPlazoEjFase1: ["", Validators.required],
      diasPlazoEjFase1: ["", Validators.required],
      mesPlazoEjFase2: [{ value: "", disabled: true }, Validators.required],
      diasPlazoEjFase2: [{ value: "", disabled: true }, Validators.required],
      observacionesEspeciales: [ null ]
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
        e.editor.deleteText(n - 1, e.editor.getLength());
    }
  }

  textoLimpio( evento: any, n: number ) {
      if ( evento !== undefined ) {
          return evento.getLength() > n ? n : evento.getLength();
      } else {
          return 0;
      }
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
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
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
        'esSupervision': this.rolAsignado !== 11 ? true : false//perfil 8 es supervisor
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
        plazoFase1PreMeses: this.addressForm.get( 'mesPlazoEjFase1' ).value,
        plazoFase1PreDias: this.addressForm.get( 'diasPlazoEjFase1' ).value,
        plazoFase2ConstruccionMeses: this.valorFDos !== 0 ? this.addressForm.get( 'mesPlazoEjFase2' ).value : null,
        plazoFase2ConstruccionDias: this.valorFDos !== 0 ? this.addressForm.get( 'diasPlazoEjFase2' ).value : null,
        observacionConsideracionesEspeciales: this.addressForm.get( 'observacionesEspeciales' ).value,
        conObervacionesActa: true,
        registroCompleto: false,
        contratoConstruccion: [],
        contratoPerfil: [],
        contratoPoliza: []
      };
      console.log( arrayContrato );
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
    // console.log(this.addressForm.value);
  }

}
