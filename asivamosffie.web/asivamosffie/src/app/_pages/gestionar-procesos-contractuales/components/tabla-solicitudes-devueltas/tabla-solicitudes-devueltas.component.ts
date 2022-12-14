import { Component, Input, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { EstadoSolicitudNovedadCodigo } from 'src/app/_interfaces/estados-novedad.interface';
import { EstadoSolicitudContratacionCodigo } from 'src/app/_interfaces/estados-solicitudContratacion.interface';
import { GrillaProcesosContractuales, TipoSolicitudCodigo } from 'src/app/_interfaces/procesosContractuales.interface';

@Component({
  selector: 'app-tabla-solicitudes-devueltas',
  templateUrl: './tabla-solicitudes-devueltas.component.html',
  styleUrls: ['./tabla-solicitudes-devueltas.component.scss']
})
export class TablaSolicitudesDevueltasComponent implements OnInit {

  @Input() $data: Observable<GrillaProcesosContractuales[]>;
  @Output() estadoSemaforo = new EventEmitter<string>();
  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 'fechaSolicitud', 'numeroSolicitud', 'tipoSolicitud', 'estadoDelRegistro', 'id' ];
  estadosSolicitudContratacion  = EstadoSolicitudContratacionCodigo;
  estadosSolicitudNovedad       = EstadoSolicitudNovedadCodigo;
  tipoSolicitud                 = TipoSolicitudCodigo;


  estadoCodigo: string;
  estadoCodigos = {
    devueltaContratacion: this.estadosSolicitudContratacion.DevueltaProcesoContractual,
    devueltaNovedad: this.estadosSolicitudNovedad.DevueltaProcesoContractual,
    devueltaLiquidacion: this.estadosSolicitudContratacion.DevueltaLiquidacionProcesoContractual,
  }


  constructor ( private routes: Router ) {
  }

  ngOnInit() {
    this.$data.subscribe( ( response: any[] ) => {
      let dataTable = [];

      response.forEach( lista => {
        if ( (lista.estadoCodigo === this.estadoCodigos.devueltaContratacion && lista.tipoSolicitudCodigo == this.tipoSolicitud.Contratacion)  ||
             (lista.estadoCodigo === this.estadoCodigos.devueltaNovedad && lista.tipoSolicitudCodigo == this.tipoSolicitud.Novedad_Contractual) ||
             (lista.estadoCodigo === this.estadoCodigos.devueltaLiquidacion && lista.tipoSolicitudCodigo == this.tipoSolicitud.Liquidacion_Contractual)
            ) {
          dataTable.push( lista );
        };
      } );

      if ( dataTable.length > 0 ) {
        this.estadoSemaforo.emit( 'completo' );
      }
      if ( dataTable.length > 0 ) {
        dataTable.forEach( registro => registro.fechaSolicitud !== undefined ? registro.fechaSolicitud = registro.fechaSolicitud.split('T')[0].split('-').reverse().join('/') : '---' );
      }

      this.dataSource = new MatTableDataSource( dataTable );
      this.dataSource.paginator              = this.paginator;
      this.dataSource.sort                   = this.sort;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por p??gina';
    } );
  };

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  gestionar ( tipoSolicitud: string, solicitudId: number, sesionComiteSolicitudId: number, estadoCodigo: string ) {
    switch ( tipoSolicitud ) {
      case 'Contrataci??n':
        this.routes.navigate( [ '/procesosContractuales/contratacion', solicitudId ], { state: { sesionComiteSolicitudId, estadoCodigo } } )
      break;
      case 'Novedad Contractual':
        this.routes.navigate( [ '/procesosContractuales/modificacionContractual', solicitudId ], { state: { sesionComiteSolicitudId, estadoCodigo } } )
      break;
      case 'Liquidaci??n Contractual':
        this.routes.navigate( [ '/procesosContractuales/liquidacion', solicitudId ], { state: { sesionComiteSolicitudId, estadoCodigo } } )
      break;
      default:
        console.log( 'No es un tipo de solicitud valido.' );

    };

  };

}
