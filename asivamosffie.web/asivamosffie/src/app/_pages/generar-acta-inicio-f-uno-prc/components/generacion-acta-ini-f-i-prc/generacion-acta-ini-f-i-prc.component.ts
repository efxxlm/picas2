import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { delay } from 'rxjs/operators';
import { EditContrato, GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-generacion-acta-ini-f-i-prc',
  templateUrl: './generacion-acta-ini-f-i-prc.component.html',
  styleUrls: ['./generacion-acta-ini-f-i-prc.component.scss']
})
export class GeneracionActaIniFIPreconstruccionComponent implements OnInit, OnDestroy {
  maxDate: Date;
  maxDate2: Date;
  public idContrato;
  public numContrato;
  public fechaContrato = "20/06/2020";//valor quemado
  public fechaFirmaContrato;
  public contratacionId;
  public fechaTramite;
  public tipoContratoCodigo;
  public fechaEnvioFirma;
  public estadoDocumentoCodigo;
  public fechaFirmaContratista;
  public fechaFirmaFiduciaria;
  public mesPlazoIni: number;
  public diasPlazoIni: number;
  public observacionesOn: boolean;
  addressForm: FormGroup;
  dataDialog: {
    modalTitle: string,
    modalText: string
  };
  fechaAprobacionRequisitos: any;
  numDRP: any;
  fechaDRP: any;
  objeto: any;
  valorIni: any;
  nitContratistaInterventoria: any;
  nomContratista: any;
  fechaAprobGarantiaPoliza: any;
  vigenciaContrato: any;
  valorFUno: any;
  nomEntidadContratistaIntervn: any;
  valorFDos: any;
  numIdContratistaObra: any;
  realizoPeticion: boolean = false;
  numIdRepresentanteLegal: any;
  nomRepresentanteLegal: any;
  tipoProponente: any;
  dataSupervisor: boolean = false;
  numIdentifiacionSupervisor: string;
  nomSupervisor: string;
  esRojo: boolean = false;
  rolAsignado: any;
  ocpion: number;
  tipoCodigo: any;
  numIdentificacionRepLegalInterventoria: any;
  nomRepresentanteLegalContrInterventoria: any;
  plazoMesesFase1 = 0;
  plazoMesesFase2 = 0;
  contrato: any;
  estaEditando = false;

