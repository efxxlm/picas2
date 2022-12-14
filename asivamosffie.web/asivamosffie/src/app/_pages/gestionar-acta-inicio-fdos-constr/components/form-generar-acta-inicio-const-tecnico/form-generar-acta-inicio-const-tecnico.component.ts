import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { parse } from 'path';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DatePipe } from '@angular/common';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';
import { ActaInicioConstruccionService } from 'src/app/core/_services/actaInicioConstruccion/acta-inicio-construccion.service';

@Component({
  selector: 'app-form-generar-acta-inicio-const-tecnico',
  templateUrl: './form-generar-acta-inicio-const-tecnico.component.html',
  styleUrls: ['./form-generar-acta-inicio-const-tecnico.component.scss']
})
export class FormGenerarActaInicioConstTecnicoComponent implements OnInit, OnDestroy {

  maxDate: Date;
  maxDate2: Date;
  public idContrato;
  public contratacionId;
  public fechaTramite;
  public tipoContratoCodigo;
  public fechaEnvioFirma;
  public estadoDocumentoCodigo;
  public fechaFirmaContratista;
  public fechaFirmaFiduciaria;
  public mesPlazoIni: number = 10;
  public diasPlazoIni: number = 25;
  public observacionesOn: boolean;
  public editable: boolean;
  public title;

  public contratoCode;
  public fechaAprobacionSupervisor;
  public cantidadProyectosAsociados;
  public departamentoYMunicipioLlaveMEN;
  public fechaAprobacionGarantiaPoliza;
  public fechaFirmaContrato;
  public fechaGeneracionDRP1;
  public fechaGeneracionDRP2;
  public institucionEducativaLlaveMEN;
  public llaveMENContrato;
  public nombreEntidadContratistaObra;
  public nombreEntidadContratistaSupervisorInterventoria;
  public numeroContrato;
  public numeroDRP1;
  public numeroDRP2;
  public observacionOConsideracionesEspeciales;
  public plazoInicialContratoSupervisor;
  public valorActualContrato;
  public valorFase1Preconstruccion;
  public valorInicialContrato;
  public valorfase2ConstruccionObra;
  public vigenciaContrato;
  public plazoActualContratoMeses;
  public plazoActualContratoDias;
  public plazoEjecucionPreConstruccionMeses;
  public plazoEjecucionPreConstruccionDias;
  nombreRepresentanteLegalInterventoria : any;
  numeroIdentificacionEntidadContratistaObra: any;
  public contrato;

  fechaSesionString: string;
  fechaSesion: Date;

  fechaSesionString2: string;
  fechaSesion2: Date;

  addressForm = this.fb.group({});
  dataDialog: {
    modalTitle: string,
    modalText: string
  };
  esActaFase2: boolean;
  fechaCreacion: Date;
  observacionesActaFase2: string;
  conObervacionesActa: boolean;
  objeto: any;
  numeroIdentificacionRepresentanteContratistaInterventoria: any;
  numeroIdentificacionSupervisor: any;
  valorProponente: any;

  realizoPeticion: boolean = false;
  esRojo: boolean = false;
  estaEditando = false;

