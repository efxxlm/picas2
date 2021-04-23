import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TipoSolicitud, TipoSolicitudes } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Component({
  selector: 'app-informacion-general',
  templateUrl: './informacion-general.component.html',
  styleUrls: ['./informacion-general.component.scss']
})
export class InformacionGeneralComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Input() esExpensas: boolean;
    @Output() tieneObservacion = new EventEmitter<boolean>();
    listaTipoSolicitud: TipoSolicitud = TipoSolicitudes;
    solicitudPagoFase: any;
    ordenGiroTercero: any;
    valorTotalFactura = 0;
    semaforoInfoGeneral = 'sin-diligenciar';
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
      'drp',
      'numeroDrp',
      'valorSolicitudDdp',
      'saldoPresupuestal'
    ];

    constructor( )
    {
    }

    ngOnInit(): void {
        this.getSolicitudPago();
    };

    getSolicitudPago() {
        // Get semaforo informacion general
        if ( this.esExpensas === true ) {
            this.valorTotalFactura = this.solicitudPago.solicitudPagoExpensas[ 0 ].valorFacturado;
        } else {
            this.valorTotalFactura = this.solicitudPago.solicitudPagoOtrosCostosServicios[0].valorFacturado;
        }

        if ( this.solicitudPago.ordenGiro !== undefined ) {
            if ( this.solicitudPago.ordenGiro.ordenGiroTercero !== undefined ) {
                if ( this.solicitudPago.ordenGiro.ordenGiroTercero.length > 0 ) {
                    this.ordenGiroTercero = this.solicitudPago.ordenGiro.ordenGiroTercero[0];

                    if ( this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica !== undefined ) {
                        if ( this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica.length > 0 ) {
                            const ordenGiroTerceroTransferenciaElectronica = this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica[0];

                            if ( ordenGiroTerceroTransferenciaElectronica.registroCompleto === false ) {
                                this.semaforoInfoGeneral = 'en-proceso';
                            }
                            if ( ordenGiroTerceroTransferenciaElectronica.registroCompleto === true ) {
                                this.semaforoInfoGeneral = 'completo';
                            }
                        }
                    }

                    if ( this.ordenGiroTercero.ordenGiroTerceroChequeGerencia !== undefined ) {
                        if ( this.ordenGiroTercero.ordenGiroTerceroChequeGerencia.length > 0 ) {
                            const ordenGiroTerceroChequeGerencia = this.ordenGiroTercero.ordenGiroTerceroChequeGerencia[0];

                            if ( ordenGiroTerceroChequeGerencia.registroCompleto === false ) {
                                this.semaforoInfoGeneral = 'en-proceso';
                            }
                            if ( ordenGiroTerceroChequeGerencia.registroCompleto === true ) {
                                this.semaforoInfoGeneral = 'completo';
                            }
                        }
                    }
                }
            }
        }

        this.dataSource = new MatTableDataSource( this.solicitudPago.tablaDRP );
    }

    getTipoSolicitudContrato( tipoSolicitudCodigo: string ) {
        if ( tipoSolicitudCodigo === this.listaTipoSolicitud.otrosCostos ) {
            return 'Otros costos y servicios';
        }

        if ( tipoSolicitudCodigo === this.listaTipoSolicitud.expensas ) {
            return 'Expensas';
        }
    }

    applyFilter(event: Event) {
        const filterValue = (event.target as HTMLInputElement).value;
        this.dataSource.filter = filterValue.trim().toLowerCase();
    };

}
