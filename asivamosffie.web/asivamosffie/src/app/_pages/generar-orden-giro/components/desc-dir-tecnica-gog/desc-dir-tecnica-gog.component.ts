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
    @Input() solicitudPagoFase: any
    @Input() esPreconstruccion: boolean;
    @Output() tieneObservacion = new EventEmitter<boolean>();
    solicitudPagoFaseCriterio: any[];
    solicitudPagoFaseFactura: any;
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
        this.solicitudPagoFaseCriterio = this.solicitudPagoFase.criteriosFase;
        this.solicitudPagoFaseFactura = this.solicitudPagoFase.solicitudPagoFaseFactura[0];

        if ( this.solicitudPago.contratoSon.solicitudPago.length > 1 ) {
            this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.contratoSon.solicitudPago[0].solicitudPagoCargarFormaPago[0];
        } else {
            this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.solicitudPagoCargarFormaPago[0];
        }
        /*
            get listaCriterios para lista desplegable
            Se reutilizan los servicios del CU 4.1.7 "Solicitud de pago"
        */
        this.listaCriterios = await this.registrarPagosSvc.getCriterioByFormaPagoCodigo( this.solicitudPagoFase.esPreconstruccion === true ? this.fasePreConstruccionFormaPagoCodigo.fasePreConstruccionFormaPagoCodigo : this.fasePreConstruccionFormaPagoCodigo.faseConstruccionFormaPagoCodigo ).toPromise()

        this.listaCriterios.forEach( value => {
            const CRITERIO_INDEX = this.solicitudPagoFaseCriterio.findIndex( criterio => value.codigo === criterio.tipoCriterioCodigo )

            if ( CRITERIO_INDEX === -1 ) {
                this.listaCriterios.splice( CRITERIO_INDEX, 1 )
            }
        } )

        // Get data de la tabla descuentos
        this.solicitudPagoFaseCriterio.forEach( criterio => this.listData.valorNetoGiro += criterio.valorFacturado );
        this.solicitudPagoFaseFactura.solicitudPagoFaseFacturaDescuento.forEach( descuento => {
            this.listData.valorNetoGiro -= descuento.valorDescuento;
            this.listData.valorTotalDescuentos += descuento.valorDescuento;

            this.listData.listaDescuentos.push(
                {
                    tipoDescuentoCodigo: descuento.tipoDescuentoCodigo,
                    valorDescuento: descuento.valorDescuento
                }
            )
        } );

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
