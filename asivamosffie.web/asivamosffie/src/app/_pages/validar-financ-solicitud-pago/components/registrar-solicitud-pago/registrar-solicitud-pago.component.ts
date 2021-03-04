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
    solicitudPagoObservacionId = 0;
    solicitudPago: any;
    solicitudPagoFase: any;
    dataSource = new MatTableDataSource();
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    displayedColumns: string[] = [
        'faseContrato',
        'pagosRealizados',
        'valorFacturado',
        'porcentajeFacturado',
        'saldoPorPagar',
        'porcentajePorPagar'
    ];
    dataTable: any[] = [
        {
          faseContrato: 'Fase 1 - Preconstrucción',
          pagosRealizados: '0',
          valorFacturado: '0',
          porcentajeFacturado: '0',
          saldoPorPagar: '$30.000.000',
          porcentajePorPagar: '100%',
        },
        {
          faseContrato: 'Fase 2 - Construcción',
          pagosRealizados: '0',
          valorFacturado: '0',
          porcentajeFacturado: '0',
          saldoPorPagar: '$75.000.000',
          porcentajePorPagar: '100%',
        }
    ];

    constructor( ){
    }

    ngOnInit(): void {
        if ( this.contrato !== undefined ) {
            this.solicitudPago = this.contrato.solicitudPagoOnly;
            this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];
        }

        this.dataSource = new MatTableDataSource(this.dataTable);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
    }

}
