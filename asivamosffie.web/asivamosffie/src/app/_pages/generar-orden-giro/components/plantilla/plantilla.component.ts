import { CommonService, Dominio } from './../../../../core/_services/common/common.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { TipoSolicitudCodigo } from './enums-plantilla/tipo-solicitud.enums';
import { FormGroup, FormBuilder, FormArray, Validators } from '@angular/forms';
import humanize from 'humanize-plus';

@Component({
  selector: 'app-plantilla',
  templateUrl: './plantilla.component.html',
  styleUrls: ['./plantilla.component.scss']
})
export class PlantillaComponent implements OnInit {

    solicitudPago = undefined;
    tipoSolicitudCodigo = TipoSolicitudCodigo;
    bancosArray: Dominio[] = [];
    formOrigen: FormGroup = this.fb.group(
        {
            aportantes: this.fb.array( [] )
        }
    );

    get aportantes() {
        return this.formOrigen.get( 'aportantes' ) as FormArray;
    }

    constructor(
        private activatedRoute: ActivatedRoute,
        private commonSvc: CommonService,
        private ordenGiroSvc: OrdenPagoService,
        private fb: FormBuilder )
    {
        this.getOrdenGiro()
    }

    ngOnInit(): void {
    }

    async getOrdenGiro() {
        const solicitudPago = await this.ordenGiroSvc.getSolicitudPagoBySolicitudPagoId( this.activatedRoute.snapshot.params.id ).toPromise()
        this.bancosArray = await this.commonSvc.listaBancos().toPromise()
        this.getOrigen()

        console.log( solicitudPago )
        this.solicitudPago = solicitudPago
    }

    async getOrigen() {
        const dataAportantes = await this.ordenGiroSvc.getAportantes( this.solicitudPago );

        // Get IDs
        if ( this.solicitudPago.ordenGiro !== undefined ) {
            const listaAportantes = []

            if ( this.solicitudPago.ordenGiro.ordenGiroDetalle !== undefined ) {
                if ( this.solicitudPago.ordenGiro.ordenGiroDetalle.length > 0 ) {
                    const ordenGiroDetalle = this.solicitudPago.ordenGiro.ordenGiroDetalle[0];

                    if ( ordenGiroDetalle.ordenGiroDetalleTerceroCausacion !== undefined ) {
                        if ( ordenGiroDetalle.ordenGiroDetalleTerceroCausacion.length > 0 ) {
                            const ordenGiroDetalleTerceroCausacion = ordenGiroDetalle.ordenGiroDetalleTerceroCausacion;

                            ordenGiroDetalleTerceroCausacion.forEach( terceroCausacion => {
                                if ( terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.length > 0 ) {
                                    terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.forEach( aportante => {
                                        const aportanteFind = listaAportantes.find( value => value.aportanteId === aportante.aportanteId )

                                        if ( aportanteFind === undefined ) {
                                            listaAportantes.push( aportante );
                                        }
                                    } );
                                }
                            } );

                            for ( const aportante of listaAportantes ) {
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
                                        return aportante.fuenteFinanciacion.cuentaBancaria[ 0 ];
                                    }
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
                        }
                    }
                }
            }
        }
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }

    getBanco( codigo: string ) {
        if ( this.bancosArray.length > 0 ) {
            const banco = this.bancosArray.find( banco => banco.codigo === codigo );

            if ( banco !== undefined ) {
                return banco.nombre;
            }
        }
    }

}
