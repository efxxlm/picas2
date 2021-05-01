import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-registrar-solicitud-pago',
  templateUrl: './registrar-solicitud-pago.component.html',
  styleUrls: ['./registrar-solicitud-pago.component.scss']
})
export class RegistrarSolicitudPagoComponent implements OnInit {

    @Input() contrato: any;
    @Input() esVerDetalle = false;
    @Input() aprobarSolicitudPagoId: any;
    @Input() registrarSolicitudPago: any;
    solicitudPagoRegistrarSolicitudPago: any
    solicitudPagoObservacionId = 0;
    solicitudPago: any;
    solicitudPagoFase: any;
    manejoAnticipoRequiere: boolean;
    tienePreconstruccion = false;
    tieneConstruccion = false;
    dataSource = new MatTableDataSource();
    solicitudPagoCargarFormaPago: any;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
        'faseContrato',
        'valorFacturado',
        'porcentajeFacturado',
        'saldoPorPagar',
        'porcentajePorPagar'
    ];

    constructor( ){
    }

    ngOnInit(): void {
        if ( this.contrato !== undefined ) {

            this.manejoAnticipoRequiere = this.contrato.contratoConstruccion.length > 0 ? this.contrato.contratoConstruccion[0].manejoAnticipoRequiere : false;

            if ( this.contrato.solicitudPago.length > 1 ) {
                this.solicitudPagoCargarFormaPago = this.contrato.solicitudPago[0].solicitudPagoCargarFormaPago[0];
            } else {
                this.solicitudPagoCargarFormaPago = this.contrato.solicitudPagoOnly.solicitudPagoCargarFormaPago[0];
            }

            this.solicitudPago = this.contrato.solicitudPagoOnly;
            this.solicitudPagoRegistrarSolicitudPago = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0];
            
            if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase !== undefined ) {
                if ( this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
                    for ( const solicitudPagoFase of this.solicitudPagoRegistrarSolicitudPago.solicitudPagoFase ) {
                        if ( solicitudPagoFase.esPreconstruccion === true ) {
                            this.tienePreconstruccion = true;
                        }

                        if ( solicitudPagoFase.esPreconstruccion === false ) {
                            this.tieneConstruccion = true;
                        }
                    }
                }
            }
        }

        this.dataSource = new MatTableDataSource( this.contrato.vContratoPagosRealizados );
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
    }

}
