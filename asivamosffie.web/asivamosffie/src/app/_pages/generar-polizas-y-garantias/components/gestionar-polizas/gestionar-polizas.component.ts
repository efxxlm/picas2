import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { PolizaGarantiaService, ContratoPoliza, InsertPoliza } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';
import { OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { EstadoPolizaCodigo } from 'src/app/_interfaces/gestionar-polizas-garantias.interface';
@Component({
  selector: 'app-gestionar-polizas',
  templateUrl: './gestionar-polizas.component.html',
  styleUrls: ['./gestionar-polizas.component.scss']
})
export class GestionarPolizasComponent implements OnInit, OnDestroy {

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
        responsableAprob: [ '', Validators.required ],
        observacionesGenerales: [ null ]
    });
    polizasYSegurosArray: Dominio[] = [];
    estadoArray: any[];
    estadosPoliza = EstadoPolizaCodigo;
    aprobadosArray = [
      { name: 'Andres Montealegre', value: '1' },
      { name: 'David Benitez', value: '2' }
    ];
    listaUsuarios: any[] = [];
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
    // variables que se van a a eliminar
    public tipoContrato;
    public objeto;
    public nombreContratista;
    public tipoIdentificacion;
    public numeroIdentificacion;
    public valorContrato;
    public plazoContrato;
    public numContrato;
    public idContrato;
    fechaFirmaContrato: any;
    tipoSolicitud: any;
    contratoPolizaId: any;
    realizoPeticion: boolean = false;
    estaEditando = false;

    get seguros() {
        return this.addressForm.get( 'seguros' ) as FormArray;
    }

    constructor(
        private router: Router,
        private polizaService: PolizaGarantiaService,
        private fb: FormBuilder,
        public dialog: MatDialog,
        private activatedRoute: ActivatedRoute,
        private common: CommonService,
        private contratacion: ProjectContractingService )
    {
        this.minDate = new Date();
        this.common.listaEstadoRevision()
            .subscribe(
                estadoRevision => {
                    console.log( estadoRevision );
                    this.estadoArray = estadoRevision;
                }
            );
    }

    ngOnInit(): void {
      this.activatedRoute.params.subscribe(param => {
        this.cargarDatos(param.id);
      });
    }

    ngOnDestroy(): void {
      if ( this.addressForm.dirty === true && this.realizoPeticion === false) {
        this.openDialogConfirmar( '', '<b>¿Desea guardar la información registrada?</b>' );
      }
    };

    openDialogConfirmar(modalTitle: string, modalText: string) {
      const confirmarDialog = this.dialog.open(ModalDialogComponent, {
        width: '30em',
        data: { modalTitle, modalText, siNoBoton: true }
      });

      confirmarDialog.afterClosed()
        .subscribe( response => {
          if ( response === true ) {
            this.onSubmit();
          }
        } );
    }

    cargarDatos(id) {
      if ( id !== undefined ) {
        this.polizaService.GetListVistaContratoGarantiaPoliza(id).subscribe(data => {
          console.log( data )
          this.fechaFirmaContrato = data[0].fechaFirmaContrato;
          this.tipoSolicitud = data[0].tipoSolicitud;
          this.numContrato = data[0].numeroContrato;
          this.tipoIdentificacion = data[0].tipoDocumento;
          if (data[0].objetoContrato != undefined || data[0].objetoContrato != null) {
            this.objeto = data[0].objetoContrato;
          }
          else {
            this.objeto = '';
          };
          this.tipoContrato = data[0].tipoContrato;
          if (data[0].valorContrato != undefined || data[0].valorContrato != null) {
            this.valorContrato = data[0].valorContrato;
          }
          else {
            this.valorContrato = 0;
          }
          this.plazoContrato = data[0].plazoContrato;
          this.loadContratacionId(data[0].contratacionId);
        });
      }
      /*
      this.polizaService.GetContratoPolizaByIdContratoId(id).subscribe(a => {
        this.loadPolizaId(a);
      });
      */
      this.common.listaGarantiasPolizas().subscribe(data0 => {
        this.polizasYSegurosArray = data0;
      });
      this.common.getUsuariosByPerfil(10).subscribe(resp => {
        this.listaUsuarios = resp;
      });
      this.idContrato = id;
    }

    loadPolizaId(a) {
      if (this.contratoPolizaId != null) {
        this.contratoPolizaId = a.contratoPolizaId;
      }
      else {
        this.contratoPolizaId = 0;
      }

    }

    loadContratacionId(a) {
      this.contratacion.getContratacionByContratacionId(a).subscribe(data => {
        this.loadInfoContratacion(data);
      });
    }

    loadInfoContratacion(data) {
      this.nombreContratista = data.contratista.nombre;
      this.numeroIdentificacion = data.contratista.numeroIdentificacion;

    }

    loadNoContratacionID() {
      this.tipoContrato = 'Pendiente';
      this.objeto = 'Pendiente';
      this.valorContrato = 0;
      this.tipoIdentificacion = 'Pendiente';
      this.plazoContrato = ' 0 meses / 0 días';
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
        console.log( this.seguros )
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

        return registroCompletoSeguros;
    }

    checkEsIncluidaPoliza( value: boolean, index: number ) {
        if ( value === false ) {
            this.seguros.controls[ index ].get( 'vigencia' ).setValue( null );
            this.seguros.controls[ index ].get( 'vigenciaAmparo' ).setValue( null );
            this.seguros.controls[ index ].get( 'valorAmparo' ).setValue( null );
        }
    }

    openDialog( modalTitle: string, modalText: string, reload = false ) {
        let ref= this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });

        ref.afterClosed().subscribe(result => {
          if(reload)
          {
            this.router.navigateByUrl( '/', { skipLocationChange: true } ).then( () => this.router.navigate( [ '/generarPolizasYGarantias' ] ) );
          }      
        });
    }

    onSubmit() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        // console.log(this.addressForm.value);
        let polizasList;
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

        return;
        if (this.addressForm.value.polizasYSeguros != undefined || this.addressForm.value.polizasYSeguros != null) {
          if ( this.addressForm.value.polizasYSeguros.length > 0 ) {
            polizasList = [this.addressForm.value.polizasYSeguros[0].codigo];
            for (let i = 1; i < this.addressForm.value.polizasYSeguros.length; i++) {
              const membAux = polizasList.push(this.addressForm.value.polizasYSeguros[i].codigo);
            }
            // console.log(polizasList);
          }
        }
        let nombreAprobado;
        if (this.addressForm.value.responsableAprob != undefined || this.addressForm.value.responsableAprob != null) {
          if (!this.addressForm.value.responsableAprob.usuarioId) {
            nombreAprobado = null;
          }
          else {
            nombreAprobado = this.addressForm.value.responsableAprob.usuarioId;
          }
        }
        var completo: boolean;
        if (this.addressForm.valid) {
          completo = true;
        }
        else {
          completo = false;
        }
        /*
          estadoPolizaCodigo: '3' => Envia el contrato al 3er acordeon principal "Con póliza observada y devuelta"
          estadoPolizaCodigo: '2' => Envia el contrato al 2er acordeon principal "En revisión de pólizas"
          Cuando el estado de revision es = devuelta( "1" ) redirige el contrato al 3er acordeon principal "Con póliza observada y devuelta" 
        */

        const contratoArray = {
          'contratoId': this.idContrato,
          'TipoSolicitudCodigo': "",
          'TipoModificacionCodigo': "",
          'DescripcionModificacion': "",
          'NombreAseguradora': this.addressForm.value.nombre,
          'NumeroPoliza': this.addressForm.value.numeroPoliza,
          'NumeroCertificado': this.addressForm.value.numeroCertificado,
          'Observaciones': "",
          'ObservacionesRevisionGeneral': this.addressForm.value.observacionesGenerales,
          'ResponsableAprobacion': nombreAprobado,
          'EstadoPolizaCodigo': this.addressForm.get( 'estadoRevision' ).value !== null ? ( this.addressForm.get( 'estadoRevision' ).value.codigo === this.estadosPoliza.sinRadicacion ? this.estadosPoliza.polizaDevuelta : this.estadosPoliza.enRevision) : this.estadosPoliza.enRevision,
          'UsuarioCreacion': "",
          'UsuarioModificacion': "",
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
          'FechaCreacion': null,
          'RegistroCompleto': false,
          'FechaModificacion': null,
          'Eliminado': false
        };
        let garantiaArray;
        this.polizaService.CreateContratoPoliza(contratoArray).subscribe(data => {
          if (data.isSuccessful == true) {
            //this.polizaService.CambiarEstadoPolizaByContratoId("2", this.idContrato).subscribe(resp0 => {});
            this.polizaService.GetContratoPolizaByIdContratoId(this.idContrato).subscribe(rep1 => {
              if (this.addressForm.value.polizasYSeguros != undefined || this.addressForm.value.polizasYSeguros != null) {
                for (let i = 0; i < polizasList.length; i++) {
                  switch (polizasList[i]) {
                    case '1':
                      garantiaArray = {
                        'contratoPolizaId': rep1.contratoPolizaId,
                        'TipoGarantiaCodigo': '1',
                        'EsIncluidaPoliza': this.addressForm.value.buenManejoCorrectaInversionAnticipo
                      };
                      this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r => {
                        this.realizoPeticion = true;
                      }, err => this.openDialog('', `<b>${err.message}</b>`) );
                      break;
                    case '2':
                      garantiaArray = {
                        'contratoPolizaId': rep1.contratoPolizaId,
                        'TipoGarantiaCodigo': '2',
                        'EsIncluidaPoliza': this.addressForm.value.estabilidadYCalidad
                      };
                      this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r1 => {
                        this.realizoPeticion = true;
                      }, err => this.openDialog('', `<b>${err.message}</b>`) );
                      break;
                    case '3':
                      garantiaArray = {
                        'contratoPolizaId': rep1.contratoPolizaId,
                        'TipoGarantiaCodigo': '3',
                        'EsIncluidaPoliza': this.addressForm.value.polizaYCoumplimiento
                      };
                      this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r2 => {
                        this.realizoPeticion = true;
                      }, err => this.openDialog('', `<b>${err.message}</b>`) );
                      break;
                    case '4':
                      garantiaArray = {
                        'contratoPolizaId': rep1.contratoPolizaId,
                        'TipoGarantiaCodigo': '4',
                        'EsIncluidaPoliza': this.addressForm.value.polizasYSegurosCompleto
                      };
                      this.polizaService.CreatePolizaGarantia(garantiaArray).subscribe(r3 => {
                        this.realizoPeticion = true;
                      }, err => this.openDialog('', `<b>${err.message}</b>`) );
                      break;
                  }
                }
              }
              const observacionArray = {
                polizaObservacionId: 0,
                contratoPolizaId: rep1.contratoPolizaId,
                observacion: this.addressForm.get( 'observacionesGenerales' ).value !== null ? this.addressForm.get( 'observacionesGenerales' ).value : '',
                fechaRevision: this.addressForm.get( 'fechaRevision' ).value !== null ? new Date( this.addressForm.get( 'fechaRevision' ).value ).toISOString() : new Date().toISOString(),
                estadoRevisionCodigo: this.addressForm.get( 'estadoRevision' ).value !== null ? this.addressForm.get( 'estadoRevision' ).value.codigo : this.estadosPoliza.enRevision
              }
              this.polizaService.createEditPolizaObservacion( observacionArray )
                .subscribe(
                  () => this.realizoPeticion = true,
                  err => this.openDialog('', `<b>${err.message}</b>`)
                );
            });
            this.realizoPeticion = true;
            this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>',true);
            
          }
          else {
            this.openDialog('', `<b>${data.message}</b>`);
          }
        }, err => this.openDialog('', `<b>${err.message}</b>`) );
    }

}
