import { ActualizarPolizasService } from './../../../../core/_services/actualizarPolizas/actualizar-polizas.service';
import { MatDialog } from '@angular/material/dialog';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import humanize from 'humanize-plus';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { TipoActualizacion, TipoActualizacionCodigo } from 'src/app/_interfaces/estados-actualizacion-polizas.interface';

@Component({
  selector: 'app-razon-tipo-actualizacion-rapg',
  templateUrl: './razon-tipo-actualizacion-rapg.component.html',
  styleUrls: ['./razon-tipo-actualizacion-rapg.component.scss']
})
export class RazonTipoActualizacionRapgComponent implements OnInit {

    @Input() contratoPoliza: any;
    @Input() esVerDetalle: boolean;
    contratoPolizaActualizacion: any;
    contratoPolizaActualizacionSeguro: any;
    listaTipoActualizacion: TipoActualizacion = TipoActualizacionCodigo;
    addressForm = this.fb.group({
        contratoPolizaActualizacionId: [ 0 ],
        razonActualizacion: [null, Validators.required],
        fechaExpedicion: [null, Validators.required],
        polizasYSeguros: [null, Validators.required],
        seguros: this.fb.array( [] )
    });
    listaGarantias: Dominio[] = [];
    razonActualizacionArray : Dominio[] = []
    polizasYSegurosArray: Dominio[] = [];
    tipoActualizacionArray: Dominio[] = [];
    estaEditando = false;
    minDate = new Date();

    get seguros() {
        return this.addressForm.get( 'seguros' ) as FormArray;
    }

    constructor(
        private common: CommonService,
        private fb: FormBuilder,
        private routes: Router,
        private dialog: MatDialog,
        private actualizarPolizaSvc: ActualizarPolizasService )
    {
    }

    ngOnInit(): void {
        this.getRazonTipoActualizacion();
    }

