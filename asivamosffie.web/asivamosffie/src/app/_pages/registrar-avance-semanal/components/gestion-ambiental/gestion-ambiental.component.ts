import { GuardadoParcialAvanceSemanalService } from './../../../../core/_services/guardadoParcialAvanceSemanal/guardado-parcial-avance-semanal.service';
import { delay } from 'rxjs/operators';
import { Router } from '@angular/router';
import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms';
import { Component, Input, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { VerificarAvanceSemanalService } from 'src/app/core/_services/verificarAvanceSemanal/verificar-avance-semanal.service';

@Component({
  selector: 'app-gestion-ambiental',
  templateUrl: './gestion-ambiental.component.html',
  styleUrls: ['./gestion-ambiental.component.scss']
})
export class GestionAmbientalComponent implements OnInit, OnDestroy {

    @Input() esVerDetalle = false;
    @Input() esRegistroNuevo: boolean;
    @Input() seguimientoSemanal: any;
    @Input() tipoObservacionAmbiental: any;
    @Output() dataGestionAmbiental = new EventEmitter<any>();
    @Output() tieneObservacion = new EventEmitter();
    obsApoyo: any;
    obsSupervisor: any;
    obsApoyoMaterialInsumo: any;
    obsSupervisorMaterialInsumo: any;
    obsApoyoResiduosConstruccion: any;
    obsSupervisorResiduosConstruccion: any;
    obsApoyoResiduosPeligrosos: any;
    obsSupervisorResiduosPeligrosos: any;
    obsApoyoManejoOtra: any;
    obsSupervisorManejoOtra: any;
    seRealizoPeticion = false;
    formGestionAmbiental: FormGroup;
    tipoActividades: Dominio[] = [];
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    gestionAmbiental: boolean;
    gestionObraAmbiental: any;
    cantidadActividades = 0;
    gestionAmbientalDetalle: any[] = [];
    // Ids gestion ambiental y manejos.
    gestionAmbientalId = 0; // ID gestion ambiental.
    manejoMaterialInsumoId = 0; // ID manejo de materiales  e insumos.
    residuosConstruccionId = 0; // ID residuos de construccion.
    residuosPeligrososId = 0; // ID residuos peligrosos.
    manejoOtrosId = 0; // ID manejo de otros.
    // MatTable historial de observaciones
    tablaHistorialgestionAmbiental = new MatTableDataSource();
    tablaHistorialManejoMateriales = new MatTableDataSource();
    tablaHistorialResiduosConstruccion = new MatTableDataSource();
    tablaHistorialResiduosPeligrosos = new MatTableDataSource();
    tablaHistorialManejoOtros = new MatTableDataSource();
    // Arreglos historial de observaciones
    historialGestionAmbiental: any[] = [];
    historialManejoMateriales: any[] = [];
    historialResiduosConstruccion: any[] = [];
    historialResiduosPeligrosos: any[] = [];
    historialManejoOtros: any[] = [];
    booleanosActividadRelacionada: any[] = [
        { value: true, viewValue: 'Si' },
        { value: false, viewValue: 'No' }
    ];
    tipoActividadesCodigo = {
        manejoMaterialInsumo: '1',
        manejoResiduosConstruccion: '2',
        manejoResiduosPeligrosos: '3',
        otra: '4'
    };
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];

    get actividades() {
        return this.formGestionAmbiental.get( 'actividades' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private dialog: MatDialog,
        private routes: Router,
        private avanceSemanalSvc: RegistrarAvanceSemanalService,
        private verificarAvanceSemanalSvc: VerificarAvanceSemanalService,
        private guardadoParcialAvanceSemanalSvc: GuardadoParcialAvanceSemanalService )
    {
        this.crearFormulario();
        this.getListaActividades();
        this.getCantidadActividades();
    }
    ngOnDestroy(): void {
        if ( this.seRealizoPeticion === false ) {
            if ( this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).value === true ) {
                if ( this.actividades.dirty === true ) {
                    this.guardadoParcialAvanceSemanalSvc.getDataGestionAmbiental( this.guardadoParcial(), this.seRealizoPeticion );
                } else {
                    this.guardadoParcialAvanceSemanalSvc.getDataGestionAmbiental( undefined );
                }
            }
            if ( this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).value === false ) {
                if ( this.formGestionAmbiental.dirty === true ) {
                    this.guardadoParcialAvanceSemanalSvc.getDataGestionAmbiental( this.guardadoParcial(), this.seRealizoPeticion );
                } else {
                    this.guardadoParcialAvanceSemanalSvc.getDataGestionAmbiental( undefined );
                }
            }
        } else {
            this.guardadoParcialAvanceSemanalSvc.getDataGestionAmbiental( undefined );
        }
    }

    ngOnInit(): void {
        this.getGestionAmbiental();
    }

    crearFormulario() {
        this.formGestionAmbiental = this.fb.group({
            seEjecutoGestionAmbiental: [ null ],
            cantidadActividad: [ '' ],
            actividades: this.fb.array( [] )
        });
    }

    getGestionAmbiental() {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalGestionObraId =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
                this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId : 0;
            if ( this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 && this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental.length > 0 ) {
                this.cantidadActividades = 0;
                this.gestionObraAmbiental = this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental[0];

                if ( this.gestionObraAmbiental.seEjecutoGestionAmbiental !== undefined ) {
                    this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).setValue( this.gestionObraAmbiental.seEjecutoGestionAmbiental );
                    this.gestionAmbiental = true;
                } else {
                    this.gestionAmbiental = false;
                }

                if ( this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).value === false ) {
                    // ID gestionAmbiental
                    this.gestionAmbientalId = this.gestionObraAmbiental.seguimientoSemanalGestionObraAmbientalId;

                    if ( this.esVerDetalle === false ) {
                        this.avanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.gestionAmbientalId, this.tipoObservacionAmbiental.gestionAmbientalCodigo )
                            .subscribe(
                                response => {
                                    this.obsApoyo = response.find( obs => obs.archivada === false && obs.esSupervisor === false );
                                    this.obsSupervisor = response.find( obs => obs.archivada === false && obs.esSupervisor === true );
                                    this.historialGestionAmbiental = response;

                                    if ( this.obsApoyo !== undefined || this.obsSupervisor !== undefined ) {
                                        this.tieneObservacion.emit();
                                    }

                                    this.tablaHistorialgestionAmbiental = new MatTableDataSource( this.historialGestionAmbiental );
                                }
                            );
                    }
                }
                if ( this.gestionObraAmbiental.tieneManejoMaterialesInsumo === true ) {
                    // ID manejo de materiales e insumos
                    if ( this.gestionObraAmbiental.manejoMaterialesInsumo !== undefined ) {
                        this.manejoMaterialInsumoId = this.gestionObraAmbiental.manejoMaterialesInsumo.manejoMaterialesInsumosId;
                    }

                    this.cantidadActividades++;
                }
                if ( this.gestionObraAmbiental.tieneManejoResiduosConstruccionDemolicion === true ) {
                    // ID residuos construccion
                    if ( this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion !== undefined ) {
                        this.residuosConstruccionId = this.gestionObraAmbiental.manejoResiduosConstruccionDemolicion.manejoResiduosConstruccionDemolicionId;
                    }

                    this.cantidadActividades++;
                }
                if ( this.gestionObraAmbiental.tieneManejoResiduosPeligrososEspeciales === true ) {
                    // ID residuos peligrosos
                    if ( this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales !== undefined ) {
                        this.residuosPeligrososId = this.gestionObraAmbiental.manejoResiduosPeligrososEspeciales.manejoResiduosPeligrososEspecialesId;
                    }

                    this.cantidadActividades++;
                }
                if ( this.gestionObraAmbiental.tieneManejoOtro === true ) {
                    // ID manejo de otros
                    if ( this.gestionObraAmbiental.manejoOtro !== undefined ) {
                        this.manejoOtrosId = this.gestionObraAmbiental.manejoOtro.manejoOtroId;
                    }

                    this.cantidadActividades++;
                }
                if ( this.gestionObraAmbiental.seEjecutoGestionAmbiental === true ) {
                    if ( this.cantidadActividades > 0 ) {
                    }
                    this.formGestionAmbiental.get( 'cantidadActividad' ).setValue( `${ this.cantidadActividades }` );
                }
            }
        }

        if(!this.esRegistroNuevo) this.formGestionAmbiental.markAllAsTouched();
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

                        if ( this.esVerDetalle === false ) {
                            this.avanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.manejoMaterialInsumoId, this.tipoObservacionAmbiental.manejoMateriales )
                                .subscribe(
                                    response => {
                                        this.obsApoyoMaterialInsumo = response.find( obs => obs.archivada === false && obs.esSupervisor === false );
                                        this.obsSupervisorMaterialInsumo = response.find( obs => obs.archivada === false && obs.esSupervisor === true );
                                        this.historialManejoMateriales = response;

                                        if ( this.obsApoyoMaterialInsumo !== undefined || this.obsSupervisorMaterialInsumo !== undefined ) {
                                            estadoSemaforoMaterial = 'en-proceso';
                                            this.tieneObservacion.emit();
                                        }

                                        this.tablaHistorialManejoMateriales = new MatTableDataSource( this.historialManejoMateriales );
                                    }
                                );                        
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

                        if ( this.esVerDetalle === false ) {
                            this.avanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.residuosConstruccionId, this.tipoObservacionAmbiental.residuosConstruccion )
                                .subscribe(
                                    response => {
                                        this.obsApoyoResiduosConstruccion = response.find( obs => obs.archivada === false && obs.esSupervisor === false );
                                        this.obsSupervisorResiduosConstruccion = response.find( obs => obs.archivada === false && obs.esSupervisor === true );
                                        this.historialResiduosConstruccion = response;

                                        if ( this.obsApoyoResiduosConstruccion !== undefined || this.obsSupervisorResiduosConstruccion !== undefined ) {
                                            estadoSemaforoConstruccion = 'en-proceso';
                                            this.tieneObservacion.emit();
                                        }

                                        this.tablaHistorialResiduosConstruccion = new MatTableDataSource( this.historialResiduosConstruccion );
                                    }
                                );
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

                        if ( this.esVerDetalle === false ) {
                            this.avanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.residuosPeligrososId, this.tipoObservacionAmbiental.residuosPeligrosos )
                                .subscribe(
                                    response => {
                                        this.obsApoyoResiduosPeligrosos = response.find( obs => obs.archivada === false && obs.esSupervisor === false );
                                        this.obsSupervisorResiduosPeligrosos = response.find( obs => obs.archivada === false && obs.esSupervisor === true );
                                        this.historialResiduosPeligrosos = response;

                                        if ( this.obsApoyoResiduosPeligrosos !== undefined || this.obsSupervisorResiduosPeligrosos !== undefined ) {
                                            estadoSemaforoEspeciales = 'en-proceso';
                                            this.tieneObservacion.emit();
                                        }

                                        this.tablaHistorialResiduosPeligrosos = new MatTableDataSource( this.historialResiduosPeligrosos );
                                    }
                                )
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

                        if ( this.esVerDetalle === false ) {
                            this.avanceSemanalSvc.getObservacionSeguimientoSemanal( this.seguimientoSemanalId, this.manejoOtrosId, this.tipoObservacionAmbiental.manejoOtra )
                                .subscribe(
                                    response => {
                                        this.obsApoyoManejoOtra = response.find( obs => obs.archivada === false && obs.esSupervisor === false );
                                        this.obsSupervisorManejoOtra = response.find( obs => obs.archivada === false && obs.esSupervisor === true );
                                        this.historialManejoOtros = response;

                                        if ( this.obsApoyoManejoOtra !== undefined || this.obsSupervisorManejoOtra !== undefined ) {
                                            estadoSemaforoOtra = 'en-proceso';
                                            this.tieneObservacion.emit();
                                        }

                                        this.tablaHistorialManejoOtros = new MatTableDataSource( this.historialManejoOtros );
                                    }
                                )
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
                if(!this.esRegistroNuevo) this.formGestionAmbiental.markAllAsTouched()
            } );
    }

    getListaActividades() {
        this.commonSvc.listaTipoActividades()
            .subscribe(
                actividades => {
                    this.tipoActividades = actividades;
                }
            );
    }

    valuePending( value: string ) {
        if ( isNaN( Number( value ) ) === true ) {
            this.formGestionAmbiental.get( 'cantidadActividad' ).setValue( '' );
        } else {
            if ( value.length > 0 ) {
                if ( Number( value ) <= 0 ) {
                    this.formGestionAmbiental.get( 'cantidadActividad' ).setValue( '1' );
                }
                if ( Number( value ) > 4 ) {
                    this.formGestionAmbiental.get( 'cantidadActividad' ).setValue( '4' );
                }
            }
        }

        if(!this.esRegistroNuevo) this.formGestionAmbiental.markAllAsTouched();
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

    async guardar() {
        this.seRealizoPeticion = true;
        const pSeguimientoSemanal = this.seguimientoSemanal;
        let seguimientoSemanalGestionObra;
        /*
            obsApoyoManejoOtra: any;
            obsSupervisorManejoOtra: any;
        */

        if ( this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).value === false ) {
            const gestionObra = this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ? this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental : [];

            seguimientoSemanalGestionObra = [
                {
                    seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
                    seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                    seguimientoSemanalGestionObraAmbiental: [
                        {
                            seguimientoSemanalGestionObraAmbientalId: gestionObra.length > 0 ? gestionObra[0].seguimientoSemanalGestionObraAmbientalId : 0,
                            seEjecutoGestionAmbiental: this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).value
                        }
                    ]
                }
            ];

            if ( pSeguimientoSemanal.seguimientoSemanalGestionObra !== undefined ) {
                if ( pSeguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ) {
                    pSeguimientoSemanal.seguimientoSemanalGestionObra[ 0 ].seguimientoSemanalGestionObraAmbiental = [
                        {
                            seguimientoSemanalGestionObraAmbientalId: gestionObra.length > 0 ? gestionObra[0].seguimientoSemanalGestionObraAmbientalId : 0,
                            seEjecutoGestionAmbiental: this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).value
                        }
                    ]
                } else {
                    pSeguimientoSemanal.seguimientoSemanalGestionObra = seguimientoSemanalGestionObra;
                }
            } else {
                pSeguimientoSemanal.seguimientoSemanalGestionObra = seguimientoSemanalGestionObra;
            }
            // obsApoyo
            // obsSupervisor
            if ( this.obsApoyo !== undefined ) {
                this.obsApoyo.archivada = !this.obsApoyo.archivada;
                await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsApoyo ).toPromise();
            }
            if ( this.obsSupervisor !== undefined ) {
                this.obsSupervisor.archivada = !this.obsSupervisor.archivada;
                await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsSupervisor ).toPromise();
            }
        }

        if ( this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).value === true && this.actividades.length > 0 ) {

            // GET metodo actividades seleccionadas en cada acordeon
            const actividadSeleccionada = ( tipoActividad ) => {
                const selectActividad = this.actividades.controls.filter( actividad => actividad.get( 'tipoActividad' ).value !== null && actividad.get( 'tipoActividad' ).value.codigo === tipoActividad );

                return selectActividad.length > 0 ? selectActividad[0] : null;
            };

            // GET gestion de obra ambiental
            const gestionObra = this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ? this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental : [];

            // GET reactive form "Manejo de materiales e insumos"
            const manejoMaterialInsumo = async () => {
                const manejoMaterial = actividadSeleccionada( this.tipoActividadesCodigo.manejoMaterialInsumo ) !== null ? actividadSeleccionada( this.tipoActividadesCodigo.manejoMaterialInsumo ).get( 'manejoMaterialInsumo' ) : null;

                if ( manejoMaterial !== null ) {
                    if ( manejoMaterial.dirty === true ) {
                        // obsApoyoMaterialInsumo
                        // obsSupervisorMaterialInsumo
                        if ( this.obsApoyoMaterialInsumo !== undefined ) {
                            this.obsApoyoMaterialInsumo.archivada = !this.obsApoyoMaterialInsumo.archivada;
                            await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsApoyoMaterialInsumo ).toPromise();
                        }
                        if ( this.obsSupervisorMaterialInsumo !== undefined ) {
                            this.obsSupervisorMaterialInsumo.archivada = !this.obsSupervisorMaterialInsumo.archivada;
                            await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsSupervisorMaterialInsumo ).toPromise();
                        }

                        return {
                            ManejoMaterialesInsumosId: manejoMaterial.get( 'manejoMaterialesInsumosId' ).value,
                            manejoMaterialesInsumosProveedor: manejoMaterial.get( 'proveedores' ).value,
                            estanProtegidosDemarcadosMateriales: manejoMaterial.get( 'estanProtegidosDemarcadosMateriales' ).value,
                            requiereObservacion: manejoMaterial.get( 'requiereObservacion' ).value,
                            observacion: manejoMaterial.get( 'requiereObservacion' ).value === true ? manejoMaterial.get( 'observacion' ).value : null,
                            url: manejoMaterial.get( 'url' ).value
                        };
                    } else {
                        return gestionObra.length > 0 && gestionObra[0].manejoMaterialesInsumo !== undefined ? gestionObra[0].manejoMaterialesInsumo : manejoMaterial.value;
                    }
                } else {
                    return gestionObra.length > 0 && gestionObra[0].manejoMaterialesInsumo !== undefined ? gestionObra[0].manejoMaterialesInsumo : null;
                }
            };
            // GET reactive form "Residuos de construccion y demolicion"
            const manejoResiduosConstruccionDemolicion = async () => {
                const residuoConstruccion = actividadSeleccionada( this.tipoActividadesCodigo.manejoResiduosConstruccion ) !== null ? actividadSeleccionada( this.tipoActividadesCodigo.manejoResiduosConstruccion ).get( 'manejoResiduosConstruccion' ) : null;

                if ( residuoConstruccion !== null ) {
                    // obsApoyoResiduosConstruccion
                    // obsSupervisorResiduosConstruccion
                    if ( residuoConstruccion.dirty === true ) {
                        if ( this.obsApoyoResiduosConstruccion !== undefined ) {
                            this.obsApoyoResiduosConstruccion.archivada = !this.obsApoyoResiduosConstruccion.archivada;
                            await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsApoyoResiduosConstruccion ).toPromise();
                        }
                        if ( this.obsSupervisorResiduosConstruccion !== undefined ) {
                            this.obsSupervisorResiduosConstruccion.archivada = !this.obsSupervisorResiduosConstruccion.archivada;
                            await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsSupervisorResiduosConstruccion ).toPromise();
                        }

                        return {
                            manejoResiduosConstruccionDemolicionId: residuoConstruccion.get('manejoResiduosConstruccionDemolicionId').value,
                            estaCuantificadoRCD: residuoConstruccion.get( 'estaCuantificadoRCD' ).value,
                            requiereObservacion: residuoConstruccion.get( 'requiereObservacion' ).value,
                            observacion: residuoConstruccion.get( 'observacion' ).value,
                            manejoResiduosConstruccionDemolicionGestor: residuoConstruccion.get( 'manejoResiduosConstruccionDemolicionGestor' ).value,
                            seReutilizadorResiduos: residuoConstruccion.get( 'seReutilizadorResiduos' ).value,
                            cantidadToneladas: residuoConstruccion.get( 'cantidadToneladas' ).value
                        };
                    } else {
                        return gestionObra.length > 0 && gestionObra[0].manejoResiduosConstruccionDemolicion !== undefined ? gestionObra[0].manejoResiduosConstruccionDemolicion : residuoConstruccion.value;
                    }
                } else {
                    return gestionObra.length > 0 && gestionObra[0].manejoResiduosConstruccionDemolicion !== undefined ? gestionObra[0].manejoResiduosConstruccionDemolicion : null;
                }
            };
            // GET reactive form "Residuos peligrosos y especiales"
            const manejoResiduosPeligrososEspeciales = async () => {
                const residuosPeligrosos = actividadSeleccionada( this.tipoActividadesCodigo.manejoResiduosPeligrosos ) !== null ? actividadSeleccionada( this.tipoActividadesCodigo.manejoResiduosPeligrosos ).get( 'manejoResiduosPeligrosos' ) : null;

                if ( residuosPeligrosos !== null ) {
                    // obsApoyoResiduosPeligrosos
                    // obsSupervisorResiduosPeligrosos
                    if ( residuosPeligrosos.dirty === true ) {
                        if ( this.obsApoyoResiduosPeligrosos !== undefined ) {
                            this.obsApoyoResiduosPeligrosos.archivada = !this.obsApoyoResiduosPeligrosos.archivada;
                            await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsApoyoResiduosPeligrosos ).toPromise();
                        }
                        if ( this.obsSupervisorResiduosPeligrosos !== undefined ) {
                            this.obsSupervisorResiduosPeligrosos.archivada = !this.obsSupervisorResiduosPeligrosos.archivada;
                            await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsSupervisorResiduosPeligrosos ).toPromise();
                        }

                        return {
                            manejoResiduosPeligrososEspecialesId: residuosPeligrosos.get( 'manejoResiduosPeligrososEspecialesId' ).value,
                            estanClasificados: residuosPeligrosos.get( 'estanClasificados' ).value,
                            requiereObservacion: residuosPeligrosos.get( 'requiereObservacion' ).value,
                            observacion: residuosPeligrosos.get( 'observacion' ).value,
                            urlRegistroFotografico: residuosPeligrosos.get( 'urlRegistroFotografico' ).value,
                        };
                    } else {
                        return gestionObra.length > 0 && gestionObra[0].manejoResiduosPeligrososEspeciales !== undefined ? gestionObra[0].manejoResiduosPeligrososEspeciales : residuosPeligrosos.value;
                    }
                } else {
                    return gestionObra.length > 0 && gestionObra[0].manejoResiduosPeligrososEspeciales !== undefined ? gestionObra[0].manejoResiduosPeligrososEspeciales : null;
                }
            };
            // GET reactive form "Otra"
            const manejoOtro = async () => {
                const otros = actividadSeleccionada( this.tipoActividadesCodigo.otra ) !== null ? actividadSeleccionada( this.tipoActividadesCodigo.otra ).get( 'otra' ) : null;

                if ( otros !== null ) {
                    // obsApoyoManejoOtra
                    // obsSupervisorManejoOtra
                    if ( otros.dirty === true ) {
                        if ( this.obsApoyoManejoOtra !== undefined ) {
                            this.obsApoyoManejoOtra.archivada = !this.obsApoyoManejoOtra.archivada;
                            await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsApoyoManejoOtra ).toPromise();
                        }
                        if ( this.obsSupervisorManejoOtra !== undefined ) {
                            this.obsSupervisorManejoOtra.archivada = !this.obsSupervisorManejoOtra.archivada;
                            await this.verificarAvanceSemanalSvc.seguimientoSemanalObservacion( this.obsSupervisorManejoOtra ).toPromise();
                        }

                        return {
                            manejoOtroId: otros.get( 'manejoOtroId' ).value,
                            fechaActividad: otros.get( 'fechaActividad' ).value,
                            actividad: otros.get( 'actividad' ).value,
                            urlSoporteGestion: otros.get( 'urlSoporteGestion' ).value
                        };
                    } else {
                        return gestionObra.length > 0 && gestionObra[0].manejoOtro !== undefined ? gestionObra[0].manejoOtro : otros.value;
                    }
                } else {
                    return gestionObra.length > 0 && gestionObra[0].manejoOtro !== undefined ? gestionObra[0].manejoOtro : null;
                }
            };
            seguimientoSemanalGestionObra = [
                {
                    seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
                    seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                    seguimientoSemanalGestionObraAmbiental: [
                        {
                            seguimientoSemanalGestionObraAmbientalId: gestionObra.length > 0 ? gestionObra[0].seguimientoSemanalGestionObraAmbientalId : 0,
                            seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                            seEjecutoGestionAmbiental: this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).value,
                            manejoMaterialesInsumo: await manejoMaterialInsumo(),
                            manejoResiduosConstruccionDemolicion: await manejoResiduosConstruccionDemolicion(),
                            manejoResiduosPeligrososEspeciales: await manejoResiduosPeligrososEspeciales(),
                            manejoOtro: await manejoOtro(),
                            tieneManejoMaterialesInsumo: await manejoMaterialInsumo() !== null ? true : false,
                            tieneManejoResiduosPeligrososEspeciales: await manejoResiduosPeligrososEspeciales() !== null ? true : false,
                            tieneManejoResiduosConstruccionDemolicion: await manejoResiduosConstruccionDemolicion() !== null ? true : false,
                            tieneManejoOtro: await manejoOtro() !== null ? true : false
                        }
                    ]
                }
            ];

            if ( pSeguimientoSemanal.seguimientoSemanalGestionObra !== undefined ) {
                if ( pSeguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ) {
                    pSeguimientoSemanal.seguimientoSemanalGestionObra[ 0 ].seguimientoSemanalGestionObraAmbiental = [
                        {
                            seguimientoSemanalGestionObraAmbientalId:   gestionObra.length > 0 ?
                                                                        gestionObra[0].seguimientoSemanalGestionObraAmbientalId : 0,
                            seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                            seEjecutoGestionAmbiental: this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).value,
                            manejoMaterialesInsumo: await manejoMaterialInsumo(),
                            manejoResiduosConstruccionDemolicion: await manejoResiduosConstruccionDemolicion(),
                            manejoResiduosPeligrososEspeciales: await manejoResiduosPeligrososEspeciales(),
                            manejoOtro: await manejoOtro(),
                            tieneManejoMaterialesInsumo: await manejoMaterialInsumo() !== null ? true : false,
                            tieneManejoResiduosPeligrososEspeciales: await manejoResiduosPeligrososEspeciales() !== null ? true : false,
                            tieneManejoResiduosConstruccionDemolicion: await manejoResiduosConstruccionDemolicion() !== null ? true : false,
                            tieneManejoOtro: await manejoOtro() !== null ? true : false
                        }
                    ]
                } else {
                    pSeguimientoSemanal.seguimientoSemanalGestionObra = seguimientoSemanalGestionObra;
                }
            } else {
                pSeguimientoSemanal.seguimientoSemanalGestionObra = seguimientoSemanalGestionObra;
            }
        }

        this.avanceSemanalSvc.saveUpdateSeguimientoSemanal( pSeguimientoSemanal )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () =>   this.routes.navigate(
                                    [
                                        '/registrarAvanceSemanal/registroSeguimientoSemanal', this.seguimientoSemanal.contratacionProyectoId
                                    ]
                                )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

    guardadoParcial() {
        if ( this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).value === false ) {
            const gestionObra = this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ? this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental : [];

            return [
                {
                    seguimientoSemanalGestionObraAmbientalId: gestionObra.length > 0 ? gestionObra[0].seguimientoSemanalGestionObraAmbientalId : 0,
                    seEjecutoGestionAmbiental: this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).value
                }
            ]
        }

        if ( this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).value === true && this.actividades.length > 0 ) {

            // GET metodo actividades seleccionadas en cada acordeon
            const actividadSeleccionada = ( tipoActividad ) => {
                const selectActividad = this.actividades.controls.filter( actividad => actividad.get( 'tipoActividad' ).value !== null && actividad.get( 'tipoActividad' ).value.codigo === tipoActividad );

                return selectActividad.length > 0 ? selectActividad[0] : null;
            };

            // GET gestion de obra ambiental
            const gestionObra =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ? this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental : [];

            // GET reactive form "Manejo de materiales e insumos"
            const manejoMaterialInsumo = () => {
                const manejoMaterial =  actividadSeleccionada( this.tipoActividadesCodigo.manejoMaterialInsumo ) !== null ? actividadSeleccionada( this.tipoActividadesCodigo.manejoMaterialInsumo ) .get( 'manejoMaterialInsumo' ) : null;

                if ( manejoMaterial !== null ) {
                    return {
                        ManejoMaterialesInsumosId: manejoMaterial.get( 'manejoMaterialesInsumosId' ).value,
                        manejoMaterialesInsumosProveedor: manejoMaterial.get( 'proveedores' ).value,
                        estanProtegidosDemarcadosMateriales: manejoMaterial.get( 'estanProtegidosDemarcadosMateriales' ).value,
                        requiereObservacion: manejoMaterial.get( 'requiereObservacion' ).value,
                        observacion: manejoMaterial.get( 'requiereObservacion' ).value === true ? manejoMaterial.get( 'observacion' ).value : null,
                        url:  manejoMaterial.get( 'url' ).value
                    };
                } else {
                    return gestionObra.length > 0 && gestionObra[0].manejoMaterialesInsumo !== undefined ? gestionObra[0].manejoMaterialesInsumo : null;
                }
            };
            // GET reactive form "Residuos de construccion y demolicion"
            const manejoResiduosConstruccionDemolicion = () => {
                const residuoConstruccion = actividadSeleccionada( this.tipoActividadesCodigo.manejoResiduosConstruccion ) !== null ? actividadSeleccionada( this.tipoActividadesCodigo.manejoResiduosConstruccion ).get( 'manejoResiduosConstruccion' ) : null;

                if ( residuoConstruccion !== null ) {
                    return {
                        manejoResiduosConstruccionDemolicionId: residuoConstruccion.get('manejoResiduosConstruccionDemolicionId').value,
                        estaCuantificadoRCD: residuoConstruccion.get( 'estaCuantificadoRCD' ).value,
                        requiereObservacion: residuoConstruccion.get( 'requiereObservacion' ).value,
                        observacion: residuoConstruccion.get( 'observacion' ).value,
                        manejoResiduosConstruccionDemolicionGestor: residuoConstruccion.get( 'manejoResiduosConstruccionDemolicionGestor' ).value,
                        seReutilizadorResiduos: residuoConstruccion.get( 'seReutilizadorResiduos' ).value,
                        cantidadToneladas: residuoConstruccion.get( 'cantidadToneladas' ).value
                    };
                } else {
                    return gestionObra.length > 0 && gestionObra[0].manejoResiduosConstruccionDemolicion !== undefined ? gestionObra[0].manejoResiduosConstruccionDemolicion : null;
                }
            };
            // GET reactive form "Residuos peligrosos y especiales"
            const manejoResiduosPeligrososEspeciales = () => {
                const residuosPeligrosos = actividadSeleccionada( this.tipoActividadesCodigo.manejoResiduosPeligrosos ) !== null ? actividadSeleccionada( this.tipoActividadesCodigo.manejoResiduosPeligrosos ).get( 'manejoResiduosPeligrosos' ) : null;

                if ( residuosPeligrosos !== null ) {
                    return {
                        manejoResiduosPeligrososEspecialesId: residuosPeligrosos.get( 'manejoResiduosPeligrososEspecialesId' ).value,
                        estanClasificados: residuosPeligrosos.get( 'estanClasificados' ).value,
                        requiereObservacion: residuosPeligrosos.get( 'requiereObservacion' ).value,
                        observacion: residuosPeligrosos.get( 'observacion' ).value,
                        urlRegistroFotografico: residuosPeligrosos.get( 'urlRegistroFotografico' ).value,
                    };
                } else {
                    return gestionObra.length > 0 && gestionObra[0].manejoResiduosPeligrososEspeciales !== undefined ? gestionObra[0].manejoResiduosPeligrososEspeciales : null;
                }
            };
            // GET reactive form "Otra"
            const manejoOtro = () => {
                const otros = actividadSeleccionada( this.tipoActividadesCodigo.otra ) !== null ? actividadSeleccionada( this.tipoActividadesCodigo.otra ).get( 'otra' ) : null;
                if ( otros !== null ) {
                    return {
                        manejoOtroId: otros.get( 'manejoOtroId' ).value,
                        fechaActividad: otros.get( 'fechaActividad' ).value,
                        actividad: otros.get( 'actividad' ).value,
                        urlSoporteGestion: otros.get( 'urlSoporteGestion' ).value
                    };
                } else {
                    return gestionObra.length > 0 && gestionObra[0].manejoOtro !== undefined ? gestionObra[0].manejoOtro : null;
                }
            }

            return [
                {
                    seguimientoSemanalGestionObraAmbientalId: gestionObra.length > 0 ? gestionObra[0].seguimientoSemanalGestionObraAmbientalId : 0,
                    seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                    seEjecutoGestionAmbiental: this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).value,
                    manejoMaterialesInsumo: manejoMaterialInsumo(),
                    manejoResiduosConstruccionDemolicion: manejoResiduosConstruccionDemolicion(),
                    manejoResiduosPeligrososEspeciales: manejoResiduosPeligrososEspeciales(),
                    manejoOtro: manejoOtro(),
                    tieneManejoMaterialesInsumo: manejoMaterialInsumo() !== null ? true : false,
                    tieneManejoResiduosPeligrososEspeciales: manejoResiduosPeligrososEspeciales() !== null ? true : false,
                    tieneManejoResiduosConstruccionDemolicion: manejoResiduosConstruccionDemolicion() !== null ? true : false,
                    tieneManejoOtro: manejoOtro() !== null ? true : false
                }
            ]
        }

    }

}