  constructor(private router: Router, private activatedRoute: ActivatedRoute, public dialog: MatDialog, private fb: FormBuilder, public datepipe: DatePipe, private services: ActaInicioConstruccionService, private service: GestionarActPreConstrFUnoService) {
    this.maxDate = new Date();
    this.maxDate2 = new Date();
  }
  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
    });
    if (localStorage.getItem("editable") == "true") {
      this.editable = true;
      this.title = 'Ver detalle/Editar';
    }
    else {
      this.editable = false;
      this.title = 'Generar';
    }
  }
  ngOnDestroy(): void {
    if (this.addressForm.dirty === true && this.realizoPeticion === false) {
      this.openDialogConfirmar('', '??Desea guardar la informaci??n registrada?');
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
  crearFormulario() {
    return this.fb.group({
      fechaActaInicioFDosConstruccion: [Date(), Validators.required],
      fechaPrevistaTerminacion: [Date(), Validators.required],
      mesPlazoEjFase2: [null, Validators.required],
      diasPlazoEjFase2: [null, Validators.required],
      observacionesEspeciales: [""]
    })
  }
  loadData(id) {
    this.services.GetVistaGenerarActaInicio(id).subscribe((data: any) => {
      /*Titulo*/
      this.contratoCode = data.numeroContrato;
      this.fechaAprobacionSupervisor = data.fechaAprobacionRequisitosSupervisorDate;
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
      //this.valorInicialContrato = data.valorInicialContrato;
      this.valorInicialContrato = data.valorActualContrato;
      this.valorActualContrato = data.valorActualContrato;
      this.valorFase1Preconstruccion = data.valorFase1Preconstruccion;
      this.valorfase2ConstruccionObra = data.valorfase2ConstruccionObra;
      this.nombreEntidadContratistaSupervisorInterventoria = data.nombreEntidadContratistaSupervisorInterventoria;
      this.nombreEntidadContratistaObra = data.nombreEntidadContratistaObra;
      this.valorProponente = data.proponenteCodigo;
      /*Campo de texto no editable*/
      this.plazoActualContratoMeses = data.plazoActualContratoMeses;
      this.plazoActualContratoDias = data.plazoActualContratoDias;
      this.plazoEjecucionPreConstruccionMeses = data.plazoFase1PreMeses;
      this.plazoEjecucionPreConstruccionDias = data.plazoFase1PreDias;
      this.nombreRepresentanteLegalInterventoria = data?.nombreRepresentanteLegalInterventoria;
      this.numeroIdentificacionEntidadContratistaObra = data?.numeroIdentificacionEntidadContratistaObra;
      this.contrato = data.contrato;
      this.loadDataObservaciones(data.contrato.contratoConstruccion[0].contratoConstruccionId);

      const mesesPlazoInicial = data.contrato.contratacion.plazoContratacion.plazoMeses;
      const diasPlazoInicial = data.contrato.contratacion.plazoContratacion.plazoDias;

      // calcula el plazo restante
      if ( data.plazoFase1PreMeses !== undefined && data.plazoFase1PreDias !== undefined ) {

        this.service.getFiferenciaMesesDias( mesesPlazoInicial, diasPlazoInicial, data.plazoFase1PreMeses, data.plazoFase1PreDias )
          .subscribe(
            response => {
              console.log(response)
              this.addressForm.get( 'mesPlazoEjFase2' ).setValue( response[0] );
              this.addressForm.get('diasPlazoEjFase2').setValue( response[1] );
            }
          );
      }
      else{
        this.addressForm.get('mesPlazoEjFase2').setValue(mesesPlazoInicial);
        this.addressForm.get('diasPlazoEjFase2').setValue(diasPlazoInicial);
      }

      // this.addressForm.get('mesPlazoEjFase2').disable();
      // this.addressForm.get('diasPlazoEjFase2').disable();

      /*Campo de texto editable*/
      if (this.editable == true) {
        this.addressForm.get('fechaActaInicioFDosConstruccion').setValue(data.fechaActaInicioFase2DateTime);
        this.addressForm.get('fechaPrevistaTerminacion').setValue(data.fechaPrevistaTerminacionDateTime);
        this.addressForm.get('observacionesEspeciales').setValue(data.observacionOConsideracionesEspeciales);
        //if ( data.plazoFase1PreMeses !== undefined && data.plazoFase1PreDias !== undefined ) {
          // const mesesPlazoInicial = data.contrato.contratacion.disponibilidadPresupuestal[0].plazoMeses;
          // const diasPlazoInicial = data.contrato.contratacion.disponibilidadPresupuestal[0].plazoDias;
          // this.service.getFiferenciaMesesDias( mesesPlazoInicial, diasPlazoInicial, data.plazoFase1PreMeses, data.plazoFase1PreDias )
          //   .subscribe(
          //     response => {
          //       console.log(response)
          //       this.addressForm.get( 'mesPlazoEjFase2' ).setValue( response[0] );
          //       this.addressForm.get('diasPlazoEjFase2').setValue( response[1] );
          //     }
          //   );
        //     this.addressForm.get('mesPlazoEjFase2').disable();
        //     this.addressForm.get('diasPlazoEjFase2').disable();
        // } else {
        //   this.addressForm.get('mesPlazoEjFase2').setValue(data.plazoFase2ConstruccionMeses);
        //   this.addressForm.get('diasPlazoEjFase2').setValue(data.plazoFase2ConstruccionDias);
        // }
      }
    });
    this.idContrato = id;
  }
  loadDataObservaciones(id) {
    /*
    this.services.GetContratoObservacionByIdContratoId(id, true).subscribe(data0 => {
      this.conObervacionesActa = data0.esActa;
      this.observacionesActaFase2 = data0.observaciones;
      this.fechaCreacion = data0.fechaCreacion;
    });
    */
    this.services.GetConstruccionObservacionByIdContratoConstruccionId(id,true).subscribe((data1:any)=>{
      if ( data1 !== null ) {
        this.conObervacionesActa = data1.esActa;
        this.observacionesActaFase2 = data1.observaciones;
        this.fechaCreacion = data1.fechaCreacion;
      }
    });
  }
  generarFechaRestante() {
    let newdate = new Date(this.addressForm.value.fechaActaInicioFDosConstruccion);
    let meses = this.addressForm.get('mesPlazoEjFase2').value > 0 ? this.addressForm.get('mesPlazoEjFase2').value :0;
    let dias = this.addressForm.get('diasPlazoEjFase2').value > 0 ? this.addressForm.get('diasPlazoEjFase2').value  :0;
    newdate.setDate(newdate.getDate() + (meses * 30) + dias);
    let newDateFinal = new Date(newdate);
    //newDateFinal.setDate(newDateFinal.getDate() + this.plazoActualContratoDias) //se estaba sumando dos veces el plazo ?? LCT -JCM (02/05/2022)
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
    height: '45px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

    if (texto) {
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
  removeTags(str) {
    if ((str === null) || (str === '')) {
      return '';
    }
    else {
      str = str.toString();
      return str.replace(/(<([^>]+)>)/ig, '');
    }
  }
  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    let meses: number = this.plazoEjecucionPreConstruccionMeses === undefined ? 0 : this.plazoEjecucionPreConstruccionMeses;
    meses = meses + (this.addressForm.get('mesPlazoEjFase2').value === undefined ? 0 : this.addressForm.get('mesPlazoEjFase2').value);

    let dias: number = this.plazoEjecucionPreConstruccionDias === undefined ? 0 : this.plazoEjecucionPreConstruccionDias;
    dias = dias + (this.addressForm.get('diasPlazoEjFase2').value === undefined ? 0 : this.addressForm.get('diasPlazoEjFase2').value);

    let plazoDias: number = this.plazoActualContratoDias;
    let plazoMeses: number = this.plazoActualContratoMeses;

    console.log(this.plazoEjecucionPreConstruccionMeses, this.addressForm.get('mesPlazoEjFase2').value, this.plazoEjecucionPreConstruccionDias, this.addressForm.get('diasPlazoEjFase2').value)
    console.log(plazoDias, dias, plazoMeses, meses )

    // if (plazoDias != dias || plazoMeses != meses) {
    //   this.openDialog( '', 'Debe verificar la informaci??n ingresada en el campo <b>Plazo de ejecuci??n fase 2 ??? Construcci??n</b>, dado que no coincide con la informaci??n inicial registrada para el contrato ' )
    //   return false;
    // }

    if (this.addressForm.valid) {
      let fecha = Date.parse(this.addressForm.get('fechaActaInicioFDosConstruccion').value);
      this.fechaSesion = new Date(fecha);
      this.fechaSesionString = `${this.fechaSesion.getFullYear()}/${this.fechaSesion.getMonth() + 1}/${this.fechaSesion.getDate()}`;

      let fecha2 = Date.parse(this.addressForm.get('fechaPrevistaTerminacion').value);
      this.fechaSesion2 = new Date(fecha2);
      this.fechaSesionString2 = `${this.fechaSesion2.getFullYear()}/${this.fechaSesion2.getMonth() + 1}/${this.fechaSesion2.getDate()}`;
      console.log(this.addressForm.value)
      //compara los meses
      if (
          this.addressForm.value.fechaActaInicioFDosConstruccion == null ||
          this.addressForm.value.fechaPrevistaTerminacion == null ||
          this.addressForm.value.mesPlazoEjFase2 == null ||
          this.addressForm.value.diasPlazoEjFase2 == null
          )
          {
            this.openDialog2('', '<b>Falta registrar informaci??n</b>');
            this.esRojo = true;
      }
      else {
        if (localStorage.getItem("editable") == "false") {
          var sumaMeses;
          var sumaDias;
          sumaMeses = this.plazoEjecucionPreConstruccionMeses + parseInt(this.addressForm.value.mesPlazoEjFase2);
          sumaDias = this.plazoEjecucionPreConstruccionDias + parseInt(this.addressForm.value.diasPlazoEjFase2);
          // console.log(sumaDias);
          // if (sumaMeses > this.plazoActualContratoMeses) {
          //   this.openDialog("", 'Debe verificar la informaci??n ingresada en el campo <b>Meses</b>, dado que no coincide con la informaci??n inicial registrada para el contrato');
          // }
          // else if (sumaDias > this.plazoActualContratoDias) {
          //   this.openDialog("", 'Debe verificar la informaci??n ingresada en el campo <b>D??as</b>, dado que no coincide con la informaci??n inicial registrada para el contrato');
          // }
          //else {
            this.services.CreatePlazoEjecucionFase2Construccion(this.idContrato, this.addressForm.value.mesPlazoEjFase2, this.addressForm.value.diasPlazoEjFase2, this.addressForm.value.observacionesEspeciales, "usr2", this.fechaSesionString, this.fechaSesionString2, false, true).subscribe(data1 => {
              if (data1.code == "200") {
                if (localStorage.getItem("origin") == "interventoria") {
                  this.services.CambiarEstadoActa(this.idContrato, "2", "usr2").subscribe(resp => {
                    this.realizoPeticion = true;
                    this.openDialog2("", '<b>La informaci??n ha sido guardada exitosamente.</b>');
                    this.router.navigate(['/generarActaInicioConstruccion']);
                  });
                }
                else {
                  this.services.CambiarEstadoActa(this.idContrato, "14", "usr2").subscribe(resp => {
                    this.realizoPeticion = true;
                    this.openDialog2("", '<b>La informaci??n ha sido guardada exitosamente.</b>');
                    this.router.navigate(['/generarActaInicioConstruccion']);
                  });
                }

              }
              else {
                this.openDialog2('', `<b>${data1.message}</b>`);
              }
            });

          //}
        }
        else {
          this.services.EditarContratoObservacion(this.idContrato, this.addressForm.value.mesPlazoEjFase2, this.addressForm.value.diasPlazoEjFase2, this.addressForm.value.observacionesEspeciales, "usr2", this.fechaSesionString, this.fechaSesionString2, false, true).subscribe(resp => {
            if (resp.code == "200") {
              this.realizoPeticion = true;
              this.openDialog2("", '<b>La informaci??n ha sido guardada exitosamente.</b>');
              this.router.navigate(['/generarActaInicioConstruccion']);
            }
            else {
              this.openDialog("", `<b>${resp.message}</b>`);
            }
          })
        }
      }

      if (localStorage.getItem("origin") == "interventoria") {
        if (this.addressForm.value.observacionesEspeciales == null) {
          localStorage.setItem("observacionesEspecialesInterventoria", "No");
        }
        else {
          localStorage.setItem("observacionesEspecialesInterventoria", "Si");
        }
      }
      // console.log(this.addressForm.value);

    } else {
      this.openDialog2('', '<b>El formulario est?? incompleto</b>');
    }
  }



}
