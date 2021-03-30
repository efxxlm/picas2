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
    @Output() seDiligenciaFormulario = new EventEmitter<boolean>();
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
        this.ordenGiroSvc.getAportantes( this.solicitudPago, dataAportantes => {

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

                                this.ordenGiroDetalleTerceroCausacion.forEach( terceroCausacion => {
                                    if ( terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.length > 0 ) {
                                        terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.forEach( aportante => this.listaAportantes.push( aportante ) );
                                    }
                                } );

                                for ( const aportante of this.listaAportantes ) {
                                    const nombreAportante = dataAportantes.listaNombreAportante.find( nombreAportante => nombreAportante.cofinanciacionAportanteId === aportante.aportanteId );
                                    const tipoAportante = dataAportantes.listaTipoAportante.find( tipoAportante => tipoAportante.dominioId === nombreAportante.tipoAportanteId );

                                    this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId( nombreAportante.cofinanciacionAportanteId )
                                        .subscribe( fuente => {
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
                                                    return aportante.fuenteFinanciacion.cuentaBancaria[ 0 ];
                                                }
                                            }

                                            if ( aportante.fuenteFinanciacion.cuentaBancaria.length === 1 ) {
                                                totalCuenta++;
                                            }
                                            console.log( cuentaBancaria() );
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
                                        } );
                                }

                                setTimeout(() => {
                                    if ( totalCuenta === this.listaAportantes.length ) {
                                        this.esUnicaCuenta = true;
                                        this.seDiligenciaFormulario.emit( false );
                                    }
                                }, 1000);
                            }
                        }
                    }
                }
            }
        } );
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

    guardar() {
        
        this.aportantes.controls.forEach( control => {
            this.ordenGiroDetalleTerceroCausacion.forEach( terceroCausacion => {
                terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.forEach( aportante => {
                    if ( control.get( 'nombreAportante' ).value.cofinanciacionAportanteId === aportante.aportanteId ) {
                        aportante.cuentaBancariaId = control.get( 'cuentaBancariaId' ).value.cuentaBancariaId;
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
                                '/generarOrdenDeGiro/generacionOrdenGiro', this.solicitudPago.solicitudPagoId
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
