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
    tipoActividades: Dominio[] = [];
    seguimientoSemanalId: number;
    seguimientoSemanalGestionObraId: number;

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private dialog: MatDialog,
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
    }

    crearFormulario() {
        this.formGestionAmbiental = this.fb.group({
            actividadRelacionada: [ null ],
            tipoActividad: [ null ],
            manejoMaterialInsumo: this.fb.group({
                manejoMaterialesInsumosId: [ 0 ],
                proveedores: this.fb.array( [
                    this.fb.group({
                        proveedor: [ '' ],
                        requierePermisosAmbientalesMineros: [ null ],
                        urlRegistroFotografico: [ '' ],
                        manejoMaterialesInsumosProveedorId: [ 0 ]
                    })
                ] ),
                estanProtegidosDemarcadosMateriales: [ null ],
                requiereObservacion: [ null ],
                observacion: [ null ],
                url: [ null ]
            }),
            manejoResiduosConstruccion: this.fb.group({
                esRcdCuantificado: [ null ],
                requiereObservacionSobreManejo: [ null ],
                observacionManejoResiduos: [ null ],
                gestorResiduos: this.fb.array( [
                    this.fb.group(
                        {
                            gestorResiduosConstruccion: [ '' ],
                            presentaPermisoAmbientalValido: [ null ],
                            urlSoporte: [ '' ]
                        }
                    )
                ] ),
                seReutilizaronLosResiduos: [ null ],
                cantidad: [ '' ]
            }),
            manejoResiduosPeligrosos: this.fb.group({
                seClasificaronlugarSeguro: [ null ],
                requiereObservacion: [ null ],
                obsManejoResiduosPeligrosos: [ null ],
                urlSoporte: [ '' ]
            }),
            otra: this.fb.group({
                fechaActividad: [ null ],
                observacionActividad: [ null ],
                urlSoporte: [ '' ]
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
        if ( this.formGestionAmbiental.get( 'tipoActividad' ).value.codigo === this.tipoActividadesCodigo.manejoMaterialInsumo ) {
            const manejoMaterial =  this.seguimientoSemanal.seguimientoSemanalGestionObra.length > 0 ?
                                    this.seguimientoSemanal.seguimientoSemanalGestionObra[0].seguimientoSemanalGestionObraAmbiental
                                    : [];
            seguimientoSemanalGestionObra = [
                {
                    seguimientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
                    seguimientoSemanalGestionObraId: this.seguimientoSemanalGestionObraId,
                    seguimientoSemanalGestionObraAmbiental: [
                        {
                            seguimientoSemanalGestionObraAmbientalId:   manejoMaterial.length > 0 ?
                                                                        manejoMaterial[0].seguimientoSemanalGestionObraAmbientalId : 0,
                            seEjecutoGestionAmbiental: this.formGestionAmbiental.get( 'actividadRelacionada' ).value,
                            manejoMaterialesInsumo:
                                {
                                    ManejoMaterialesInsumosId: this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'manejoMaterialesInsumosId' ).value,
                                    manejoMaterialesInsumosProveedor:
                                        this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'proveedores' ).value,
                                    estanProtegidosDemarcadosMateriales: this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'estanProtegidosDemarcadosMateriales' ).value,
                                    requiereObservacion:
                                        this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'requiereObservacion' ).value,
                                    observacion:
                                        this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'requiereObservacion' ).value === true
                                        ? this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'observacion' ).value : null,
                                    url: this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'url' ).value
                                }
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
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
