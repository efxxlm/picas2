import { Component, OnInit, Output, ViewChild, EventEmitter, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import moment from 'moment';
import { ContratosModificacionesContractualesService } from 'src/app/core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';

@Component({
  selector: 'app-tabla-registrados',
  templateUrl: './tabla-registrados.component.html',
  styleUrls: ['./tabla-registrados.component.scss']
})
export class TablaRegistradosComponent implements OnInit {

  @Input() esRegistrado: boolean;
  dataSource                = new MatTableDataSource();
  @Output() sinData = new EventEmitter<boolean>();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 'fechaCreacion', 'numeroSolicitud', 'tipoSolicitud', 'estadoRegistro', 'id' ];
  dataTable: any[] = [];
  estadoCodigos = {
    registrado: '6',
    registradoNovedad: '26',
    cancelado: '23',
    canceladoNovedad: '14'
  };
  estadoCodigosTipoSolicitud = {
    contratacion: '2',
    novedadContractual: '11'
  };

  constructor ( private routes: Router,
                private contratosContractualesSvc: ContratosModificacionesContractualesService ) {
    this.getGrilla();
  }

  ngOnInit(): void {
  };

  getGrilla () {
    this.contratosContractualesSvc.getGrilla()
      .subscribe( ( resp: any ) => {

        for ( let contrataciones of resp ) {
          if(!this.esRegistrado){
            if ( (contrataciones.estadoCodigo === this.estadoCodigos.cancelado && contrataciones.tipoSolicitudCodigo === this.estadoCodigosTipoSolicitud.contratacion) || (contrataciones.estadoCodigo === this.estadoCodigos.canceladoNovedad && contrataciones.tipoSolicitudCodigo === this.estadoCodigosTipoSolicitud.novedadContractual) ) {
              this.dataTable.push( contrataciones );
            };
          }else{
            if ( contrataciones.estadoCodigo === this.estadoCodigos.registrado || contrataciones.estadoCodigo === this.estadoCodigos.registradoNovedad ) {
              this.dataTable.push( contrataciones );
            };
          }
        };

        if ( this.dataTable.length === 0 ) {
          this.sinData.emit( false );
        }
        if ( this.dataTable.length > 0 ) {
          this.dataTable.forEach( registro => registro.fechaCreacion = registro.contratacion.fechaCreacion !== undefined ? moment( registro.contratacion.fechaCreacion ).format( 'DD/MM/YYYY' ) : '' );
        }
        this.dataSource                        = new MatTableDataSource( this.dataTable );
        this.dataSource.paginator              = this.paginator;
        this.dataSource.sort                   = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      } );
  }

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  gestionar ( tipoSolicitud: string, id: number, estadoCodigo: string ) {
    switch ( tipoSolicitud ) {

      case "Contratación":
        this.routes.navigate( [ '/contratosModificacionesContractuales/contratacion', id ], { state: { estadoCodigo } } );
      break;

      case "Novedad Contractual":
        this.routes.navigate( [ '/contratosModificacionesContractuales/detalleModificacionContractual', id ] );
      break;
      default:
        console.log( 'No es una opcion valida.' );

    };

  };

};
