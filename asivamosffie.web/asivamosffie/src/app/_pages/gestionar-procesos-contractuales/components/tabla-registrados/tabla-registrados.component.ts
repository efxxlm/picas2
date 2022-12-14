import { ContratosModificacionesContractualesService } from './../../../../core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';
import { Component, OnInit, Output, ViewChild, EventEmitter, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { GrillaProcesosContractuales } from 'src/app/_interfaces/procesosContractuales.interface';

@Component({
  selector: 'app-tabla-registrados',
  templateUrl: './tabla-registrados.component.html',
  styleUrls: ['./tabla-registrados.component.scss']
})
export class TablaRegistradosComponent implements OnInit {

  dataSource                = new MatTableDataSource();
  @Input() $data: Observable<GrillaProcesosContractuales[]>;
  @Input() esRegistrado: boolean;
  @Output() sinData = new EventEmitter<string>();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 'fechaSolicitud', 'numeroSolicitud', 'tipoSolicitud', 'estadoRegistro', 'id' ];
  dataTable: any[] = [];
  estadoCodigos = {
    registrado: '6',
    registradoNovedad: '26',
    Liquidado: '20',
    cancelado: '23',
    canceladoNovedad: '14'
  };
  estadoCodigosTipoSolicitud = {
    contratacion: '2',
    novedadContractual: '11'
  };

  constructor (
    private routes: Router,
    private contratosContractualesSvc: ContratosModificacionesContractualesService )
  {
  }

  ngOnInit(): void {
    this.getGrilla();
  };

  getGrilla () {
    this.$data.subscribe( ( resp: any ) => {

        for ( let contrataciones of resp ) {
          if(!this.esRegistrado){
            if ( (contrataciones.estadoCodigo === this.estadoCodigos.cancelado && contrataciones.tipoSolicitudCodigo === this.estadoCodigosTipoSolicitud.contratacion) || (contrataciones.estadoCodigo === this.estadoCodigos.canceladoNovedad && contrataciones.tipoSolicitudCodigo === this.estadoCodigosTipoSolicitud.novedadContractual) ) {
              this.dataTable.push( contrataciones );
            };
          }else{
            if ( contrataciones.estadoCodigo === this.estadoCodigos.registrado || contrataciones.estadoCodigo === this.estadoCodigos.registradoNovedad || (contrataciones.estadoCodigo === this.estadoCodigos.Liquidado && contrataciones.tipoSolicitud === "Liquidaci??n Contractual")) {
              this.dataTable.push( contrataciones );
            };
          }
        };

        if ( this.dataTable.length > 0 ) {
          this.sinData.emit( 'completo' );
          this.dataTable.forEach( registro => registro.fechaSolicitud !== undefined ? registro.fechaSolicitud = registro.fechaSolicitud.split('T')[0].split('-').reverse().join('/') : '---' );
        }

        this.dataSource                        = new MatTableDataSource( this.dataTable );
        this.dataSource.paginator              = this.paginator;
        this.dataSource.sort                   = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por p??gina';
    } );
  }

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  gestionar ( tipoSolicitud: string, id: number, estadoCodigo: string ) {

    switch ( tipoSolicitud ) {

      case "Contrataci??n":
        this.routes.navigate( [ '/procesosContractuales/contratacionRegistrados', id ], { state: { estadoCodigo } } );
      break;

      case "Novedad Contractual":
        this.routes.navigate( [ '/procesosContractuales/modificacionContractual', id ] );
      break;
      case 'Liquidaci??n Contractual':
        this.routes.navigate( [ '/procesosContractuales/liquidacion', id ], { state: { estadoCodigo } } )
      break;
      default:
        console.log( 'No es una opcion valida.' );

    };

  };

};
