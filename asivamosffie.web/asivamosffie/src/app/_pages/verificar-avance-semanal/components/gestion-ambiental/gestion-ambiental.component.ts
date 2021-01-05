import { VerificarAvanceSemanalService } from './../../../../core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { delay } from 'rxjs/operators';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-gestion-ambiental',
  templateUrl: './gestion-ambiental.component.html',
  styleUrls: ['./gestion-ambiental.component.scss']
})
export class GestionAmbientalComponent implements OnInit {

    @Input() esVerDetalle = false;
    @Input() seguimientoSemanal: any;
    @Output() seRealizoPeticion = new EventEmitter<boolean>();
    formGestionAmbiental: FormGroup;
    formGestionAmbientalObservacion: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ]
    });
    formMaterialObservacion: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ]
    });
    formResiduosConstruccion: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ]
    });
    formResiduosPeligrosos: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ]
    });
    formManejoOtra: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ]
    });
    tablaHistorial = new MatTableDataSource();
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
    dataHistorial: any[] = [
        {
            fechaRevision: new Date(),
            responsable: 'Apoyo a la supervisión',
            historial: '<p>Se recomienda que en cada actividad se especifique el responsable.</p>'
        }
    ];
    tipoActividades: Dominio[] = [];
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    obsGestionAmbientalId = 0; // ID de la observacion a gestion ambiental
    gestionAmbientalId = 0; // ID gestion ambiental
    manejoMaterialInsumoObsId = 0; // ID de la observacion a manejo de materiales e insumos
    residuosConstruccionObsId = 0; // ID de la observacion a residuos de construccion
    residuosPeligrososObsId = 0; // ID de la observacion a residuos peligrosos
    manejoOtrosObsId = 0; // ID de la observacion al manejo de otros
    gestionAmbiental: boolean;
    gestionObraAmbiental: any;
    cantidadActividades = 0;
    gestionAmbientalDetalle: any[] = [];
    tipoActividadesCodigo = {
        manejoMaterialInsumo: '1',
        manejoResiduosConstruccion: '2',
        manejoResiduosPeligrosos: '3',
        otra: '4'
    };
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

    get actividades() {
        return this.formGestionAmbiental.get( 'actividades' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private dialog: MatDialog,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private routes: Router )
    {
        this.crearFormulario();
        this.getListaActividades();
        this.getCantidadActividades();
    }

    ngOnInit(): void {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalGestionObraId =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
                this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId : 0;
            if (    this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0
                    && this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental.length > 0 )
            {
                this.cantidadActividades = 0;
                this.gestionObraAmbiental =     this.seguimientoSemanal.seguimientoSemanalGestionObra[0]
                                                .seguimientoSemanalGestionObraAmbiental[0];
                this.gestionAmbientalId = this.gestionObraAmbiental.seguimientoSemanalGestionObraAmbientalId;
                if ( this.gestionObraAmbiental.seEjecutoGestionAmbiental !== undefined ) {
                    this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' )
                        .setValue( this.gestionObraAmbiental.seEjecutoGestionAmbiental );
                    this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).markAsDirty();
                    this.gestionAmbiental = true;
                } else {
                    this.gestionAmbiental = false;
                }
                if ( this.gestionObraAmbiental.tieneManejoMaterialesInsumo === true ) {
                    this.cantidadActividades++;
                }
                if ( this.gestionObraAmbiental.tieneManejoResiduosConstruccionDemolicion === true ) {
                    this.cantidadActividades++;
                }
                if ( this.gestionObraAmbiental.tieneManejoResiduosPeligrososEspeciales === true ) {
                    this.cantidadActividades++;
                }
                if ( this.gestionObraAmbiental.tieneManejoOtro === true ) {
                    this.cantidadActividades++;
                }
                if ( this.gestionObraAmbiental.seEjecutoGestionAmbiental === true ) {
                    if ( this.cantidadActividades > 0 ) {
                    }
                    this.formGestionAmbiental.get( 'cantidadActividad' ).setValue( `${ this.cantidadActividades }` );
                }
            }
            this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
        }
    }

    getCantidadActividades() {
        this.formGestionAmbiental.get( 'cantidadActividad' ).valueChanges
            .pipe(
                delay( 1000 )
            )
            .subscribe( value => {
                if ( Number( value ) > 0 && (   this.gestionObraAmbiental === undefined
                                                || this.gestionObraAmbiental.tieneManejoMaterialesInsumo === undefined
                                                || this.gestionObraAmbiental.tieneManejoResiduosConstruccionDemolicion === undefined
                                                || this.gestionObraAmbiental.tieneManejoResiduosPeligrososEspeciales === undefined
                                                || this.gestionObraAmbiental.tieneManejoOtro === undefined ) ) {
                    this.actividades.clear();
                    this.tipoActividades = [];
                    this.getListaActividades();
                    for ( let i = 0; i < Number( value ); i++ ) {
                        this.actividades.push(
                            this.fb.group({
                                tipoActividad: [ null ],
                                estadoSemaforo: [ 'sin-diligenciar' ],
                                manejoMaterialInsumo: this.fb.group({
                                    manejoMaterialesInsumosId: [ 0 ],
                                    proveedores: this.fb.array( [] ),
                                    estanProtegidosDemarcadosMateriales: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    url: [ null ]
                                }),
                                manejoResiduosConstruccion: this.fb.group({
                                    manejoResiduosConstruccionDemolicionId: [ 0 ],
                                    estaCuantificadoRCD: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    manejoResiduosConstruccionDemolicionGestor: this.fb.array( [] ),
                                    seReutilizadorResiduos: [ null ],
                                    cantidadToneladas: [ '' ]
                                }),
                                manejoResiduosPeligrosos: this.fb.group({
                                    manejoResiduosPeligrososEspecialesId: [ 0 ],
                                    estanClasificados: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    urlRegistroFotografico: [ '' ]
                                }),
                                otra: this.fb.group({
                                    manejoOtroId: [ 0 ],
                                    fechaActividad: [ null ],
                                    actividad: [ null ],
                                    urlSoporteGestion: [ '' ]
                                })
                            })
                        );
                    }
                }
                if ( Number( value ) > 0 && this.gestionObraAmbiental !== undefined ) {

                    this.actividades.clear();
                    this.tipoActividades = [];
                    this.getListaActividades();
                    setTimeout(() => {
                    // GET manejo de materiales e insumos
                    if ( this.gestionObraAmbiental.tieneManejoMaterialesInsumo === true ) {
                        let estadoSemaforoMaterial = 'sin-diligenciar';
                        const actividadSeleccionada = this.tipoActividades.filter(
                            actividad => actividad.codigo === this.tipoActividadesCodigo.manejoMaterialInsumo
                        );
                        const manejoMaterial = this.gestionObraAmbiental.manejoMaterialesInsumo;

                        if ( manejoMaterial !== undefined && manejoMaterial.registroCompleto === false ) {
                            estadoSemaforoMaterial = 'en-proceso';
                        }

                        if ( manejoMaterial !== undefined && manejoMaterial.registroCompleto === true ) {
                            estadoSemaforoMaterial = 'completo';
                        }
                        this.actividades.push(
                            this.fb.group({
                                tipoActividad: [ actividadSeleccionada[0] ],
                                estadoSemaforo: [ estadoSemaforoMaterial ],
                                manejoMaterialInsumo: this.fb.group({
                                    manejoMaterialesInsumosId: [ 0 ],
                                    proveedores: this.fb.array( [] ),
                                    estanProtegidosDemarcadosMateriales: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    url: [ null ]
                                }),
                                manejoResiduosConstruccion: this.fb.group({
                                    manejoResiduosConstruccionDemolicionId: [ 0 ],
                                    estaCuantificadoRCD: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    manejoResiduosConstruccionDemolicionGestor: this.fb.array( [] ),
                                    seReutilizadorResiduos: [ null ],
                                    cantidadToneladas: [ '' ]
                                }),
                                manejoResiduosPeligrosos: this.fb.group({
                                    manejoResiduosPeligrososEspecialesId: [ 0 ],
                                    estanClasificados: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    urlRegistroFotografico: [ '' ]
                                }),
                                otra: this.fb.group({
                                    manejoOtroId: [ 0 ],
                                    fechaActividad: [ null ],
                                    actividad: [ null ],
                                    urlSoporteGestion: [ '' ]
                                })
                            })
                        );
                        this.valuePendingTipoActividad( actividadSeleccionada[0] );
                    }
                    // GET residuos de construccion y demolicion
                    if ( this.gestionObraAmbiental.tieneManejoResiduosConstruccionDemolicion === true ) {
                        let estadoSemaforoConstruccion = 'sin-diligenciar';
                        const actividadSeleccionada = this.tipoActividades.filter(
                            actividad => actividad.codigo === this.tipoActividadesCodigo.manejoResiduosConstruccion
                        );
                        const residuosConstruccion = this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion;

                        if ( residuosConstruccion !== undefined && residuosConstruccion.registroCompleto === false ) {
                            estadoSemaforoConstruccion = 'en-proceso';
                        }

                        if ( residuosConstruccion !== undefined && residuosConstruccion.registroCompleto === true ) {
                            estadoSemaforoConstruccion = 'completo';
                        }
                        this.actividades.push(
                            this.fb.group({
                                tipoActividad: [ actividadSeleccionada[0] ],
                                estadoSemaforo: [ estadoSemaforoConstruccion ],
                                manejoMaterialInsumo: this.fb.group({
                                    manejoMaterialesInsumosId: [ 0 ],
                                    proveedores: this.fb.array( [] ),
                                    estanProtegidosDemarcadosMateriales: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    url: [ null ]
                                }),
                                manejoResiduosConstruccion: this.fb.group({
                                    manejoResiduosConstruccionDemolicionId: [ 0 ],
                                    estaCuantificadoRCD: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    manejoResiduosConstruccionDemolicionGestor: this.fb.array( [] ),
                                    seReutilizadorResiduos: [ null ],
                                    cantidadToneladas: [ '' ]
                                }),
                                manejoResiduosPeligrosos: this.fb.group({
                                    manejoResiduosPeligrososEspecialesId: [ 0 ],
                                    estanClasificados: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    urlRegistroFotografico: [ '' ]
                                }),
                                otra: this.fb.group({
                                    manejoOtroId: [ 0 ],
                                    fechaActividad: [ null ],
                                    actividad: [ null ],
                                    urlSoporteGestion: [ '' ]
                                })
                            })
                        );
                        this.valuePendingTipoActividad( actividadSeleccionada[0] );
                    }
                    // GET manejo de residuos peligrosos y especiales
                    if ( this.gestionObraAmbiental.tieneManejoResiduosPeligrososEspeciales === true ) {
                        let estadoSemaforoEspeciales = 'sin-diligenciar';
                        const actividadSeleccionada = this.tipoActividades.filter(
                            actividad => actividad.codigo === this.tipoActividadesCodigo.manejoResiduosPeligrosos
                        );

                        const residuosEspeciales = this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales;

                        if ( residuosEspeciales !== undefined && residuosEspeciales.registroCompleto === false ) {
                            estadoSemaforoEspeciales = 'en-proceso';
                        }

                        if ( residuosEspeciales !== undefined && residuosEspeciales.registroCompleto === true ) {
                            estadoSemaforoEspeciales = 'completo';
                        }
                        this.actividades.push(
                            this.fb.group({
                                tipoActividad: [ actividadSeleccionada[0] ],
                                estadoSemaforo: [ estadoSemaforoEspeciales ],
                                manejoMaterialInsumo: this.fb.group({
                                    manejoMaterialesInsumosId: [ 0 ],
                                    proveedores: this.fb.array( [] ),
                                    estanProtegidosDemarcadosMateriales: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    url: [ null ]
                                }),
                                manejoResiduosConstruccion: this.fb.group({
                                    manejoResiduosConstruccionDemolicionId: [ 0 ],
                                    estaCuantificadoRCD: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    manejoResiduosConstruccionDemolicionGestor: this.fb.array( [] ),
                                    seReutilizadorResiduos: [ null ],
                                    cantidadToneladas: [ '' ]
                                }),
                                manejoResiduosPeligrosos: this.fb.group({
                                    manejoResiduosPeligrososEspecialesId: [ 0 ],
                                    estanClasificados: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    urlRegistroFotografico: [ '' ]
                                }),
                                otra: this.fb.group({
                                    manejoOtroId: [ 0 ],
                                    fechaActividad: [ null ],
                                    actividad: [ null ],
                                    urlSoporteGestion: [ '' ]
                                })
                            })
                        );
                        this.valuePendingTipoActividad( actividadSeleccionada[0] );
                    }
                    // GET manejo de otros
                    if ( this.gestionObraAmbiental.tieneManejoOtro === true ) {
                        let estadoSemaforoOtra = 'sin-diligenciar';
                        const actividadSeleccionada = this.tipoActividades.filter(
                            actividad => actividad.codigo === this.tipoActividadesCodigo.otra
                        );

                        const manejoOtros = this.gestionObraAmbiental.manejoOtro;

                        if ( manejoOtros !== undefined && manejoOtros.registroCompleto === false ) {
                            estadoSemaforoOtra = 'en-proceso';
                        }

                        if ( manejoOtros !== undefined && manejoOtros.registroCompleto === true ) {
                            estadoSemaforoOtra = 'completo';
                        }
                        this.actividades.push(
                            this.fb.group({
                                tipoActividad: [ actividadSeleccionada[0] ],
                                estadoSemaforo: [ estadoSemaforoOtra ],
                                manejoMaterialInsumo: this.fb.group({
                                    manejoMaterialesInsumosId: [ 0 ],
                                    proveedores: this.fb.array( [] ),
                                    estanProtegidosDemarcadosMateriales: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    url: [ null ]
                                }),
                                manejoResiduosConstruccion: this.fb.group({
                                    manejoResiduosConstruccionDemolicionId: [ 0 ],
                                    estaCuantificadoRCD: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    manejoResiduosConstruccionDemolicionGestor: this.fb.array( [] ),
                                    seReutilizadorResiduos: [ null ],
                                    cantidadToneladas: [ '' ]
                                }),
                                manejoResiduosPeligrosos: this.fb.group({
                                    manejoResiduosPeligrososEspecialesId: [ 0 ],
                                    estanClasificados: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    urlRegistroFotografico: [ '' ]
                                }),
                                otra: this.fb.group({
                                    manejoOtroId: [ 0 ],
                                    fechaActividad: [ null ],
                                    actividad: [ null ],
                                    urlSoporteGestion: [ '' ]
                                })
                            })
                        );
                        this.valuePendingTipoActividad( actividadSeleccionada[0] );
                    }

                    this.formGestionAmbiental.get( 'cantidadActividad' ).setValidators( Validators.min( this.actividades.length ) );
                    const nuevasActividades = Number( value ) - this.actividades.length;
                    if ( Number( value ) < this.actividades.length && Number( value ) > 0 ) {
                        this.openDialog(
                          '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
                        );
                        this.formGestionAmbiental.get( 'cantidadActividad' ).setValue( String( this.actividades.length ) );
                        return;
                    }
                    for ( let i = 0; i < nuevasActividades; i++ ) {
                        this.actividades.push(
                            this.fb.group({
                                tipoActividad: [ null ],
                                estadoSemaforo: [ 'sin-diligenciar' ],
                                manejoMaterialInsumo: this.fb.group({
                                    manejoMaterialesInsumosId: [ 0 ],
                                    proveedores: this.fb.array( [] ),
                                    estanProtegidosDemarcadosMateriales: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    url: [ null ]
                                }),
                                manejoResiduosConstruccion: this.fb.group({
                                    manejoResiduosConstruccionDemolicionId: [ 0 ],
                                    estaCuantificadoRCD: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    manejoResiduosConstruccionDemolicionGestor: this.fb.array( [] ),
                                    seReutilizadorResiduos: [ null ],
                                    cantidadToneladas: [ '' ]
                                }),
                                manejoResiduosPeligrosos: this.fb.group({
                                    manejoResiduosPeligrososEspecialesId: [ 0 ],
                                    estanClasificados: [ null ],
                                    requiereObservacion: [ null ],
                                    observacion: [ null ],
                                    urlRegistroFotografico: [ '' ]
                                }),
                                otra: this.fb.group({
                                    manejoOtroId: [ 0 ],
                                    fechaActividad: [ null ],
                                    actividad: [ null ],
                                    urlSoporteGestion: [ '' ]
                                })
                            })
                        );
                    }
                    }, 1500);
                }
            } );
    }

    crearFormulario() {
        this.formGestionAmbiental = this.fb.group({
            seEjecutoGestionAmbiental: [ null ],
            cantidadActividad: [ '' ],
            actividades: this.fb.array( [] )
        });
    }

    getListaActividades() {
        this.commonSvc.listaTipoActividades()
            .subscribe(
                actividades => {
                    this.tipoActividades = actividades;
                }
            );
    }

    valuePendingTipoActividad( actividad: Dominio ) {
        this.tipoActividades.filter( ( value, index ) => {
            if ( value.codigo === actividad.codigo ) {
                this.tipoActividades.splice( index, 1 );
            }
        } );
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
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

    guardar() {
        const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.obsGestionAmbientalId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: '4',
            observacionPadreId: this.seguimientoSemanalGestionObraId,
            observacion: this.formGestionAmbientalObservacion.get( 'observaciones' ).value,
            tieneObservacion: this.formGestionAmbientalObservacion.get( 'tieneObservaciones' ).value,
            esSupervisor: false
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () =>   this.routes.navigate(
                                    [
                                        '/verificarAvanceSemanal/verificarSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
                                    ]
                                )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    guardarManejoMaterial() {
        const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.manejoMaterialInsumoObsId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: '5',
            observacionPadreId: this.gestionAmbientalId,
            observacion: this.formMaterialObservacion.get( 'observaciones' ).value,
            tieneObservacion: this.formMaterialObservacion.get( 'tieneObservaciones' ).value,
            esSupervisor: false
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () =>   this.routes.navigate(
                                    [
                                        '/verificarAvanceSemanal/verificarSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
                                    ]
                                )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    guardarResiduosConstruccion() {
        const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.residuosConstruccionObsId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: '6',
            observacionPadreId: this.gestionAmbientalId,
            observacion: this.formResiduosConstruccion.get( 'observaciones' ).value,
            tieneObservacion: this.formResiduosConstruccion.get( 'tieneObservaciones' ).value,
            esSupervisor: false
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () =>   this.routes.navigate(
                                    [
                                        '/verificarAvanceSemanal/verificarSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
                                    ]
                                )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    guardarResiduosPeligrosos() {
        const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.residuosPeligrososObsId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: '7',
            observacionPadreId: this.gestionAmbientalId,
            observacion: this.formResiduosPeligrosos.get( 'observaciones' ).value,
            tieneObservacion: this.formResiduosPeligrosos.get( 'tieneObservaciones' ).value,
            esSupervisor: false
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () =>   this.routes.navigate(
                                    [
                                        '/verificarAvanceSemanal/verificarSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
                                    ]
                                )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    guardarManejoOtra() {
        const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.manejoOtrosObsId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: '8',
            observacionPadreId: this.gestionAmbientalId,
            observacion: this.formManejoOtra.get( 'observaciones' ).value,
            tieneObservacion: this.formManejoOtra.get( 'tieneObservaciones' ).value,
            esSupervisor: false
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () =>   this.routes.navigate(
                                    [
                                        '/verificarAvanceSemanal/verificarSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
                                    ]
                                )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
