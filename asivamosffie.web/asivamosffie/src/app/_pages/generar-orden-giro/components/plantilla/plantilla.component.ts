import { CommonService, Dominio } from './../../../../core/_services/common/common.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { TipoSolicitudCodigo } from './enums-plantilla/tipo-solicitud.enums';
import { FormBuilder, FormArray, Validators } from '@angular/forms';
import humanize from 'humanize-plus';
import { MediosPagoCodigo } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';

@Component({
  selector: 'app-plantilla',
  templateUrl: './plantilla.component.html',
  styleUrls: ['./plantilla.component.scss']
})
export class PlantillaComponent implements OnInit {

    solicitudPago = undefined;
    ordenGiroTercero = undefined;
    tipoSolicitudCodigo = TipoSolicitudCodigo;
    listaMediosPagoCodigo = MediosPagoCodigo;
    bancosArray: Dominio[] = [];
    listaMedioPago: Dominio[] = [];
    listaBancos: Dominio[] = [];
    terceroGiroForm = this.fb.group({
        medioPagoGiroContrato: [null, Validators.required],
        transferenciaElectronica: this.fb.group( {
            ordenGiroTerceroId: [ 0 ],
            ordenGiroTerceroTransferenciaElectronicaId: [ 0 ],
            titularCuenta: [ '' ],
            titularNumeroIdentificacion: [ '' ],
            numeroCuenta: [ '' ],
            bancoCodigo: [ null ],
            esCuentaAhorros: [ null ]
        } ),
        chequeGerencia: this.fb.group( {
            ordenGiroTerceroId: [ 0 ],
            ordenGiroTerceroChequeGerenciaId: [ 0 ],
            nombreBeneficiario: [ '' ],
            numeroIdentificacionBeneficiario: [ '' ]
        } )
    })
    listaDetalleGiro: { contratacionProyectoId: number, llaveMen: string, fases: any[], semaforoDetalle: string }[] = [];
    formOrigen = this.fb.group(
        {
            aportantes: this.fb.array( [] )
        }
    )

    get aportantes() {
        return this.formOrigen.get( 'aportantes' ) as FormArray;
    }

    constructor(
        private activatedRoute: ActivatedRoute,
        private commonSvc: CommonService,
        private ordenGiroSvc: OrdenPagoService,
        private fb: FormBuilder,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
        this.getOrdenGiro()
    }

    ngOnInit(): void {
    }

    async getOrdenGiro() {
        const solicitudPago = await this.ordenGiroSvc.getSolicitudPagoBySolicitudPagoId( this.activatedRoute.snapshot.params.id ).toPromise()
        this.bancosArray = await this.commonSvc.listaBancos().toPromise()
        this.solicitudPago = solicitudPago

        this.getProyectos()
        this.getOrigen()
        this.getDataTerceroGiro()

        console.log( solicitudPago )
    }

    async getProyectos() {
        // Peticion asincrona de los proyectos por contratoId
        const getProyectosByIdContrato: any[] = await this.registrarPagosSvc.getProyectosByIdContrato( this.solicitudPago.contratoId ).toPromise();
        const LISTA_PROYECTOS: any[] = getProyectosByIdContrato[1];
        const solicitudPagoFase: any[] = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[ 0 ].solicitudPagoFase;

        LISTA_PROYECTOS.forEach( proyecto => {
            // Objeto Proyecto que se agregara al array listaDetalleGiro
            const PROYECTO = {
                semaforoDetalle: 'sin-diligenciar',
                contratacionProyectoId: proyecto.contratacionProyectoId,
                llaveMen: proyecto.llaveMen,
                fases: []
            }

            const listFase = solicitudPagoFase.filter( fase => fase.contratacionProyectoId === proyecto.contratacionProyectoId )
            PROYECTO.fases = listFase

            if ( PROYECTO.fases.length > 0 ) {
                this.listaDetalleGiro.push( PROYECTO )
            }
        } )
    }

    getDataTerceroGiro() {
        this.commonSvc.listaMediosPago()
        .subscribe( listaMediosPago => {
            this.listaMedioPago = listaMediosPago;

            this.commonSvc.listaBancos()
                .subscribe( async listaBancos => {
                    this.listaBancos = listaBancos;

                    if ( this.solicitudPago.ordenGiro !== undefined ) {
                        if ( this.solicitudPago.ordenGiro.ordenGiroTercero !== undefined ) {
                            if ( this.solicitudPago.ordenGiro.ordenGiroTercero.length > 0 ) {
                                this.ordenGiroTercero = this.solicitudPago.ordenGiro.ordenGiroTercero[0];
                                // Get data tercero de giro
                                const medioPago = this.listaMedioPago.find( medio => medio.codigo === this.ordenGiroTercero.medioPagoGiroCodigo );

                                if ( medioPago !== undefined ) {
                                    this.terceroGiroForm.get( 'medioPagoGiroContrato' ).setValue( medioPago.codigo );
                                }

                                if ( this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica !== undefined ) {
                                    if ( this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica.length > 0 ) {
                                        const ordenGiroTerceroTransferenciaElectronica = this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica[0];

                                        this.terceroGiroForm.get( 'transferenciaElectronica' ).setValue(
                                            {
                                                ordenGiroTerceroId: 0,
                                                ordenGiroTerceroTransferenciaElectronicaId: ordenGiroTerceroTransferenciaElectronica.ordenGiroTerceroTransferenciaElectronicaId,
                                                titularCuenta: ordenGiroTerceroTransferenciaElectronica.titularCuenta !== undefined ? ordenGiroTerceroTransferenciaElectronica.titularCuenta : '',
                                                titularNumeroIdentificacion: ordenGiroTerceroTransferenciaElectronica.titularNumeroIdentificacion !== undefined ? ordenGiroTerceroTransferenciaElectronica.titularNumeroIdentificacion : '',
                                                numeroCuenta: ordenGiroTerceroTransferenciaElectronica.numeroCuenta !== undefined ? ordenGiroTerceroTransferenciaElectronica.numeroCuenta : '',
                                                bancoCodigo: ordenGiroTerceroTransferenciaElectronica.bancoCodigo !== undefined ? ordenGiroTerceroTransferenciaElectronica.bancoCodigo : null,
                                                esCuentaAhorros: ordenGiroTerceroTransferenciaElectronica.esCuentaAhorros !== undefined ? ordenGiroTerceroTransferenciaElectronica.esCuentaAhorros : null
                                            }
                                        )
                                    }
                                }

                                if ( this.ordenGiroTercero.ordenGiroTerceroChequeGerencia !== undefined ) {
                                    if ( this.ordenGiroTercero.ordenGiroTerceroChequeGerencia.length > 0 ) {
                                        const ordenGiroTerceroChequeGerencia = this.ordenGiroTercero.ordenGiroTerceroChequeGerencia[0];
                                        
                                        this.terceroGiroForm.get( 'chequeGerencia' ).setValue(
                                            {
                                                ordenGiroTerceroId: 0,
                                                ordenGiroTerceroChequeGerenciaId: ordenGiroTerceroChequeGerencia.ordenGiroTerceroChequeGerenciaId,
                                                nombreBeneficiario: ordenGiroTerceroChequeGerencia.nombreBeneficiario !== undefined ? ordenGiroTerceroChequeGerencia.nombreBeneficiario : '',
                                                numeroIdentificacionBeneficiario: ordenGiroTerceroChequeGerencia.numeroIdentificacionBeneficiario !== undefined ? ordenGiroTerceroChequeGerencia.numeroIdentificacionBeneficiario : ''
                                            }
                                        )
                                    }
                                }
                            }
                        }
                    }
                } );
        } );
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