    async getRazonTipoActualizacion() {
        this.polizasYSegurosArray = await this.common.listaGarantiasPolizas().toPromise();
        this.razonActualizacionArray = await this.common.listaRazonActualizacion().toPromise();
        this.tipoActualizacionArray = await this.common.listaTipoActualizacion().toPromise();

        this.polizasYSegurosArray.forEach( ( poliza, index ) => {
            const garantia = this.contratoPoliza.polizaGarantia.find( garantia => garantia.tipoGarantiaCodigo === poliza.codigo );

            if ( garantia !== undefined ) {
                this.listaGarantias.push( poliza )
            }
        } )

        if ( this.contratoPoliza.contratoPolizaActualizacion !== undefined ) {
            if ( this.contratoPoliza.contratoPolizaActualizacion.length > 0 ) {
                this.contratoPolizaActualizacion = this.contratoPoliza.contratoPolizaActualizacion[ 0 ];
                this.addressForm.get( 'contratoPolizaActualizacionId' ).setValue( this.contratoPolizaActualizacion.contratoPolizaActualizacionId );
                this.addressForm.get( 'razonActualizacion' ).setValue( this.contratoPolizaActualizacion.razonActualizacionCodigo !== undefined ? this.razonActualizacionArray.find( razon => razon.codigo === this.contratoPolizaActualizacion.razonActualizacionCodigo ).codigo : null );
                this.addressForm.get( 'fechaExpedicion' ).setValue( this.contratoPolizaActualizacion.fechaExpedicionActualizacionPoliza !== undefined ? new Date( this.contratoPolizaActualizacion.fechaExpedicionActualizacionPoliza ) : null )

                if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro !== undefined ) {
                    if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro.length > 0 ) {
                        this.contratoPolizaActualizacionSeguro = this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro;
                        const segurosSeleccionados = [];

                        for ( const seguro of this.contratoPolizaActualizacionSeguro ) {
                            const poliza = this.polizasYSegurosArray.find( poliza => poliza.codigo === seguro.tipoSeguroCodigo );
                            const actualizacionSeleccionada = [];

                            if ( seguro.tieneFechaSeguro === true ) {
                                actualizacionSeleccionada.push( this.listaTipoActualizacion.seguros );
                            }
                            if ( seguro.tieneFechaVigenciaAmparo === true ) {
                                actualizacionSeleccionada.push( this.listaTipoActualizacion.fecha );
                            }
                            if ( seguro.tieneValorAmparo === true ) {
                                actualizacionSeleccionada.push( this.listaTipoActualizacion.valor );
                            }

                            segurosSeleccionados.push( seguro.tipoSeguroCodigo );

                            if ( poliza !== undefined ) {
                                this.seguros.push( this.fb.group(
                                    {
                                        contratoPolizaActualizacionSeguroId: [ seguro.contratoPolizaActualizacionSeguroId ],
                                        nombre: [ poliza.nombre ],
                                        codigo: [ poliza.codigo ],
                                        tipoActualizacion: [ actualizacionSeleccionada, Validators.required ]
                                    }
                                ) )
                            }
                        }

                        this.addressForm.get( 'polizasYSeguros' ).setValue( segurosSeleccionados );
                    }
                }
            }
        }
    }
  
    getvalues( codigoSeguro: string ) {
        const listaSeguros = [ ...codigoSeguro ];

        if ( codigoSeguro.length === 0 ) {
            this.seguros.clear();
            return;
        }

        if ( this.seguros.length > 0 ) {
            this.seguros.controls.forEach( ( control, indexControl ) => {
                const seguroIndex = listaSeguros.findIndex( codigo => codigo === control.get( 'codigo' ).value );
                const seguro = listaSeguros.find( codigo => codigo === control.get( 'codigo' ).value );

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
            const seguro = this.polizasYSegurosArray.find( poliza => poliza.codigo === codigo );

            if ( seguro !== undefined ) {
                this.seguros.push( this.fb.group(
                    {
                        contratoPolizaActualizacionSeguroId: [ 0 ],
                        nombre: [ seguro.nombre ],
                        codigo: [ seguro.codigo ],
                        tipoActualizacion: [ null, Validators.required ]
                    }
                ) )
            }
        }
    }
    // evalua tecla a tecla
    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);
        return alphanumeric.test(inputChar) ? true : false;
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }

    getRazonActualizacion( codigo: string ) {
        if ( this.razonActualizacionArray.length > 0 ) {
            const razon = this.razonActualizacionArray.find( razon => razon.codigo === codigo );

            if ( razon !== undefined ) {
                return razon.nombre;
            }
        }
    }

    getSeguros( codigo: string ) {
        if ( this.polizasYSegurosArray.length > 0 ) {
            const seguro = this.polizasYSegurosArray.find( seguro => seguro.codigo === codigo );

            if ( seguro !== undefined ) {
                return seguro.nombre;
            }
        }
    }

    getTipoActualizacion( codigo: string ) {
        if ( this.tipoActualizacionArray.length > 0 ) {
            const tipo = this.tipoActualizacionArray.find( tipo => tipo.codigo === codigo );

            if ( tipo !== undefined ) {
                return tipo.nombre;
            }
        }
    }

    deleteSeguro( index: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        const listSeguro: string[] = this.addressForm.get( 'polizasYSeguros' ).value;
                        const indexSeguro =listSeguro.findIndex( codigo => codigo === this.seguros.controls[ index ].get( 'codigo' ).value );
                        listSeguro.splice( indexSeguro, 1 );

                        if ( this.seguros.controls[ index ].get( 'contratoPolizaActualizacionSeguroId' ).value !== 0 ) {
                            const pContratoPolizaActualizacionSeguro = {
                                contratoPolizaActualizacionSeguroId: this.seguros.controls[ index ].get( 'contratoPolizaActualizacionSeguroId' ).value
                            };

                            this.actualizarPolizaSvc.deleteContratoPolizaActualizacionSeguro( pContratoPolizaActualizacionSeguro )
                                .subscribe( );
                        }
                        this.addressForm.get( 'polizasYSeguros' ).setValue( listSeguro );
                        this.seguros.removeAt( index );
                        this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                    }
                }
            )
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    openDialogTrueFalse(modalTitle: string, modalText: string) {

        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText, siNoBoton: true }
        });

        return dialogRef.afterClosed();
    }

    onSubmit() {
        this.estaEditando = true;

        const contratoPolizaActualizacionSeguro = () => {
            const listSeguro = [];

            this.seguros.controls.forEach( control => {
                const listaTipoActualizacionCodigo: string[] = control.get( 'tipoActualizacion' ).value;

                if ( listaTipoActualizacionCodigo.length > 0 ) {
                    const tieneFechaSeguro = listaTipoActualizacionCodigo.find( codigo => codigo === this.listaTipoActualizacion.seguros );
                    const tieneFechaVigenciaAmparo = listaTipoActualizacionCodigo.find( codigo => codigo === this.listaTipoActualizacion.fecha );
                    const tieneValorAmparo = listaTipoActualizacionCodigo.find( codigo => codigo === this.listaTipoActualizacion.valor );

                    listSeguro.push(
                        {
                            contratoPolizaActualizacionSeguroId: control.get( 'contratoPolizaActualizacionSeguroId' ).value,
                            contratoPolizaActualizacionId: this.addressForm.get( 'contratoPolizaActualizacionId' ).value,
                            tipoSeguroCodigo: control.get( 'codigo' ).value,
                            tieneFechaSeguro: tieneFechaSeguro !== undefined ? true : false,
                            tieneFechaVigenciaAmparo: tieneFechaVigenciaAmparo !== undefined ? true : false,
                            tieneValorAmparo: tieneValorAmparo !== undefined ? true : false,
                        }
                    )
                }
            } );

            return listSeguro;
        }

        const pContratoPolizaActualizacion = {
            contratoPolizaActualizacionId: this.addressForm.get( 'contratoPolizaActualizacionId' ).value,
            contratoPolizaId: this.contratoPoliza.contratoPolizaId,
            razonActualizacionCodigo: this.addressForm.get( 'razonActualizacion' ).value,
            fechaExpedicionActualizacionPoliza: this.addressForm.get( 'fechaExpedicion' ).value !== null ? new Date( this.addressForm.get( 'fechaExpedicion' ).value ).toISOString() : null,
            contratoPolizaActualizacionSeguro: contratoPolizaActualizacionSeguro()
        }

        this.actualizarPolizaSvc.createorUpdateCofinancing( pContratoPolizaActualizacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/registrarActualizacionesPolizasYGarantias/verDetalleEditarPoliza', this.contratoPoliza.contratoPolizaId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
