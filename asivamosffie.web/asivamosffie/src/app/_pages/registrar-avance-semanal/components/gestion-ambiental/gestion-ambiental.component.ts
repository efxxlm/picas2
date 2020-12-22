import { delay } from 'rxjs/operators';
import { Router } from '@angular/router';
import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';

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
    tipoActividades: Dominio[] = [];
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;
    gestionAmbiental: boolean;
    gestionObraAmbiental: any;
    cantidadActividades = 0;
    gestionAmbientalDetalle: any[] = [];
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

    get actividades() {
        return this.formGestionAmbiental.get( 'actividades' ) as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private dialog: MatDialog,
        private routes: Router,
        private avanceSemanalSvc: RegistrarAvanceSemanalService )
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
                        this.formGestionAmbiental.get( 'cantidadActividad' ).disable();
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
                    // GET manejo de materiales e insumos
                    if ( this.gestionObraAmbiental.tieneManejoMaterialesInsumo === true ) {
                        const actividadSeleccionada = this.tipoActividades.filter(
                            actividad => actividad.codigo === this.tipoActividadesCodigo.manejoMaterialInsumo
                        );
                        this.actividades.push(
                            this.fb.group({
                                tipoActividad: [ actividadSeleccionada[0] ],
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
                        const actividadSeleccionada = this.tipoActividades.filter(
                            actividad => actividad.codigo === this.tipoActividadesCodigo.manejoResiduosConstruccion
                        );
                        this.actividades.push(
                            this.fb.group({
                                tipoActividad: [ actividadSeleccionada[0] ],
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
                        const actividadSeleccionada = this.tipoActividades.filter(
                            actividad => actividad.codigo === this.tipoActividadesCodigo.manejoResiduosPeligrosos
                        );
                        this.actividades.push(
                            this.fb.group({
                                tipoActividad: [ actividadSeleccionada[0] ],
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
                        const actividadSeleccionada = this.tipoActividades.filter(
                            actividad => actividad.codigo === this.tipoActividadesCodigo.otra
                        );
                        this.actividades.push(
                            this.fb.group({
                                tipoActividad: [ actividadSeleccionada[0] ],
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

    guardar() {

        const pSeguimientoSemanal = this.seguimientoSemanal;
        let seguimientoSemanalGestionObra;
        if ( this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).value === false ) {
            const gestionObra =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
            this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental
            : [];
            seguimientoSemanalGestionObra = [
                {
                    seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
                    seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                    seguimientoSemanalGestionObraAmbiental: [
                        {
                            seguimientoSemanalGestionObraAmbientalId:   gestionObra.length > 0 ?
                                                                        gestionObra[0].seguimientoSemanalGestionObraAmbientalId : 0,
                            seEjecutoGestionAmbiental: this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).value
                        }
                    ]
                }
            ];
        }
        if ( this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).value === true && this.actividades.length > 0 ) {

            // GET metodo actividades seleccionadas en cada acordeon
            const actividadSeleccionada = ( tipoActividad ) => {
                const selectActividad = this.actividades.controls.filter(
                    actividad =>    actividad.get( 'tipoActividad' ).value !== null
                                    && actividad.get( 'tipoActividad' ).value.codigo === tipoActividad
                );
                return selectActividad.length > 0 ? selectActividad[0] : null;
            };

            // GET gestion de obra ambiental
            const gestionObra =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
                                    this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental
                                    : [];

            // GET reactive form "Manejo de materiales e insumos"
            const manejoMaterialInsumo = () => {

                const manejoMaterial =  actividadSeleccionada( this.tipoActividadesCodigo.manejoMaterialInsumo ) !== null ?
                                        actividadSeleccionada( this.tipoActividadesCodigo.manejoMaterialInsumo )
                                        .get( 'manejoMaterialInsumo' )
                                        : null;

                if ( manejoMaterial !== null ) {
                    if ( manejoMaterial.dirty === true ) {
                        return {
                            ManejoMaterialesInsumosId: manejoMaterial.get( 'manejoMaterialesInsumosId' ).value,
                            manejoMaterialesInsumosProveedor: manejoMaterial.get( 'proveedores' ).value,
                            estanProtegidosDemarcadosMateriales: manejoMaterial.get( 'estanProtegidosDemarcadosMateriales' ).value,
                            requiereObservacion: manejoMaterial.get( 'requiereObservacion' ).value,
                            observacion:    manejoMaterial.get( 'requiereObservacion' ).value === true
                                            ?  manejoMaterial.get( 'observacion' ).value : null,
                            url:  manejoMaterial.get( 'url' ).value
                        };
                    }   else {
                        return  gestionObra.length > 0
                                && gestionObra[0].manejoMaterialesInsumo !== undefined
                                ? gestionObra[0].manejoMaterialesInsumo : manejoMaterial.value;
                    }
                } else {
                    return  gestionObra.length > 0
                            && gestionObra[0].manejoMaterialesInsumo !== undefined ? gestionObra[0].manejoMaterialesInsumo : null;
                }
            };
            // GET reactive form "Residuos de construccion y demolicion"
            const manejoResiduosConstruccionDemolicion = () => {

                const residuoConstruccion =
                    actividadSeleccionada( this.tipoActividadesCodigo.manejoResiduosConstruccion ) !== null ?
                    actividadSeleccionada( this.tipoActividadesCodigo.manejoResiduosConstruccion )
                    .get( 'manejoResiduosConstruccion' )
                    : null;

                if ( residuoConstruccion !== null ) {
                    if ( residuoConstruccion.dirty === true ) {
                        console.log( 'condicion 1' );
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
                        return  gestionObra.length > 0 && gestionObra[0].manejoResiduosConstruccionDemolicion !== undefined
                        ? gestionObra[0].manejoResiduosConstruccionDemolicion : residuoConstruccion.value;
                    }
                } else {
                    return  gestionObra.length > 0
                            && gestionObra[0].manejoResiduosConstruccionDemolicion !== undefined
                    ? gestionObra[0].manejoResiduosConstruccionDemolicion : null;
                }
            };
            // GET reactive form "Residuos peligrosos y especiales"
            const manejoResiduosPeligrososEspeciales = () => {
                const residuosPeligrosos =  actividadSeleccionada( this.tipoActividadesCodigo.manejoResiduosPeligrosos ) !== null ?
                                            actividadSeleccionada( this.tipoActividadesCodigo.manejoResiduosPeligrosos )
                                            .get( 'manejoResiduosPeligrosos' )
                                            : null;
                if ( residuosPeligrosos !== null ) {
                    if ( residuosPeligrosos.dirty === true ) {
                        return {
                            manejoResiduosPeligrososEspecialesId: residuosPeligrosos.get( 'manejoResiduosPeligrososEspecialesId' ).value,
                            estanClasificados: residuosPeligrosos.get( 'estanClasificados' ).value,
                            requiereObservacion: residuosPeligrosos.get( 'requiereObservacion' ).value,
                            observacion: residuosPeligrosos.get( 'observacion' ).value,
                            urlRegistroFotografico: residuosPeligrosos.get( 'urlRegistroFotografico' ).value,
                        };
                    } else {
                        return  gestionObra.length > 0 && gestionObra[0].manejoResiduosPeligrososEspeciales !== undefined
                        ? gestionObra[0].manejoResiduosPeligrososEspeciales : residuosPeligrosos.value;
                    }
                } else {
                    return  gestionObra.length > 0 && gestionObra[0].manejoResiduosPeligrososEspeciales !== undefined
                            ? gestionObra[0].manejoResiduosPeligrososEspeciales : null;
                }
            };
            // GET reactive form "Otra"
            const manejoOtro = () => {
                const otros =   actividadSeleccionada( this.tipoActividadesCodigo.otra ) !== null ?
                                actividadSeleccionada( this.tipoActividadesCodigo.otra )
                                .get( 'otra' )
                                : null;
                if ( otros !== null ) {
                    if ( otros.dirty === true ) {
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
                            seguimientoSemanalGestionObraAmbientalId:   gestionObra.length > 0 ?
                                                                        gestionObra[0].seguimientoSemanalGestionObraAmbientalId : 0,
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
            ];
        }
        pSeguimientoSemanal.seguimientoSemanalGestionObra = seguimientoSemanalGestionObra;
        console.log( pSeguimientoSemanal );
        this.avanceSemanalSvc.saveUpdateSeguimientoSemanal( pSeguimientoSemanal )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.seRealizoPeticion.emit( true );
                    console.log( this.routes.url );
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

}
