import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CreatePolizaGarantia, CreatePolizaObservacion, EditPoliza, InsertPoliza, PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { EstadoPolizaCodigo } from 'src/app/_interfaces/gestionar-polizas-garantias.interface';
import { EstadosRevision, PerfilCodigo } from 'src/app/_interfaces/estados-actualizacion-polizas.interface';
import humanize from 'humanize-plus';
@Component({
  selector: 'app-editar-en-revision',
  templateUrl: './editar-en-revision.component.html',
  styleUrls: ['./editar-en-revision.component.scss']
})
export class EditarEnRevisionComponent implements OnInit, OnDestroy {

    addressForm = this.fb.group({
        nombre: [ null, Validators.compose([ Validators.required, Validators.minLength(1), Validators.maxLength(50)]) ],
        numeroPoliza: [ null, Validators.compose([ Validators.required, Validators.minLength(2), Validators.maxLength(20) ]) ],
        numeroCertificado: [ null, Validators.compose([ Validators.required, Validators.minLength(2), Validators.maxLength(20)]) ],
        fecha: [ null, Validators.required ],
        seguros: this.fb.array( [] ),
        polizasYSeguros: [ null, Validators.required ],
        cumpleAsegurado: [ null, Validators.required ],
        cumpleBeneficiario: [ null, Validators.required],
        cumpleAfianzado: [ null, Validators.required],
        reciboDePago: [ null, Validators.required],
        condicionesGenerales: [ null, Validators.required],
        fechaRevision: [ null, Validators.required],
        estadoRevision: [ null, Validators.required],
        fechaAprob: [ null, Validators.required],
        responsableAprob: [ null, Validators.required ],
        observacionesGenerales: [ null ]
    });
    contrato: any;
    estadosRevision = EstadosRevision;
    listaUsuarios: any[] = [];
    realizoPeticion: boolean = false;
    estaEditando = false;
    listaPerfilCodigo = PerfilCodigo;
    polizasYSegurosArray: Dominio[] = [];
    listaTipoSolicitudContrato: Dominio[] = [];
    listaTipoDocumento: Dominio[] = [];
    estadosPoliza = EstadoPolizaCodigo;
    estadoArray = [];
    minDate: Date;
    editorStyle = {
      height: '100px'
    };
    config = {
      toolbar: [
        ['bold', 'italic', 'underline'],
        [{ list: 'ordered' }, { list: 'bullet' }],
        [{ indent: '-1' }, { indent: '+1' }],
        [{ align: [] }],
      ]
    };

    get seguros() {
        return this.addressForm.get( 'seguros' ) as FormArray;
    }

    constructor(
        private routes: Router,
        private polizaSvc: PolizaGarantiaService,
        private fb: FormBuilder,
        private dialog: MatDialog,
        private activatedRoute: ActivatedRoute,
        private commonSvc: CommonService )
    {
        this.minDate = new Date();
        this.minDate = new Date();
        this.commonSvc.getUsuariosByPerfil( this.listaPerfilCodigo.fiduciaria )
            .subscribe( getUsuariosByPerfil => this.listaUsuarios = getUsuariosByPerfil );
        this.commonSvc.listaGarantiasPolizas()
            .subscribe( listaGarantiasPolizas => this.polizasYSegurosArray = listaGarantiasPolizas );
        this.commonSvc.listaTipodocumento()
            .subscribe( listaTipodocumento => this.listaTipoDocumento = listaTipodocumento )
        this.commonSvc.listaEstadoRevision()
            .subscribe( estadoRevision => this.estadoArray = estadoRevision );
        this.commonSvc.listaTipoSolicitudContrato()
            .subscribe( response => this.listaTipoSolicitudContrato = response );
        this.polizaSvc.getContratoByContratoId( this.activatedRoute.snapshot.params.id )
            .subscribe(
                response => {
                    this.contrato = response;
                    console.log( this.contrato )
                }
            )
    }

    ngOnInit(): void {
    }

    ngOnDestroy(): void {
        if (this.addressForm.dirty === true && this.realizoPeticion === false) {
            this.openDialogConfirmar('', '¿Desea guardar la información registrada?');
        }
    }

    getTipoSolicitud( codigo: string ) {
        if ( this.listaTipoSolicitudContrato.length > 0 ) {
            const tipo = this.listaTipoSolicitudContrato.find( tipo => tipo.codigo === codigo );

            if ( tipo !== undefined ) {
                return tipo.nombre;
            }
        }
    }

