import { Router } from '@angular/router';
import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FormGroup, FormBuilder } from '@angular/forms';
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

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private dialog: MatDialog,
        private routes: Router,
        private avanceSemanalSvc: RegistrarAvanceSemanalService )
    {
        this.crearFormulario();
        this.commonSvc.listaTipoActividades()
            .subscribe(
                actividades => {
                    this.tipoActividades = actividades;
                }
            );
    }

    ngOnInit(): void {
        if ( this.seguimientoSemanal !== undefined ) {
            this.seguimientoSemanalId = this.seguimientoSemanal.seguimientoSemanalId;
            this.seguimientoSemanalGestionObraId =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
                this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraId : 0;
        }
        if (    this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0
                && this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental.length > 0 )
        {
            const gestionObraAmbiental = this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental[0];
            if ( gestionObraAmbiental.seEjecutoGestionAmbiental !== undefined ) {
                this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).setValue( gestionObraAmbiental.seEjecutoGestionAmbiental );
                this.formGestionAmbiental.get( 'seEjecutoGestionAmbiental' ).markAsDirty();
                this.gestionAmbiental = true;
            } else {
                this.gestionAmbiental = false;
            }
        }
    }

    crearFormulario() {
        this.formGestionAmbiental = this.fb.group({
            seEjecutoGestionAmbiental: [ null ],
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
        });
    }

    valuePending( value: any ) {
        console.log( value );
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        // console.log( this.formGestionAmbiental.value );

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
        if ( this.formGestionAmbiental.get( 'tipoActividad' ).value !== null ) {
            const gestionObra =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
                                    this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental
                                    : [];
            const manejoMaterialInsumo = () => {
                if ( this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).dirty === true ) {
                    return {
                        ManejoMaterialesInsumosId:
                            this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'manejoMaterialesInsumosId' ).value,
                        manejoMaterialesInsumosProveedor:
                            this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'proveedores' ).value,
                        estanProtegidosDemarcadosMateriales: this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'estanProtegidosDemarcadosMateriales' ).value,
                        requiereObservacion:
                            this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'requiereObservacion' ).value,
                        observacion:
                            this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'requiereObservacion' ).value === true
                            ? this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'observacion' ).value : null,
                        url: this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'url' ).value
                    };
                } else {
                    return gestionObra[0].manejoMaterialesInsumo !== undefined ? gestionObra[0].manejoMaterialesInsumo : null;
                }
            };
            const manejoResiduosConstruccionDemolicion = () => {
                if ( this.formGestionAmbiental.get( 'manejoResiduosConstruccion' ).dirty === true ) {
                    return {
                        manejoResiduosConstruccionDemolicionId: this.formGestionAmbiental.get( 'manejoResiduosConstruccion' ).get( 'manejoResiduosConstruccionDemolicionId' ).value,
                        estaCuantificadoRCD:
                            this.formGestionAmbiental.get( 'manejoResiduosConstruccion' ).get( 'estaCuantificadoRCD' ).value,
                        requiereObservacion:
                            this.formGestionAmbiental.get( 'manejoResiduosConstruccion' ).get( 'requiereObservacion' ).value,
                        observacion: this.formGestionAmbiental.get( 'manejoResiduosConstruccion' ).get( 'observacion' ).value,
                        manejoResiduosConstruccionDemolicionGestor: this.formGestionAmbiental.get( 'manejoResiduosConstruccion' ).get( 'manejoResiduosConstruccionDemolicionGestor' ).value,
                        seReutilizadorResiduos:
                            this.formGestionAmbiental.get( 'manejoResiduosConstruccion' ).get( 'seReutilizadorResiduos' ).value,
                        cantidadToneladas: this.formGestionAmbiental.get( 'manejoResiduosConstruccion' ).get( 'cantidadToneladas' ).value
                    };
                } else {
                    return  gestionObra[0].manejoResiduosConstruccionDemolicion !== undefined
                            ? gestionObra[0].manejoResiduosConstruccionDemolicion : null;
                }
            };
            const manejoResiduosPeligrososEspeciales = () => {
                if ( this.formGestionAmbiental.get( 'manejoResiduosPeligrosos' ).dirty === true ) {
                    return {
                        manejoResiduosPeligrososEspecialesId: this.formGestionAmbiental.get( 'manejoResiduosPeligrosos' ).get( 'manejoResiduosPeligrososEspecialesId' ).value,
                        estanClasificados: this.formGestionAmbiental.get( 'manejoResiduosPeligrosos' ).get( 'estanClasificados' ).value,
                        requiereObservacion: this.formGestionAmbiental.get( 'manejoResiduosPeligrosos' ).get( 'requiereObservacion' ).value,
                        observacion: this.formGestionAmbiental.get( 'manejoResiduosPeligrosos' ).get( 'observacion' ).value,
                        urlRegistroFotografico:
                            this.formGestionAmbiental.get( 'manejoResiduosPeligrosos' ).get( 'urlRegistroFotografico' ).value,
                    };
                } else {
                    return  gestionObra[0].manejoResiduosPeligrososEspeciales !== undefined
                            ? gestionObra[0].manejoResiduosPeligrososEspeciales : null;
                }
            };
            const manejoOtro = () => {
                if ( this.formGestionAmbiental.get( 'otra' ).dirty === true ) {
                    return {
                        manejoOtroId: this.formGestionAmbiental.get( 'otra' ).get( 'manejoOtroId' ).value,
                        fechaActividad: this.formGestionAmbiental.get( 'otra' ).get( 'fechaActividad' ).value,
                        actividad: this.formGestionAmbiental.get( 'otra' ).get( 'actividad' ).value,
                        urlSoporteGestion: this.formGestionAmbiental.get( 'otra' ).get( 'urlSoporteGestion' ).value
                    };
                } else {
                    return gestionObra[0].manejoOtro !== undefined ? gestionObra[0].manejoOtro : null;
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
                            manejoOtro: manejoOtro()
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
