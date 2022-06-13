import { CommonService, Dominio } from './../../../../core/_services/common/common.service';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';

@Component({
  selector: 'app-desc-dir-tecnica-gog',
  templateUrl: './desc-dir-tecnica-gog.component.html',
  styleUrls: ['./desc-dir-tecnica-gog.component.scss']
})
export class DescDirTecnicaGogComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Input() solicitudPagoFase: any;
    @Input() esPreconstruccion: boolean;
    @Output() estadoSemaforo = new EventEmitter<string>();
    solicitudPagoFaseCriterio: any[];
    solicitudPagoFaseFactura: any;
    solicitudPagoFaseFacturaDescuento: any;
    fasePreConstruccionFormaPagoCodigo: any;
    recibeListaChqueo = false;
    tiposDescuentoArray: Dominio[] = [];
    listaCriterios: Dominio[] = [];
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'tipoDescuento',
      'valorDescuento',
      'valorTotalDescuentos',
      'valorNetoGiro'
    ];
    listData = {
        listaDescuentos: [],
        valorNetoGiro: 0,
        valorTotalDescuentos: 0
    };

    constructor(
        private commonSvc: CommonService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
        this.commonSvc.tiposDescuento()
            .subscribe(response => this.tiposDescuentoArray = response);
    }

    ngOnInit(): void {
        this.getDireccionTecnica();
    }

    async getDireccionTecnica() {
        // Get Tablas
        this.solicitudPagoFaseCriterio = this.solicitudPagoFase.solicitudPagoFaseCriterio;
        this.solicitudPagoFaseFacturaDescuento = this.solicitudPagoFase.solicitudPagoFaseFacturaDescuento;

        if ( this.solicitudPago.contratoSon.solicitudPago.length > 1 ) {
            this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.contratoSon.solicitudPago[0].solicitudPagoCargarFormaPago[0];
        } else {
            this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.solicitudPagoCargarFormaPago[0];
        }
        /*
            get listaCriterios para lista desplegable
            Se reutilizan los servicios del CU 4.1.7 "Solicitud de pago"
        */
        const listaCriteriosTemp = await this.registrarPagosSvc.getCriterioByFormaPagoCodigo( this.solicitudPagoFase.esPreconstruccion === true ? this.fasePreConstruccionFormaPagoCodigo.fasePreConstruccionFormaPagoCodigo : this.fasePreConstruccionFormaPagoCodigo.faseConstruccionFormaPagoCodigo ).toPromise()
        const listaCriteriosFinal = []

        this.solicitudPagoFaseCriterio.forEach( fc => {
            const CRITERIO = listaCriteriosTemp.find( c => c.codigo === fc.tipoCriterioCodigo )

            if ( CRITERIO !== undefined ) listaCriteriosFinal.push( CRITERIO )
        } )
        this.listaCriterios = listaCriteriosFinal
        // this.listaCriterios.forEach( value => {
        //     const CRITERIO_INDEX = this.solicitudPagoFaseCriterio.findIndex( criterio => value.codigo === criterio.tipoCriterioCodigo )

        //     if ( CRITERIO_INDEX === -1 ) {
        //         this.listaCriterios.splice( CRITERIO_INDEX, 1 )
        //     }
        // } )

        // Get data de la tabla descuentos
        this.solicitudPagoFaseCriterio.forEach( criterio => this.listData.valorNetoGiro += criterio.valorFacturado );
        this.solicitudPagoFaseFacturaDescuento.forEach( descuento => {
            this.listData.valorNetoGiro -= descuento.valorDescuento;
            this.listData.valorTotalDescuentos += descuento.valorDescuento;

            this.listData.listaDescuentos.push(
                {
                    tipoDescuentoCodigo: descuento.tipoDescuentoCodigo,
                    valorDescuento: descuento.valorDescuento
                }
            )
        } );

        // Check semaforo descuentos de direccion tecnica
        const ordenGiro = this.solicitudPago.ordenGiro

        if ( ordenGiro !== undefined ) {
            const ordenGiroDetalle = ordenGiro.ordenGiroDetalle[ 0 ]

            if ( ordenGiroDetalle !== undefined ) {
                const ordenGiroDetalleDescuentoTecnica: any[] = ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica

                if ( ordenGiroDetalleDescuentoTecnica !== undefined && ordenGiroDetalleDescuentoTecnica.length > 0 ) {
                    const detalleDescuentoFilter = ordenGiroDetalleDescuentoTecnica.filter( detalle => detalle.contratacionProyectoId === this.solicitudPagoFase.contratacionProyectoId )

                    if ( detalleDescuentoFilter.length > 0 ) {
                        const totalRegistrosCompletos = detalleDescuentoFilter.filter( detalle => detalle.registroCompleto === true ).length
                        const totalRegistrosEnProceso = detalleDescuentoFilter.filter( detalle => detalle.registroCompleto === false ).length

                        if ( totalRegistrosCompletos > 0 && totalRegistrosCompletos === detalleDescuentoFilter.length ) {
                            this.estadoSemaforo.emit( 'completo' )
                        }

                        if ( totalRegistrosCompletos > 0 && totalRegistrosCompletos < detalleDescuentoFilter.length ) {
                            this.estadoSemaforo.emit( 'en-proceso' )
                        }

                        if ( totalRegistrosEnProceso > 0 && totalRegistrosEnProceso === detalleDescuentoFilter.length ) {
                            this.estadoSemaforo.emit( 'en-proceso' )
                        }

                        if ( totalRegistrosEnProceso > 0 && totalRegistrosEnProceso < detalleDescuentoFilter.length ) {
                            this.estadoSemaforo.emit( 'en-proceso' )
                        }
                    }
                }
            }
        }


        this.dataSource = new MatTableDataSource( [ this.listData ] );
    }

    getTipoDescuento( tipoDescuentoCodigo: string ) {
        if (this.tiposDescuentoArray.length > 0) {
            const descuento = this.tiposDescuentoArray.find( descuento => descuento.codigo === tipoDescuentoCodigo );
            
            if ( descuento !== undefined ) {
                return descuento.nombre;
            }
        }
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    }

}