  constructor(private router: Router, private activatedRoute: ActivatedRoute, public dialog: MatDialog, private fb: FormBuilder, private service: GestionarActPreConstrFUnoService) {
    this.addressForm = this.crearFormulario();
    this.addressForm.get( 'mesPlazoEjFase1' ).valueChanges
    .pipe(
      delay( 1000 )
    )
    .subscribe(
      value => {
        this.addressForm.get( 'fechaPrevistaTerminacion' ).setValue( null );
        this.addressForm.get( 'diasPlazoEjFase1' ).setValue( null );
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
          if ( this.plazoMesesFase1 > 0 ) {
            this.service.getFiferenciaMesesDias( mesesPlazoInicial, diasPlazoInicial, this.plazoMesesFase1, this.plazoMesesFase2 )
              .subscribe(
                response => {
                  this.addressForm.get( 'mesPlazoEjFase2' ).setValue( response[0] );
                  this.addressForm.get('diasPlazoEjFase2').setValue( response[1] );
                }
              );
          }
        }
      }
    );
    this.maxDate = new Date();
    this.maxDate2 = new Date();
  }
  ngOnDestroy(): void {
    if (this.addressForm.dirty === true && this.realizoPeticion === false) {
      this.openDialogConfirmar('', '¿Desea guardar la información registrada?');
    }
  }
  ngOnInit(): void {
    this.cargarRol();
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
    });
  }

  getSizeInput( value: any ) {
    if ( value !== null ) {
      return value.toString().length;
    } else {
      return 0;
    }
  }

  cargarRol() {
    this.rolAsignado = JSON.parse(localStorage.getItem("actualUser")).rol[0].perfilId;
    if (this.rolAsignado == 11) {
      this.ocpion = 1;
    }
    else {
      this.ocpion = 2;
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
    this.service.GetContratoByContratoId(id).subscribe(data => {
      this.contrato = data;
      this.cargarDataParaInsercion(data);
    });
    this.idContrato = id;
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
    this.fechaDRP = data.contratacion.disponibilidadPresupuestal[0].fechaDrp;
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

    this.numIdentificacionRepLegalInterventoria = data.contratacion.contratista.representanteLegalNumeroIdentificacion;
    this.nomRepresentanteLegalContrInterventoria = data.contratacion.contratista.representanteLegal;

    if (this.ocpion == 2) {
      this.dataSupervisor = true;
      this.numIdentifiacionSupervisor = data.usuarioInterventoria.numeroIdentificacion;
      this.nomSupervisor = data.usuarioInterventoria.nombres + " " + data.usuarioInterventoria.apellidos;
    }
  }
  generarFechaRestante() {
    let newdate = new Date(this.addressForm.value.fechaActaInicioFUnoPreconstruccion);
    newdate.setDate(newdate.getDate() + (this.mesPlazoIni * 30.44));
    // console.log(newdate);
    let newDateFinal = new Date(newdate);
    newDateFinal.setDate(newDateFinal.getDate() + this.diasPlazoIni)
    // console.log(newDateFinal);
    this.addressForm.get('fechaPrevistaTerminacion').setValue(newDateFinal);

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
  crearFormulario() {
    return this.fb.group({
      fechaActaInicioFUnoPreconstruccion: [null, Validators.required],
      fechaPrevistaTerminacion: [null, Validators.required],
      mesPlazoEjFase1: [null, Validators.required],
      diasPlazoEjFase1: [null, Validators.required],
      mesPlazoEjFase2: [ { value: null, disabled: true } , Validators.required],
      diasPlazoEjFase2: [ { value: null, disabled: true } , Validators.required],
      observacionesEspeciales: [ null ]
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
    let mesPlazoFase2;
    let diasPlazoFase2;
    let esSupervisionBool;
    if (this.ocpion==1){
      esSupervisionBool=true;
    }
    else{
      esSupervisionBool=false;
    }
    if (this.valorFDos == 0) {
      mesPlazoFase2 = 0;
      diasPlazoFase2 = 0;
      if ( this.addressForm.get( 'fechaActaInicioFUnoPreconstruccion' ).value === null || this.addressForm.get( 'fechaPrevistaTerminacion' ).value === null || this.addressForm.get( 'mesPlazoEjFase1' ).value === null
        || this.addressForm.get( 'diasPlazoEjFase1' ).value === null || this.addressForm.get( 'observacionesEspeciales' ).value === null ) {
          this.openDialog2('', '<b>Falta registrar información</b>');
          this.esRojo = true;
      }
      else {
        if ((this.addressForm.value.mesPlazoEjFase1 > this.mesPlazoIni) || (this.addressForm.value.mesPlazoEjFase1 < this.mesPlazoIni)) {
          this.openDialog('', 'Debe verificar la información ingresada en el campo <b>Plazo de ejecución - fase 1 - Preconstrucción Meses</b>, dado que no coincide con la información inicial registrada para el contrato');
        }
        else if ((this.addressForm.value.diasPlazoEjFase1 > this.diasPlazoIni) || (this.addressForm.value.diasPlazoEjFase1 < this.diasPlazoIni)) {
          this.openDialog('', 'Debe verificar la información ingresada en el campo <b>Plazo de ejecución - fase 1 - Preconstrucción Días</b>, dado que no coincide con la información inicial registrada para el contrato');
        }
        else {
          const arrayObservacion=[{
            'ContratoId':this.idContrato,
            "observaciones":this.addressForm.value.observacionesEspeciales,
            'esActa':false,
            'esActaFase1':true,
            'esSupervision':esSupervisionBool
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
            fechaActaInicioFase1: this.addressForm.value.fechaActaInicioFUnoPreconstruccion,
            fechaTerminacion: this.addressForm.value.fechaPrevistaTerminacion,
            plazoFase1PreMeses: this.addressForm.value.mesPlazoEjFase1,
            plazoFase1PreDias: this.addressForm.value.diasPlazoEjFase1,
            plazoFase2ConstruccionMeses: mesPlazoFase2,
            plazoFase2ConstruccionDias: diasPlazoFase2,
            observaciones: this.addressForm.value.observacionesEspeciales,
            conObervacionesActa: true,
            registroCompleto: true,
            contratoConstruccion: [],
            contratoObservacion: arrayObservacion,
            contratoPerfil: [],
            contratoPoliza: []
          };
          this.service.EditContrato(arrayContrato).subscribe((data: any) => {
            if (data.code == "200") {
              if (localStorage.getItem("origin") == "obra") {
                this.service.CambiarEstadoActa(this.idContrato, "14").subscribe(data0 => {
                  this.realizoPeticion = true;
                  this.openDialog2('', '<b>La información ha sido guardada exitosamente.</b>');
                  this.router.navigate(['/generarActaInicioFaseIPreconstruccion']);
                });
              }
              else {
                this.service.CambiarEstadoActa(this.idContrato, "2").subscribe(data0 => {
                  this.realizoPeticion = true;
                  this.openDialog2('', '<b>La información ha sido guardada exitosamente.</b>');
                  this.router.navigate(['/generarActaInicioFaseIPreconstruccion']);
                });
              }
            }
          });
        }
      }
    }
    else {
      mesPlazoFase2 = parseInt(this.addressForm.value.mesPlazoEjFase2);
      diasPlazoFase2 = parseInt(this.addressForm.value.diasPlazoEjFase2);
      var sumaMeses;
      var sumaDias;
      sumaMeses = parseInt(this.addressForm.value.mesPlazoEjFase1) + mesPlazoFase2;
      sumaDias = parseInt(this.addressForm.value.diasPlazoEjFase1) + diasPlazoFase2;
      if (  this.addressForm.get( 'fechaActaInicioFUnoPreconstruccion' ).value === null || this.addressForm.get( 'fechaPrevistaTerminacion' ).value === null || this.addressForm.get( 'mesPlazoEjFase1' ).value === null
            || this.addressForm.get( 'diasPlazoEjFase1' ).value === null || this.addressForm.get( 'observacionesEspeciales' ).value === null ) {
        this.openDialog2('', '<b>Falta registrar información</b>');
        this.esRojo = true;
      }
      else {
        if ((sumaMeses > this.mesPlazoIni) && this.valorFDos != 0) {
          this.openDialog('', 'Debe verificar la información ingresada en el campo <b>Plazo de ejecución - fase 1 - Preconstrucción Meses</b>, dado que no coincide con la informacion inicial registrada para el contrato');
        }
        else if ((sumaDias > this.diasPlazoIni) && this.valorFDos != 0) {
          this.openDialog('', 'Debe verificar la información ingresada en el campo <b>Plazo de ejecución - fase 2 - Construcción Días</b>, dado que no coincide con la informacion inicial registrada para el contrato');
        }
        else if ((this.valorFUno + this.valorFDos) != this.valorIni) {
          this.openDialog('', 'Debe verificar la información ingresada en el campo <b>Valor inicial del contrato</b>, dado que no coincide con la información inicial registrada para el contrato');
        }
        else {
          const arrayObservacion2=[{
            'ContratoId':this.idContrato,
            "observaciones":this.addressForm.value.observacionesEspeciales,
            'esActa':false,
            'esActaFase1':true,
            'esSupervision':esSupervisionBool
          }];
          const arrayContrato2: EditContrato = {
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
            fechaActaInicioFase1: this.addressForm.value.fechaActaInicioFUnoPreconstruccion,
            fechaTerminacion: this.addressForm.value.fechaPrevistaTerminacion,
            plazoFase1PreMeses: this.addressForm.value.mesPlazoEjFase1,
            plazoFase1PreDias: this.addressForm.value.diasPlazoEjFase1,
            plazoFase2ConstruccionMeses: this.addressForm.value.mesPlazoEjFase2,
            plazoFase2ConstruccionDias: this.addressForm.value.diasPlazoEjFase2,
            observaciones: this.addressForm.value.observacionesEspeciales,
            conObervacionesActa: true,
            registroCompleto: true,
            contratoConstruccion: [],
            contratoObservacion: arrayObservacion2,
            contratoPerfil: [],
            contratoPoliza: []
          };
          this.service.EditContrato(arrayContrato2).subscribe((data: any) => {
            if (data.code == "200") {
              if (localStorage.getItem("origin") == "obra") {
                this.service.CambiarEstadoActa(this.idContrato, "14").subscribe(data0 => {
                  this.realizoPeticion = true;
                  this.openDialog2('', '<b>La información ha sido guardada exitosamente.</b>');
                  this.router.navigate(['/generarActaInicioFaseIPreconstruccion']);
                });
              }
              else {
                this.service.CambiarEstadoActa(this.idContrato, "2").subscribe(data0 => {
                  this.realizoPeticion = true;
                  this.openDialog2('', '<b>La información ha sido guardada exitosamente.</b>');
                  this.router.navigate(['/generarActaInicioFaseIPreconstruccion']);
                });
              }
            }
          });
        }
      }
    }
  }
}

