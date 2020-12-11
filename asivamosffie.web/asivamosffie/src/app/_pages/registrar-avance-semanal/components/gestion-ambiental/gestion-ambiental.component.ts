import { RegistrarAvanceSemanalService } from './../../../../core/_services/registrarAvanceSemanal/registrar-avance-semanal.service';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
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
    }

    crearFormulario() {
        this.formGestionAmbiental = this.fb.group({
            actividadRelacionada: [ null ],
            tipoActividad: [ null ],
            manejoMaterialInsumo: this.fb.group({
                proveedores: this.fb.array( [
                    this.fb.group({
                        proveedor: [ '' ],
                        requierePermisosAmbientalesMineros: [ null ],
                        urlRegistroFotografico: [ '' ]
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
        const seguimientoSemanalGestionObra = [
            {
                segumientoSemanalId: this.seguimientoSemanal.seguimientoSemanalId,
                seguimientoSemanalGestionObraId: 0,
                seguimientoSemanalGestionObraAmbiental: [
                    {
                        seguimientoSemanalGestionObraAmbientalId: 0,
                        seEjecutoGestionAmbiental: this.formGestionAmbiental.get( 'actividadRelacionada' ).value,
                        manejoMaterialesInsumo:
                            {
                                ManejoMaterialesInsumosId: 0,
                                manejoMaterialesInsumosProveedor:
                                    this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'proveedores' ).value,
                                estanProtegidosDemarcadosMateriales: this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'estanProtegidosDemarcadosMateriales' ).value,
                                requiereObservacion:
                                    this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'requiereObservacion' ).value,
                                observacion: this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'observacion' ).value,
                                url: this.formGestionAmbiental.get( 'manejoMaterialInsumo' ).get( 'url' ).value
                            }
                    }
                ]
            }
        ];
        pSeguimientoSemanal.seguimientoSemanalGestionObra = seguimientoSemanalGestionObra;
        console.log( pSeguimientoSemanal );
        this.avanceSemanalSvc.saveUpdateSeguimientoSemanal( pSeguimientoSemanal )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                },
                err => {
                    this.openDialog( '', `<b>${ err.message }</b>` );
                    console.log( err );
                }
            );
    }

}
