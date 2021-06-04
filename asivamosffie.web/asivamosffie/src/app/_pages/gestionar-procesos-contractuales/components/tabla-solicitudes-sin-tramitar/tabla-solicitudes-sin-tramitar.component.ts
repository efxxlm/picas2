import { Component, Input, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { GrillaProcesosContractuales } from 'src/app/_interfaces/procesosContractuales.interface';
import { ProcesosContractualesService } from '../../../../core/_services/procesosContractuales/procesos-contractuales.service';
import { Observable } from 'rxjs';
import { DataSolicitud } from '../../../../_interfaces/procesosContractuales.interface';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

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
  enviarFiduciaria: string = "4";
  estadoCodigos = {
    aprobadoCf: '13'
  };

  constructor ( private routes: Router,
                private procesosContractualesSvc: ProcesosContractualesService,
                private dialog: MatDialog )
  {
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
          this.enviarFiduciaria = '4';
          solicitud.estadoRegistro === true ? conTrue+=1 : conFalse+=1;

          dataTable.push( solicitud );
        };

        if (solicitud.tipoSolicitud === 'Novedad Contractual' && (solicitud.estadoCodigo === '12' || solicitud.estadoCodigo === '19')){
          this.enviarFiduciaria = '24';
          solicitud.estadoRegistro === true ? conTrue+=1 : conFalse+=1;

          dataTable.push( solicitud );
        }
        if (solicitud.tipoSolicitud === 'Liquidación Contractual' && (solicitud.estadoCodigo === '21')){
          this.enviarFiduciaria = '6';
          solicitud.estadoRegistro === true ? conTrue+=1 : conFalse+=1;

          dataTable.push( solicitud );
        }
      }

      if ( conTrue === dataTable.length ) {
        this.estadoAcordeon.emit( 'completo' );
      }
      if ( conFalse === dataTable.length ) {
        this.estadoAcordeon.emit( 'sin-diligenciar' );
      }
      if ( conTrue > conFalse || conTrue < conFalse ) {
        this.estadoAcordeon.emit( 'en-proceso' );
      };

      if ( conTrue > 0 && conFalse > 0 && conTrue + conFalse === dataTable.length ) {
        this.estadoAcordeon.emit( 'en-proceso' );
      }

      if ( dataTable.length > 0 ) {
        dataTable.forEach( registro => registro.fechaSolicitud !== undefined ? registro.fechaSolicitud = registro.fechaSolicitud.split('T')[0].split('-').reverse().join('/') : '---' );
      }

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

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  }

  gestionar ( tipoSolicitud: string, solicitudId: number, sesionComiteSolicitudId: number, estadoCodigo: string ) {

    console.log( sesionComiteSolicitudId, estadoCodigo, tipoSolicitud );

    switch ( tipoSolicitud ) {

      case 'Contratación':
        this.routes.navigate( [ '/procesosContractuales/contratacion', solicitudId ], { state: { sesionComiteSolicitudId, estadoCodigo } } );
      break;
      case 'Novedad Contractual':
        /*Ejemplo para los 3 tipos de modificaciones contractuales*/
        this.routes.navigate( [ '/procesosContractuales/modificacionContractual', solicitudId ], { state: { sesionComiteSolicitudId, estadoCodigo } } );
        //this.routes.navigate( [ '/procesosContractuales/modificacionContractual', solicitudId ], { state: { suspension: true, reinicio: false, sesionComiteSolicitudId, estadoCodigo } } );
        //this.routes.navigate( [ '/procesosContractuales/modificacionContractual', solicitudId ], { state: { suspension: false, reinicio: true, sesionComiteSolicitudId, estadoCodigo } } )
      break;
      case 'Liquidación Contractual':
        this.routes.navigate( [ '/procesosContractuales/liquidacion', solicitudId ], { state: { sesionComiteSolicitudId, estadoCodigo } } );
      break;
      default:
        console.log( 'No es un tipo de solicitud valido.' );

    };

  };

  sendCambioTramite ( elemento: any ) {
    if(elemento.tipoSolicitud === 'Liquidación Contractual'){
      this.enviarFiduciaria = '6';
    }else if(elemento.tipoSolicitud === 'Novedad Contractual'){
      this.enviarFiduciaria = '24';
    }else{
      this.enviarFiduciaria = '4';
    }
    this.procesosContractualesSvc.sendCambioTramite( this.enviarFiduciaria, elemento.sesionComiteSolicitudId, elemento.solicitudId )
      .subscribe(
        response => {
          this.openDialog( '', `<b>${response.message}</b>` );
          this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
            () => this.routes.navigate( [ '/procesosContractuales' ] )
          );
        },
        err => this.openDialog( '', `<b>${err.message}</b>` )
      );

  }

};
