import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabla-proceso-firmas',
  templateUrl: './tabla-proceso-firmas.component.html',
  styleUrls: ['./tabla-proceso-firmas.component.scss']
})
export class TablaProcesoFirmasComponent implements OnInit {

  @Input() dataTable: any[] = [];
  dataSource                = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 'fechaSolicitud', 'numeroSolicitud', 'tipoSolicitud', 'estadoRegistro', 'estadoDocumento', 'id' ];
  ELEMENT_DATA    : any[]    = [
    { titulo: 'Número de solicitud', name: 'numeroSolicitud' },
    { titulo: 'Tipo de solicitud', name: 'tipoSolicitud' }
  ];

  constructor ( private routes: Router ) { }

  ngOnInit(): void {
    this.dataSource                        = new MatTableDataSource( this.dataTable );
    this.dataSource.paginator              = this.paginator;
    this.dataSource.sort                   = this.sort;
  };

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  gestionar ( tipoSolicitud: string, id: number ) {

    switch ( tipoSolicitud ) {

      case "Contratación":
        this.routes.navigate( [ '/contratosModificacionesContractuales/contratacion', id ] );
      break;

      case "Modificación contractual":
        this.routes.navigate( [ '/contratosModificacionesContractuales/modificacionContractual', id ] );
      break;
      default:
        console.log( 'No es una opcion valida.' );

    };

  };

}
