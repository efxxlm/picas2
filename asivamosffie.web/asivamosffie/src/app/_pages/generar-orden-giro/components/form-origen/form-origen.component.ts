import { Router } from '@angular/router';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { Dominio } from './../../../../core/_services/common/common.service';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { OrdenPagoService } from './../../../../core/_services/ordenPago/orden-pago.service';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import humanize from 'humanize-plus';

@Component({
  selector: 'app-form-origen',
  templateUrl: './form-origen.component.html',
  styleUrls: ['./form-origen.component.scss']
})
export class FormOrigenComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Output() seDiligenciaFormulario = new EventEmitter<boolean>();
    @Output() estadoSemaforo = new EventEmitter<string>();
    ordenGiroDetalle: any;
    ordenGiroDetalleTerceroCausacion: any[];
    ordenGiroId = 0;
    ordenGiroDetalleId = 0;
    esUnicaCuenta: boolean;
    formOrigen: FormGroup;
    listaAportantes = [];
    listaFuenteTipoFinanciacion: Dominio[] = [];
    listaTipoAportante: Dominio[] = [];
    bancosArray: Dominio[] = [];
    listaNombreAportante: { tipoAportanteId: number; cofinanciacionAportanteId: number; nombreAportante: string; }[] = [];
    listaNombreCuenta: Dominio[] = [ { codigo: '1', nombre: 'Alcaldía de Susacón' } ];

    get aportantes() {
        return this.formOrigen.get( 'aportantes' ) as FormArray;
    }

    constructor(
        private ordenGiroSvc: OrdenPagoService,
        private commonSvc: CommonService,
        private dialog: MatDialog,
        private fb: FormBuilder,
        private routes: Router )
    {
        this.commonSvc.listaBancos()
            .subscribe( listaBancos => this.bancosArray = listaBancos );
        this.commonSvc.listaFuenteTipoFinanciacion()
            .subscribe( listaFuenteTipoFinanciacion => this.listaFuenteTipoFinanciacion = listaFuenteTipoFinanciacion );
        this.formOrigen = this.crearFormulario();
    }

    ngOnInit(): void {
        this.getOrigen();
    }

    async getOrigen() {
        const dataAportantes = await this.ordenGiroSvc.getAportantesNew( this.solicitudPago );

        // Get IDs
        if ( this.solicitudPago.ordenGiro !== undefined ) {
            this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;

            if ( this.solicitudPago.ordenGiro.ordenGiroDetalle !== undefined ) {
                if ( this.solicitudPago.ordenGiro.ordenGiroDetalle.length > 0 ) {
                    this.ordenGiroDetalle = this.solicitudPago.ordenGiro.ordenGiroDetalle[0];
                    this.ordenGiroDetalleId = this.ordenGiroDetalle.ordenGiroDetalleId;

                    if ( this.ordenGiroDetalle.ordenGiroDetalleTerceroCausacion !== undefined ) {
                        if ( this.ordenGiroDetalle.ordenGiroDetalleTerceroCausacion.length > 0 ) {
                            this.ordenGiroDetalleTerceroCausacion = this.ordenGiroDetalle.ordenGiroDetalleTerceroCausacion;
                            let totalCuenta = 0;
                            let totalEnProceso = 0;
                            let totalCompleto = 0;
                            let totalInCompleto = 0;

                            this.ordenGiroDetalleTerceroCausacion.forEach( terceroCausacion => {
                                if ( terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.length > 0 ) {
                                    terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.forEach( aportante => {
                                        aportante.registroCompleto = aportante.cuentaBancariaId > 0 ? true : false;
                                        const aportanteFind = this.listaAportantes.find( value => value.aportanteId === aportante.aportanteId )
                                        if ( aportanteFind === undefined ) {
                                            this.listaAportantes.push( aportante );
                                        }
                                    } );
                                }
                            } );

                            for ( const aportante of this.listaAportantes ) {
                                const nombreAportante = dataAportantes.listaNombreAportante.find( nombreAportante => nombreAportante.cofinanciacionAportanteId === aportante.aportanteId );
                                const tipoAportante = dataAportantes.listaTipoAportante.find( tipoAportante => tipoAportante.dominioId === nombreAportante.tipoAportanteId );
                                const fuente = await this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante.cofinanciacionAportanteId ).toPromise();
                                const fuenteRecurso = fuente.find( fuenteValue => fuenteValue.codigo === aportante.fuenteRecursoCodigo );
                                const cuentaBancaria = ( ) => {
                                    if ( aportante.fuenteFinanciacion.cuentaBancaria.length > 1 ) {
                                        if ( aportante.cuentaBancariaId !== undefined ) {
                                            const cuenta = aportante.fuenteFinanciacion.cuentaBancaria.find( cuenta => cuenta.cuentaBancariaId === aportante.cuentaBancariaId );

                                            if ( cuenta !== undefined ) {
                                                return cuenta;
                                            } else {
                                                return null;
                                            }
                                        } else {
                                            return null;
                                        }
                                    } else {
                                        return null;
                                    }
                                }

                                if ( aportante.fuenteFinanciacion.cuentaBancaria.length === 1 ) {
                                    totalCuenta++;
                                }
                                if(aportante.registroCompleto == true){
                                  totalCompleto++;
                                }else if(aportante.registroCompleto == false){
                                  totalInCompleto++;
                                }

                                this.aportantes.push(
                                    this.fb.group(
                                        {
                                            tipoAportante: [ tipoAportante ],
                                            nombreAportante: [ nombreAportante ],
                                            fuente: [ fuenteRecurso ],
                                            listaCuentaBancaria: [ aportante.fuenteFinanciacion.cuentaBancaria ],
                                            cuentaBancariaId: [ cuentaBancaria(), Validators.required ]
                                        }
                                    )
                                );
                            }

                            if ( totalCompleto > 0 && totalCompleto === this.listaAportantes.length ) {
                                this.estadoSemaforo.emit( 'completo' );
                            }else if ( totalInCompleto > 0 && totalInCompleto === this.listaAportantes.length ) {
                              this.estadoSemaforo.emit( 'sin-diligenciar' );
                            }else{
                              this.estadoSemaforo.emit( 'en-proceso' );
                            }
                            if ( totalCuenta === this.listaAportantes.length ) {
                                this.esUnicaCuenta = true;
                                this.seDiligenciaFormulario.emit( false );
                            }
                        }
                    }
                }
            }
        }
    }

    crearFormulario() {
        return this.fb.group(
            {
                aportantes: this.fb.array( [] )
            }
        )
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    getBanco( codigo: string ) {
        if ( this.bancosArray.length > 0 ) {
            const banco = this.bancosArray.find( banco => banco.codigo === codigo );

            if ( banco !== undefined ) {
                return banco.nombre;
            }
        }
    }

    checkSaveBtn() {
        let totalCuentasBancarias = 0;

        if ( this.aportantes.length > 0 ) {
            this.aportantes.controls.forEach( control => {
                totalCuentasBancarias += control.get( 'listaCuentaBancaria' ).value.lengthM
            } )

            if ( totalCuentasBancarias !== this.aportantes.length ) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    guardar() {
        this.aportantes.controls.forEach( control => {
            this.ordenGiroDetalleTerceroCausacion.forEach( terceroCausacion => {
                terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.forEach( aportante => {
                    if ( control.get( 'nombreAportante' ).value.cofinanciacionAportanteId === aportante.aportanteId ) {
                      aportante.cuentaBancariaId = control.get( 'cuentaBancariaId' )?.value?.cuentaBancariaId;
                    }
                } )
            } )
        } )


        const pOrdenGiro = {
            solicitudPagoId: this.solicitudPago.solicitudPagoId,
            ordenGiroId: this.ordenGiroId,
            ordenGiroDetalle: [
                {
                    ordenGiroId: this.ordenGiroId,
                    ordenGiroDetalleId: this.ordenGiroDetalleId,
                    ordenGiroDetalleTerceroCausacion: this.ordenGiroDetalleTerceroCausacion
                }
            ]
        }

        this.ordenGiroSvc.createEditOrdenGiro( pOrdenGiro )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/generarOrdenDeGiro/verDetalleEditarOrdenGiro', this.solicitudPago.solicitudPagoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
