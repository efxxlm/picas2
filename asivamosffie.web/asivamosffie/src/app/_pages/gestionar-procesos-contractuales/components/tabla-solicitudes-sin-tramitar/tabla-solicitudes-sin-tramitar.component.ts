import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabla-solicitudes-sin-tramitar',
  templateUrl: './tabla-solicitudes-sin-tramitar.component.html',
  styleUrls: ['./tabla-solicitudes-sin-tramitar.component.scss']
})
export class TablaSolicitudesSinTramitarComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort: MatSort;
  displayedColumns: string[] = [ 'fechaSolicitud', 'numeroSolicitud', 'tipoSolicitud', 'estadoRegistro', 'id' ];
  ELEMENT_DATA: any[] = [
    {titulo: 'Fecha de la solicitud', name: 'fechaSolicitud'},
    { titulo: 'Número de solicitud', name: 'numeroSolicitud' },
    { titulo: 'Tipo de solicitud', name: 'tipoSolicitud' }
  ];

  data: any[] = [
    {
      fechaSolicitud: '15/06/2020',
      numeroSolicitud: 'PI_008',
      tipoSolicitud: 'Contratación',
      estadoRegistro: false,
      fechaEnvioTramite: null,
      id: 0
    },
    {
      fechaSolicitud: '15/06/2020',
      numeroSolicitud: 'CO_003',
      tipoSolicitud: 'Modificación contractual',
      estadoRegistro: false,
      fechaEnvioTramite: null,
      id: 1
    },
    {
      fechaSolicitud: '15/06/2020',
      numeroSolicitud: 'SL_001',
      tipoSolicitud: 'Liquidación',
      estadoRegistro: false,
      fechaEnvioTramite: null,
      id: 2
    }
  ]

  constructor ( private routes: Router ) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource( this.data );
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  gestionar ( tipoSolicitud: string, id: number ) {
    
    switch ( tipoSolicitud ) {

      case 'Contratación':
        this.routes.navigate( [ '/procesosContractuales/contratacion', id ] )
      break;
      case 'Modificación contractual':
        this.routes.navigate( [ '/procesosContractuales/modificacionContractual', id ] )
      break;
      case 'Liquidación':
        this.routes.navigate( [ '/procesosContractuales/liquidacion', id ] )
      break;
      default:
        console.log( 'No es un tipo de solicitud aceptada' );

    };

  };

};