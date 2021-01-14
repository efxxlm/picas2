import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
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
    @Input() tipoObservacionAmbiental: any;
    @Output() seRealizoPeticion = new EventEmitter<boolean>();
    formGestionAmbiental: FormGroup;
    formGestionAmbientalObservacion: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ],
        fechaCreacion: [ null ]
    });
    formMaterialObservacion: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ],
        fechaCreacion: [ null ]
    });
    formResiduosConstruccion: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ],
        fechaCreacion: [ null ]
    });
    formResiduosPeligrosos: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ],
        fechaCreacion: [ null ]
    });
    formManejoOtra: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null ],
        fechaCreacion: [ null ]
    });
    // MatTable historial de observaciones
    tablaHistorialgestionAmbiental = new MatTableDataSource();
    tablaHistorialManejoMateriales = new MatTableDataSource();
    tablaHistorialResiduosConstruccion = new MatTableDataSource();
    tablaHistorialResiduosPeligrosos = new MatTableDataSource();
    tablaHistorialManejoOtros = new MatTableDataSource();
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
    // Arreglos historial de observaciones
    historialGestionAmbiental: any[] = [];
    historialManejoMateriales: any[] = [];
    historialResiduosConstruccion: any[] = [];
    historialResiduosPeligrosos: any[] = [];
    historialManejoOtros: any[] = [];
    tipoActividades: Dominio[] = [];
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    gestionAmbiental: boolean;
    gestionObraAmbiental: any;
    cantidadActividades = 0;
    // Ids gestion ambiental y manejos.
    gestionAmbientalId = 0; // ID gestion ambiental.
    manejoMaterialInsumoId = 0; // ID manejo de materiales  e insumos.
    residuosConstruccionId = 0; // ID residuos de construccion.
    residuosPeligrososId = 0; // ID residuos peligrosos.
    manejoOtrosId = 0; // ID manejo de otros.
    // Ids observaciones gestion ambiental y manejos.
    obsGestionAmbientalId = 0; // ID de la observacion a gestion ambiental.
    manejoMaterialInsumoObsId = 0; // ID de la observacion a manejo de materiales e insumos.
    residuosConstruccionObsId = 0; // ID de la observacion a residuos de construccion.
    residuosPeligrososObsId = 0; // ID de la observacion a residuos peligrosos.
    manejoOtrosObsId = 0; // ID de la observacion al manejo de otros.
    // Observaciones apoyo a la gestion ambiental y actividades
    obsApoyoAmbiental: any[] = [];
    obsApoyoManejoMateriales: any[] = [];
    obsApoyoResiduosConstruccion: any[] = [];
    obsApoyoResiduosPeligrosos: any[] = [];
    obsApoyoOtros: any[] = [];
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
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private registrarAvanceSemanalSvc: RegistrarAvanceSemanalService,
        private dialog: MatDialog,
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
                // ID gestionAmbiental
                this.gestionAmbientalId = this.gestionObraAmbiental.seguimientoSemanalGestionObraAmbientalId;
                
                // IDs manejos
                /*
                    manejoMaterialInsumoId
                    residuosConstruccionId
                    residuosPeligrososId
                    manejoOtrosId
                */
                // ID manejo de materiales e insumos
                if ( this.gestionObraAmbiental.manejoMaterialesInsumo !== undefined ) {
                    this.manejoMaterialInsumoId = this.gestionObraAmbiental.manejoMaterialesInsumo.manejoMaterialesInsumosId;
                }
                // ID residuos construccion
                if ( this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion !== undefined ) {
                    this.residuosConstruccionId = this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion.manejoResiduosConstruccionDemolicionId;
                }
                // ID residuos peligrosos
                if ( this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales !== undefined ) {
                    this.residuosPeligrososId = this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales.manejoResiduosPeligrososEspecialesId;
                }
                // ID manejo de otros
                if ( this.gestionObraAmbiental.manejoOtro !== undefined ) {
                    this.manejoOtrosId = this.gestionObraAmbiental.manejoOtro.manejoOtroId;
                }
                // IDs observaciones ambientales y actividades
                /*
                    obsGestionAmbientalId
                    manejoMaterialInsumoObsId
                    residuosConstruccionObsId
                    residuosPeligrososObsId
                    manejoOtrosObsId
                */
                // ID gestion ambiental
                if ( this.gestionObraAmbiental.observacionApoyoId !== undefined || this.gestionObraAmbiental.observacionSupervisorId !== undefined ) {
                    if ( this.gestionObraAmbiental.observacionSupervisorId !== undefined ) {
                        this.obsGestionAmbientalId = this.gestionObraAmbiental.observacionSupervisorId;
                    }
                    // GET observacion gestion ambiental
                    this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.gestionAmbientalId, this.tipoObservacionAmbiental.gestionAmbientalCodigo )
                        .subscribe(
                            response => {
                                this.obsApoyoAmbiental = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                const obsSupervisorAmbiental = response.filter( obs => obs.archivada === false && obs.esSupervisor === true );
                                this.historialGestionAmbiental = response.filter( obs => obs.archivada === true );
                                this.tablaHistorialgestionAmbiental = new MatTableDataSource( this.historialGestionAmbiental );
                                if ( obsSupervisorAmbiental.length > 0 ) {
                                    if ( obsSupervisorAmbiental[0].observacion !== undefined ) {
                                        if ( obsSupervisorAmbiental[0].observacion.length > 0 ) {
                                            this.formGestionAmbientalObservacion.get( 'observaciones' ).setValue( obsSupervisorAmbiental[0].observacion );
                                        }
                                    }
                                    this.formGestionAmbientalObservacion.get( 'tieneObservaciones' ).setValue( this.gestionObraAmbiental.tieneObservacionSupervisor );
                                    this.formGestionAmbientalObservacion.get( 'fechaCreacion' ).setValue( obsSupervisorAmbiental[0].fechaCreacion );
                                }
                            }
                        );
                }
                // ID manejo de materiales e insumos
                if ( this.gestionObraAmbiental.manejoMaterialesInsumo !== undefined && ( this.gestionObraAmbiental.manejoMaterialesInsumo.observacionApoyoId !== undefined || this.gestionObraAmbiental.manejoMaterialesInsumo.observacionSupervisorId !== undefined ) ) {
                    if ( this.gestionObraAmbiental.manejoMaterialesInsumo.observacionSupervisorId !== undefined ) {
                        this.manejoMaterialInsumoObsId = this.gestionObraAmbiental.manejoMaterialesInsumo.observacionSupervisorId;
                    }
                    // GET observacion manejo de materiales e insumos
                    this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.manejoMaterialInsumoId, this.tipoObservacionAmbiental.manejoMateriales )
                        .subscribe(
                            response => {
                                this.obsApoyoManejoMateriales = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                const obsSupervisorManejoMateriales = response.filter( obs => obs.archivada === false && obs.esSupervisor === true );
                                this.historialManejoMateriales = response.filter( obs => obs.archivada === true );
                                this.tablaHistorialManejoMateriales = new MatTableDataSource( this.historialManejoMateriales );
                                if ( obsSupervisorManejoMateriales.length > 0 ) {
                                    if ( obsSupervisorManejoMateriales[0].observacion !== undefined ) {
                                        if ( obsSupervisorManejoMateriales[0].observacion.length > 0 ) {
                                            this.formMaterialObservacion.get( 'observaciones' ).setValue( obsSupervisorManejoMateriales[0].observacion );
                                        }
                                    }
                                    this.formMaterialObservacion.get( 'tieneObservaciones' ).setValue( this.gestionObraAmbiental.manejoMaterialesInsumo.tieneObservacionSupervisor );
                                    this.formMaterialObservacion.get( 'fechaCreacion' ).setValue( obsSupervisorManejoMateriales[0].fechaCreacion );
                                }
                            }
                        );
                }
                // ID residuos de construccion
                if ( this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion !== undefined && ( this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion.observacionApoyoId !== undefined || this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion.observacionSupervisorId !== undefined ) ) {
                    if ( this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion.observacionSupervisorId !== undefined ) {
                        this.residuosConstruccionObsId = this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion.observacionSupervisorId;
                    }
                    // GET observacion residuos de construccion
                    this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.residuosConstruccionId, this.tipoObservacionAmbiental.residuosConstruccion )
                        .subscribe(
                            response => {
                                this.obsApoyoResiduosConstruccion = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                const obsSupervisorResiduosConstruccion = response.filter( obs => obs.archivada === false && obs.esSupervisor === true );
                                this.historialResiduosConstruccion = response.filter( obs => obs.archivada === true );
                                this.tablaHistorialResiduosConstruccion = new MatTableDataSource( this.historialResiduosConstruccion );
                                if ( obsSupervisorResiduosConstruccion.length > 0 ) {
                                    if ( obsSupervisorResiduosConstruccion[0].observacion !== undefined ) {
                                        if ( obsSupervisorResiduosConstruccion[0].observacion.length > 0 ) {
                                            this.formResiduosConstruccion.get( 'observaciones' ).setValue( obsSupervisorResiduosConstruccion[0].observacion );
                                        }
                                    }
                                    this.formResiduosConstruccion.get( 'tieneObservaciones' ).setValue( this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion.tieneObservacionSupervisor );
                                    this.formResiduosConstruccion.get( 'fechaCreacion' ).setValue( obsSupervisorResiduosConstruccion[0].fechaCreacion );
                                }
                            }
                        );
                }
                // ID residuos peligrosos
                if ( this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales !== undefined && ( this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales.observacionApoyoId !== undefined || this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales.observacionSupervisorId !== undefined ) ) {
                    if ( this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales.observacionSupervisorId !== undefined ) {
                        this.residuosPeligrososObsId = this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales.observacionSupervisorId;
                    }
                    // GET observacion residuos peligrosos
                    this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.residuosPeligrososId, this.tipoObservacionAmbiental.residuosPeligrosos )
                        .subscribe(
                            response => {
                                this.obsApoyoResiduosPeligrosos = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                const obsSupervisorResiduosPeligrosos = response.filter( obs => obs.archivada === false && obs.esSupervisor === true );
                                this.historialResiduosPeligrosos = response.filter( obs => obs.archivada === true );
                                this.tablaHistorialResiduosPeligrosos = new MatTableDataSource( this.historialResiduosPeligrosos );
                                if ( obsSupervisorResiduosPeligrosos.length > 0 ) {
                                    if ( obsSupervisorResiduosPeligrosos[0].observacion !== undefined ) {
                                        if ( obsSupervisorResiduosPeligrosos[0].observacion.length > 0 ) {
                                            this.formResiduosPeligrosos.get( 'observaciones' ).setValue( obsSupervisorResiduosPeligrosos[0].observacion );
                                        }
                                    }
                                    this.formResiduosPeligrosos.get( 'tieneObservaciones' ).setValue( this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales.tieneObservacionSupervisor );
                                    this.formResiduosPeligrosos.get( 'fechaCreacion' ).setValue( obsSupervisorResiduosPeligrosos[0].fechaCreacion );
                                }
                            }
                        )
                }
                // ID manejo de otros
                if ( this.gestionObraAmbiental.manejoOtro !== undefined && ( this.gestionObraAmbiental.manejoOtro.observacionApoyoId !== undefined || this.gestionObraAmbiental.manejoOtro.observacionSupervisorId !== undefined ) ) {
                    if ( this.gestionObraAmbiental.manejoOtro.observacionSupervisorId !== undefined ) {
                        this.manejoOtrosObsId = this.gestionObraAmbiental.manejoOtro.observacionSupervisorId;
                    }
                    // GET observacion manejo de otros
                    this.registrarAvanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.manejoOtrosId, this.tipoObservacionAmbiental.manejoOtra )
                        .subscribe(
                            response => {
                                this.obsApoyoOtros = response.filter( obs => obs.archivada === false && obs.esSupervisor === false );
                                const obsSupervisorOtros = response.filter( obs => obs.archivada === false && obs.esSupervisor === true );
                                this.historialManejoOtros = response.filter( obs => obs.archivada === true );
                                this.tablaHistorialManejoOtros = new MatTableDataSource( this.historialManejoOtros );
                                if ( obsSupervisorOtros.length > 0 ) {
                                    if ( obsSupervisorOtros[0].observacion !== undefined ) {
                                        if ( obsSupervisorOtros[0].observacion.length > 0 ) {
                                            this.formManejoOtra.get( 'observaciones' ).setValue( obsSupervisorOtros[0].observacion );
                                        }
                                    }
                                    this.formManejoOtra.get( 'tieneObservaciones' ).setValue( this.gestionObraAmbiental.manejoOtro.tieneObservacionSupervisor );
                                    this.formManejoOtra.get( 'fechaCreacion' ).setValue( obsSupervisorOtros[0].fechaCreacion );
                                }
                            }
                        )
                }

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

                        if ( manejoMaterial !== undefined && manejoMaterial.registroCompletoObservacionSupervisor === false ) {
                            estadoSemaforoMaterial = 'en-proceso';
                        }

                        if ( manejoMaterial !== undefined && manejoMaterial.registroCompletoObservacionSupervisor === true ) {
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

                        if ( residuosConstruccion !== undefined && residuosConstruccion.registroCompletoObservacionSupervisor === false ) {
                            estadoSemaforoConstruccion = 'en-proceso';
                        }

                        if ( residuosConstruccion !== undefined && residuosConstruccion.registroCompletoObservacionSupervisor === true ) {
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

                        if ( residuosEspeciales !== undefined && residuosEspeciales.registroCompletoObservacionSupervisor === false ) {
                            estadoSemaforoEspeciales = 'en-proceso';
                        }

                        if ( residuosEspeciales !== undefined && residuosEspeciales.registroCompletoObservacionSupervisor === true ) {
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

                        if ( manejoOtros !== undefined && manejoOtros.registroCompletoObservacionSupervisor === false ) {
                            estadoSemaforoOtra = 'en-proceso';
                        }

                        if ( manejoOtros !== undefined && manejoOtros.registroCompletoObservacionSupervisor === true ) {
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
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        if ( this.formGestionAmbientalObservacion.get( 'tieneObservaciones' ).value === false && this.formGestionAmbientalObservacion.get( 'observaciones' ).value !== null ) {
            this.formGestionAmbientalObservacion.get( 'observaciones' ).setValue( '' );
        }
        const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.obsGestionAmbientalId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoObservacionAmbiental.gestionAmbientalCodigo,
            observacionPadreId: this.gestionAmbientalId,
            observacion: this.formGestionAmbientalObservacion.get( 'observaciones' ).value,
            tieneObservacion: this.formGestionAmbientalObservacion.get( 'tieneObservaciones' ).value,
            esSupervisor: true
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.verificarAvanceSemanalSvc.getValidarRegistroCompletoObservaciones( this.seguimientoSemanalId, 'True' )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () =>   this.routes.navigate(
                                                [
                                                    '/validarAvanceSemanal/validarSeguimientoSemanal', this.seguimientoSemanalId
                                                ]
                                            )
                                );
                            }
                        );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    guardarManejoMaterial() {
        if ( this.formMaterialObservacion.get( 'tieneObservaciones' ).value === false && this.formMaterialObservacion.get( 'observaciones' ).value !== null ) {
            this.formMaterialObservacion.get( 'observaciones' ).setValue( '' );
        }
        const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.manejoMaterialInsumoObsId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoObservacionAmbiental.manejoMateriales,
            observacionPadreId: this.manejoMaterialInsumoId,
            observacion: this.formMaterialObservacion.get( 'observaciones' ).value,
            tieneObservacion: this.formMaterialObservacion.get( 'tieneObservaciones' ).value,
            esSupervisor: true
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.verificarAvanceSemanalSvc.getValidarRegistroCompletoObservaciones( this.seguimientoSemanalId, 'True' )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () =>   this.routes.navigate(
                                                [
                                                    '/validarAvanceSemanal/validarSeguimientoSemanal', this.seguimientoSemanalId
                                                ]
                                            )
                                );
                            }
                        );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    guardarResiduosConstruccion() {
        if ( this.formResiduosConstruccion.get( 'tieneObservaciones' ).value === false && this.formResiduosConstruccion.get( 'observaciones' ).value !== null ) {
            this.formResiduosConstruccion.get( 'observaciones' ).setValue( '' );
        }
        const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.residuosConstruccionObsId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoObservacionAmbiental.residuosConstruccion,
            observacionPadreId: this.residuosConstruccionId,
            observacion: this.formResiduosConstruccion.get( 'observaciones' ).value,
            tieneObservacion: this.formResiduosConstruccion.get( 'tieneObservaciones' ).value,
            esSupervisor: true
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.verificarAvanceSemanalSvc.getValidarRegistroCompletoObservaciones( this.seguimientoSemanalId, 'True' )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () =>   this.routes.navigate(
                                                [
                                                    '/validarAvanceSemanal/validarSeguimientoSemanal', this.seguimientoSemanalId
                                                ]
                                            )
                                );
                            }
                        );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    guardarResiduosPeligrosos() {
        if ( this.formResiduosPeligrosos.get( 'tieneObservaciones' ).value === false && this.formResiduosPeligrosos.get( 'observaciones' ).value !== null ) {
            this.formResiduosPeligrosos.get( 'observaciones' ).setValue( '' );
        }
        const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.residuosPeligrososObsId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoObservacionAmbiental.residuosPeligrosos,
            observacionPadreId: this.residuosPeligrososId,
            observacion: this.formResiduosPeligrosos.get( 'observaciones' ).value,
            tieneObservacion: this.formResiduosPeligrosos.get( 'tieneObservaciones' ).value,
            esSupervisor: true
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.verificarAvanceSemanalSvc.getValidarRegistroCompletoObservaciones( this.seguimientoSemanalId, 'True' )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () =>   this.routes.navigate(
                                                [
                                                    '/validarAvanceSemanal/validarSeguimientoSemanal', this.seguimientoSemanalId
                                                ]
                                            )
                                );
                            }
                        );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    guardarManejoOtra() {
        if ( this.formManejoOtra.get( 'tieneObservaciones' ).value === false && this.formManejoOtra.get( 'observaciones' ).value !== null ) {
            this.formManejoOtra.get( 'observaciones' ).setValue( '' );
        }
        const pSeguimientoSemanalObservacion = {
			seguimientoSemanalObservacionId: this.manejoOtrosObsId,
            seguimientoSemanalId: this.seguimientoSemanalId,
            tipoObservacionCodigo: this.tipoObservacionAmbiental.manejoOtra,
            observacionPadreId: this.manejoOtrosId,
            observacion: this.formManejoOtra.get( 'observaciones' ).value,
            tieneObservacion: this.formManejoOtra.get( 'tieneObservaciones' ).value,
            esSupervisor: true
        }
        console.log( pSeguimientoSemanalObservacion );
        this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( pSeguimientoSemanalObservacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.verificarAvanceSemanalSvc.getValidarRegistroCompletoObservaciones( this.seguimientoSemanalId, 'True' )
                        .subscribe(
                            () => {
                                this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                                    () =>   this.routes.navigate(
                                                [
                                                    '/validarAvanceSemanal/validarSeguimientoSemanal', this.seguimientoSemanalId
                                                ]
                                            )
                                );
                            }
                        );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
