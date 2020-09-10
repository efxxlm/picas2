import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { GrillaProcesosContractuales } from 'src/app/_interfaces/procesosContractuales.interface';

@Component({
  selector: 'app-tabla-solicitudes-enviadas',
  templateUrl: './tabla-solicitudes-enviadas.component.html',
  styleUrls: ['./tabla-solicitudes-enviadas.component.scss']
})
export class TablaSolicitudesEnviadasComponent implements OnInit {

  @Input() enviadasFiduciaria: boolean;
  @Input() $data: Observable<GrillaProcesosContractuales[]>;
  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 'fechaSolicitud', 'numeroSolicitud', 'tipoSolicitud', 'estadoDelRegistro', 'id' ];
  estadoCodigo: string;

  constructor ( private routes: Router ) {
  }

  ngOnInit() {
    this.$data.subscribe( resp => {
      console.log( resp );
      this.dataSource = new MatTableDataSource( resp );
      this.dataSource.paginator              = this.paginator;
      this.dataSource.sort                   = this.sort;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    } );
  };

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  gestionar ( tipoSolicitud: string, id: number ) {
    
    switch ( tipoSolicitud ) {

      case 'Contratación':
        this.routes.navigate( [ '/procesosContractuales/contratacion', id ] )
      break;
      case 'Modificación contractual':
        /*Ejemplo para los 3 tipos de modificaciones contractuales*/
        //this.routes.navigate( [ '/procesosContractuales/modificacionContractual', id ], { state: { suspension: true, reinicio: false } } )
        //this.routes.navigate( [ '/procesosContractuales/modificacionContractual', id ], { state: { suspension: true, reinicio: false } } );
        this.routes.navigate( [ '/procesosContractuales/modificacionContractual', id ], { state: { suspension: false, reinicio: true } } )
      break;
      case 'Liquidación':
        this.routes.navigate( [ '/procesosContractuales/liquidacion', id ] )
      break;
      default:
        console.log( 'No es un tipo de solicitud valido.' );

    };

  };

}
