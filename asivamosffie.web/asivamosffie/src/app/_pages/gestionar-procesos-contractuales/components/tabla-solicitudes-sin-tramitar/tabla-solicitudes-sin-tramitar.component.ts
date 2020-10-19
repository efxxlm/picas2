import { Component, Input, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { GrillaProcesosContractuales } from 'src/app/_interfaces/procesosContractuales.interface';
import { ProcesosContractualesService } from '../../../../core/_services/procesosContractuales/procesos-contractuales.service';
import { Observable } from 'rxjs';
import { DataSolicitud } from '../../../../_interfaces/procesosContractuales.interface';

@Component({
  selector: 'app-tabla-solicitudes-sin-tramitar',
  templateUrl: './tabla-solicitudes-sin-tramitar.component.html',
  styleUrls: ['./tabla-solicitudes-sin-tramitar.component.scss']
})
export class TablaSolicitudesSinTramitarComponent implements OnInit {

  @Input() $data: Observable<GrillaProcesosContractuales[]>;
  @Output() estadoAcordeon = new EventEmitter<string>();
  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 'fechaSolicitud', 'numeroSolicitud', 'tipoSolicitud', 'estadoDelRegistro', 'id' ];
  estadoCodigo: string;
  estadoCodigoFiduciaria: string = "9";
  estadoCodigos = {
    aprobadoCf: '13'
  };

  constructor ( private routes: Router,
                private procesosContractualesSvc: ProcesosContractualesService ) {
  }

  ngOnInit() {
    this.getDataGrilla();
  };

  getDataGrilla () {
    this.$data.subscribe( resp => {
      let dataTable = [];
      let conTrue = 0;
      let conFalse = 0;
      
      for ( let solicitud of resp ) {

        if ( solicitud.estadoCodigo === this.estadoCodigos.aprobadoCf ) {

          ( solicitud.estadoRegistro ) ? conTrue+=1 : conFalse+=1;

          dataTable.push( solicitud );
        };
      }

      if ( conTrue === dataTable.length ) {
        this.estadoAcordeon.emit( 'completo' );
      } else if ( conFalse === dataTable.length ) {
        this.estadoAcordeon.emit( 'sin-diligenciar' );
      } else if ( conTrue > conFalse || conTrue < conFalse ) {
        this.estadoAcordeon.emit( 'en-proceso' );
      };

      this.dataSource = new MatTableDataSource( dataTable );
      this.dataSource.paginator              = this.paginator;
      this.dataSource.sort                   = this.sort;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    } );
  }

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  gestionar ( tipoSolicitud: string, solicitudId: number, sesionComiteSolicitudId: number, estadoCodigo: string ) {
    
    console.log( sesionComiteSolicitudId );

    switch ( tipoSolicitud ) {

      case 'Contratación':
        this.routes.navigate( [ '/procesosContractuales/contratacion', solicitudId ], { state: { sesionComiteSolicitudId, estadoCodigo } } );
      break;
      case 'Modificación contractual':
        /*Ejemplo para los 3 tipos de modificaciones contractuales*/
        this.routes.navigate( [ '/procesosContractuales/modificacionContractual', solicitudId ], { state: { sesionComiteSolicitudId, estadoCodigo } } );
        //this.routes.navigate( [ '/procesosContractuales/modificacionContractual', solicitudId ], { state: { suspension: true, reinicio: false, sesionComiteSolicitudId, estadoCodigo } } );
        //this.routes.navigate( [ '/procesosContractuales/modificacionContractual', solicitudId ], { state: { suspension: false, reinicio: true, sesionComiteSolicitudId, estadoCodigo } } )
      break;
      case 'Liquidación':
        this.routes.navigate( [ '/procesosContractuales/liquidacion', solicitudId ], { state: { sesionComiteSolicitudId, estadoCodigo } } );
      break;
      default:
        console.log( 'No es un tipo de solicitud valido.' );

    };

  };

  sendCambioTramite ( elemento: any ) {
    
    elemento.estadoCodigo = this.estadoCodigoFiduciaria;

    this.procesosContractualesSvc.sendCambioTramite( elemento )
      .subscribe( console.log );

  }

};