    getTipoDocumento( codigo: string ) {
        if ( this.listaTipoDocumento.length > 0 ) {
            const documento = this.listaTipoDocumento.find( documento => documento.codigo === codigo );

            if ( documento !== undefined ) {
                return documento.nombre;
            }
        }
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
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
    }

    async getvalues( listCodigo: string[] ) {
        const listaSeguros = [ ...listCodigo ];

        if ( this.seguros.length > 0 ) {
            this.seguros.controls.forEach( ( control, indexControl ) => {
                const seguroIndex = listaSeguros.findIndex( codigo => codigo === control.get( 'tipoGarantiaCodigo' ).value );
                const seguro = listaSeguros.find( codigo => codigo === control.get( 'tipoGarantiaCodigo' ).value );

                if ( seguroIndex !== -1 ) {
                    listaSeguros.splice( seguroIndex, 1 );
                }

                if ( seguro === undefined ) {
                    this.seguros.removeAt( indexControl );
                    listaSeguros.splice( seguroIndex, 1 );
                }
            } );
        }

        for ( const codigo of listaSeguros ) {
            const seguro = this.polizasYSegurosArray.find( seguro => seguro.codigo === codigo );

            if ( seguro !== undefined ) {
                this.seguros.push( this.fb.group(
                    {
                        contratoPolizaId: [ 0 ],
                        polizaGarantiaId: [ 0 ],
                        nombre: [ seguro.nombre ],
                        tipoGarantiaCodigo: [ seguro.codigo ],
                        esIncluidaPoliza: [ null, Validators.required ],
                        vigencia: [ null, Validators.required ],
                        vigenciaAmparo: [ null, Validators.required ],
                        valorAmparo: [ null, Validators.required ]
                    }
                ) )
            }
        }
    }

    checkSeguros() {
        let registroCompletoSeguros = false;

        if ( this.seguros.length > 0 ) {
            let registroCompleto = 0;
            this.seguros.controls.forEach( control => {
                if ( control.get( 'esIncluidaPoliza' ).value !== null ) {
                    if ( control.get( 'esIncluidaPoliza' ).value === true ) {
                        if (control.get( 'vigencia' ).value !== null && control.get( 'vigenciaAmparo' ).value !== null && control.get( 'valorAmparo' ).value !== null ) {
                            registroCompleto++;
                        }
                    }

                    if ( control.get( 'esIncluidaPoliza' ).value === false ) {
                        registroCompleto++;
                    }
                }
            } );
            
            if ( registroCompleto === this.seguros.length ) {
                registroCompletoSeguros = true;
            }
        }

        if ( registroCompletoSeguros === false ) {
            this.addressForm.get( 'cumpleAsegurado' ).setValue( null );
            this.addressForm.get( 'cumpleBeneficiario' ).setValue( null );
            this.addressForm.get( 'cumpleAfianzado' ).setValue( null );
            this.addressForm.get( 'reciboDePago' ).setValue( null );
            this.addressForm.get( 'condicionesGenerales' ).setValue( null );
        }

        return registroCompletoSeguros;
    }

    checkEsIncluidaPoliza( value: boolean, index: number ) {
        if ( value === false ) {
            this.seguros.controls[ index ].get( 'vigencia' ).setValue( null );
            this.seguros.controls[ index ].get( 'vigenciaAmparo' ).setValue( null );
            this.seguros.controls[ index ].get( 'valorAmparo' ).setValue( null );
        }
    }

    // evalua tecla a tecla
    validateNumberKeypress(event: KeyboardEvent) {
      const alphanumeric = /[0-9]/;
      const inputChar = String.fromCharCode(event.charCode);
      return alphanumeric.test(inputChar) ? true : false;
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

    openDialog(modalTitle: string, modalText: string) {
      this.dialog.open(ModalDialogComponent, {
        width: '28em',
        data: { modalTitle, modalText }
      });
    }

    onSubmit() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        const getPolizaGarantia = () => {
            const listSeguros = [];

            if ( this.seguros.length > 0 ) {
                this.seguros.controls.forEach( control => {
                    listSeguros.push(
                        {
                            contratoPolizaId: control.get( 'contratoPolizaId' ).value,
                            polizaGarantiaId: control.get( 'polizaGarantiaId' ).value,
                            tipoGarantiaCodigo: control.get( 'tipoGarantiaCodigo' ).value,
                            esIncluidaPoliza: control.get( 'esIncluidaPoliza' ).value,
                            vigencia: control.get( 'vigencia' ).value,
                            vigenciaAmparo: control.get( 'vigenciaAmparo' ).value,
                            valorAmparo: control.get( 'valorAmparo' ).value
                        }
                    )
                } )
            }

            return listSeguros.length > 0 ? listSeguros : null;
        }
        const getHistorialObservaciones = () => {
            return [
                {
                    polizaObservacionId: 0, 
                    contratoPolizaId: 0,
                    observacion: this.addressForm.get( 'observacionesGenerales' ).value !== null ? this.addressForm.get( 'observacionesGenerales' ).value : '',
                    fechaRevision: this.addressForm.get( 'fechaRevision' ).value !== null ? new Date( this.addressForm.get( 'fechaRevision' ).value ).toISOString() : new Date().toISOString(),
                    estadoRevisionCodigo: this.addressForm.get( 'estadoRevision' ).value !== null ? this.addressForm.get( 'estadoRevision' ).value : this.estadosPoliza.enRevision
                }
            ]
        }

        const pContrato = {
            contratoId: Number( this.activatedRoute.snapshot.params.id ),
            contratoPoliza: [
                {
                    contratoId: Number( this.activatedRoute.snapshot.params.id ),
                    contratoPolizaId: 0,
                    nombreAseguradora: this.addressForm.get( 'nombre' ).value,
                    numeroPoliza: this.addressForm.get( 'numeroPoliza' ).value,
                    numeroCertificado: this.addressForm.get( 'numeroCertificado' ).value,
                    observacionesRevisionGeneral: this.addressForm.get( 'observacionesGenerales' ).value,
                    estadoPolizaCodigo: this.addressForm.get( 'estadoRevision' ).value !== null ? ( this.addressForm.get( 'estadoRevision' ).value === this.estadosPoliza.sinRadicacion ? this.estadosPoliza.polizaDevuelta : this.estadosPoliza.enRevision ) : this.estadosPoliza.enRevision,
                    fechaExpedicion: this.addressForm.get( 'fecha' ).value,
                    cumpleDatosAsegurado: this.addressForm.get( 'cumpleAsegurado' ).value,
                    cumpleDatosBeneficiario: this.addressForm.get( 'cumpleBeneficiario' ).value,
                    cumpleDatosTomador: this.addressForm.get( 'cumpleAfianzado' ).value,
                    incluyeReciboPago: this.addressForm.get( 'reciboDePago' ).value,
                    incluyeCondicionesGenerales: this.addressForm.get( 'condicionesGenerales' ).value,
                    fechaAprobacion: this.addressForm.get( 'fechaAprob' ).value,
                    polizaGarantia: getPolizaGarantia(),
                    historialObservaciones: getHistorialObservaciones()
                }
            ]
        }
    
        /*
        if (this.addressForm.value.polizasYSeguros != undefined || this.addressForm.value.polizasYSeguros != null) {
          polizasList = [this.addressForm.value.polizasYSeguros[0].codigo];
          for (let i = 1; i < this.addressForm.value.polizasYSeguros.length; i++) {
            const membAux = polizasList.push(this.addressForm.value.polizasYSeguros[i].codigo);
          }
        }
        let nombreAprobado;
        if (this.addressForm.value.responsableAprob != undefined || this.addressForm.value.responsableAprob != null) {
          if (!this.addressForm.value.responsableAprob.usuarioId) {
            nombreAprobado = "pendiente";
          }
          else {
            nombreAprobado = this.addressForm.value.responsableAprob.usuarioId;
          }
        }
        var statePoliza = this.estadosPoliza.enRevision;
        let revEstado;
        if (this.addressForm.value.estadoRevision != undefined || this.addressForm.value.estadoRevision != null) {
          revEstado = this.addressForm.value.estadoRevision.codigo;
          if (revEstado == this.estadosPoliza.sinRadicacion ) {
            statePoliza = this.estadosPoliza.polizaDevuelta;
          }
          else {
            statePoliza = this.estadosPoliza.enRevision;
          }
        }
        var completo: boolean;
        if (this.addressForm.valid) {
          completo = true;
        }
        else {
          completo = false;
        }
    
        const contratoArray = {
          'contratoId': this.idContrato,
          "contratoPolizaId": this.idPoliza,
          'TipoSolicitudCodigo': "",
          'TipoModificacionCodigo': "",
          'DescripcionModificacion': "",
          'NombreAseguradora': this.addressForm.value.nombre,
          'NumeroPoliza': this.addressForm.value.numeroPoliza,
          'NumeroCertificado': this.addressForm.value.numeroCertificado,
          'Observaciones': "",
          'ObservacionesRevisionGeneral': this.addressForm.value.observacionesGenerales,
          'ResponsableAprobacion': nombreAprobado,
          'EstadoPolizaCodigo': statePoliza,
          //'UsuarioCreacion': "",
          //'UsuarioModificacion': "",
          'FechaExpedicion': this.addressForm.value.fecha,
          'Vigencia': this.addressForm.value.vigenciaPoliza,
          'VigenciaAmparo': this.addressForm.value.vigenciaAmparo,
          'ValorAmparo': this.addressForm.value.valorAmparo,
          'CumpleDatosAsegurado': this.addressForm.value.cumpleAsegurado,
          'CumpleDatosBeneficiario': this.addressForm.value.cumpleBeneficiario,
          'CumpleDatosTomador': this.addressForm.value.cumpleAfianzado,
          'IncluyeReciboPago': this.addressForm.value.reciboDePago,
          'IncluyeCondicionesGenerales': this.addressForm.value.condicionesGenerales,
          'FechaAprobacion': this.addressForm.value.fechaAprob,
          'Estado': false,
          //'FechaCreacion': "",
          'RegistroCompleto': completo,
          //'FechaModificacion': "",
          'Eliminado': false
        };
        const observacionArray = {
          polizaObservacionId: 0,
          contratoPolizaId: this.idPoliza,
          observacion: this.addressForm.get( 'observacionesGenerales' ).value !== null ? this.addressForm.get( 'observacionesGenerales' ).value : '',
          fechaRevision: this.addressForm.get( 'fechaRevision' ).value !== null ? new Date( this.addressForm.get( 'fechaRevision' ).value ).toISOString() : null,
          estadoRevisionCodigo: this.addressForm.get( 'estadoRevision' ).value !== null ? this.addressForm.get( 'estadoRevision' ).value.codigo : null
        }
        let garantiaArray;
        if (this.addressForm.value.polizasYSeguros != undefined || this.addressForm.value.polizasYSeguros != null) {
          for (let i = 0; i < polizasList.length; i++) {
            switch (polizasList[i]) {
              case '1':
                garantiaArray = {
                  'contratoPolizaId': this.idPoliza,
                  'TipoGarantiaCodigo': '1',
                  'EsIncluidaPoliza': this.addressForm.value.buenManejoCorrectaInversionAnticipo
                };
                this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r => {
                }, err => this.openDialog( '', `<b>${ err.message }</b>` ) );
                break;
              case '2':
                garantiaArray = {
                  'contratoPolizaId': this.idPoliza,
                  'TipoGarantiaCodigo': '2',
                  'EsIncluidaPoliza': this.addressForm.value.estabilidadYCalidad
                };
                this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r1 => {
                }, err => this.openDialog( '', `<b>${ err.message }</b>` ) );
                break;
              case '3':
                garantiaArray = {
                  'contratoPolizaId': this.idPoliza,
                  'TipoGarantiaCodigo': '3',
                  'EsIncluidaPoliza': this.addressForm.value.polizaYCoumplimiento
                };
                this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r2 => {
                }, err => this.openDialog( '', `<b>${ err.message }</b>` ) );
                break;
              case '4':
                garantiaArray = {
                  'contratoPolizaId': this.idPoliza,
                  'TipoGarantiaCodigo': '4',
                  'EsIncluidaPoliza': this.addressForm.value.polizasYSegurosCompleto
                };
                this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r3 => {
                }, err => this.openDialog( '', `<b>${ err.message }</b>` ) );
                break;
            }
          }
        }
        this.polizaService.EditarContratoPoliza(contratoArray).subscribe(data => {
          if (data.isSuccessful == true) {
            /*this.polizaService.CreatePolizaGarantia(polizaGarantia).subscribe(data0=>{
    
            });
            this.polizaService.createEditPolizaObservacion( observacionArray )
              .subscribe(
                () => this.realizoPeticion = true,
                err => this.openDialog('', `<b>${err.message}</b>`)
              );
            /*
            this.polizaService.CambiarEstadoPolizaByContratoId(statePoliza, this.idContrato).subscribe(resp1 => {
    
            });

            this.realizoPeticion = true;
            this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
            this.router.navigate(['/generarPolizasYGarantias']);
          }
          else {
            this.openDialog('', `<b>${data.message}</b>`);
          }
        }, err => this.openDialog( '', `<b>${ err.message }</b>` ) );
        */
    }

}
