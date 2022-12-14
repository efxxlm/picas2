import { Component, Input, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ContratosModificacionesContractualesService } from '../../../../core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';
import { ProcesosContractualesService } from '../../../../core/_services/procesosContractuales/procesos-contractuales.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import moment from 'moment';

@Component({
  selector: 'app-tabla-proceso-firmas',
  templateUrl: './tabla-proceso-firmas.component.html',
  styleUrls: ['./tabla-proceso-firmas.component.scss']
})
export class TablaProcesoFirmasComponent implements OnInit {

  dataSource                = new MatTableDataSource();
  @Output() sinData = new EventEmitter<boolean>();
  @Output() estadoAcordeon = new EventEmitter<string>();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort: MatSort;
  displayedColumns: string[] = [ 'fechaSolicitud', 'numeroSolicitud', 'tipoSolicitud', 'estadoRegistro', 'estadoDocumento', 'id' ];
  ELEMENT_DATA: any[]    = [
    { titulo: 'Número de solicitud', name: 'numeroSolicitud' },
    { titulo: 'Tipo de solicitud', name: 'tipoSolicitud' }
  ];
  dataTable: any[] = [];
  estadoCodigo: string;
  estadoCodigos = {
    enFirmaFiduciaria: '5',
    enFirmaContratista: '10',
    firmado: '8',
    registrado: '6',
    firmadoNovedad: '25',
    registradoNovedad: '26',
  };

  constructor(
    private routes: Router,
    private contratosContractualesSvc: ContratosModificacionesContractualesService,
    private dialog: MatDialog,
    private procesosContractualesSvc: ProcesosContractualesService ) {
    this.getGrilla();
  }

  ngOnInit(): void {
  }

  getGrilla() {
    this.contratosContractualesSvc.getGrilla()
      .subscribe( ( resp: any ) => {
        resp.forEach(element => {
          if((element.contratacion.estadoSolicitudCodigo === this.estadoCodigos.enFirmaFiduciaria)
          || (element.contratacion.estadoSolicitudCodigo === this.estadoCodigos.enFirmaContratista)
          || (element.estadoCodigo === this.estadoCodigos.firmadoNovedad && element.estadoDelRegistro == 'Incompleto')){
              element.registroCompletoNew = false;
          }else if(element.contratacion.estadoSolicitudCodigo === this.estadoCodigos.firmado
            || (element.estadoCodigo === this.estadoCodigos.firmadoNovedad && element.estadoDelRegistro == 'Completo')){
              element.registroCompletoNew = true;
          }else{
              element.registroCompletoNew = false;
          }
        });
        let firmado = 0;
        let firmaContratista = 0;
        for ( const contrataciones of resp ) {
          if ( contrataciones.estadoCodigo === this.estadoCodigos.enFirmaFiduciaria) {
            this.dataTable.push( contrataciones );
          } else if ( contrataciones.estadoCodigo === this.estadoCodigos.firmado || contrataciones.estadoCodigo === this.estadoCodigos.firmadoNovedad) {
            this.dataTable.push( contrataciones );
            firmado++;
          } else if ( contrataciones.estadoCodigo === this.estadoCodigos.enFirmaContratista ) {
            this.dataTable.push( contrataciones );
            firmaContratista++;
          }
        }
        if ( this.dataTable.length === 0 ) {
          this.sinData.emit( false );
          this.estadoAcordeon.emit( 'completo' );
          return;
        }

        if ( firmaContratista === this.dataTable.length ) {
          this.estadoAcordeon.emit( 'sin-diligenciar' );
        } else if ( firmado === this.dataTable.length ) {
          this.estadoAcordeon.emit( 'completo' );
        } else if ( firmaContratista < this.dataTable.length || firmado < this.dataTable.length ) {
          this.estadoAcordeon.emit( 'en-proceso' );
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

  applyFilter( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  gestionar( tipoSolicitud: string, id: number, estadoCodigo: string ) {

    switch ( tipoSolicitud ) {

      case 'Contratación':
        this.routes.navigate( [ '/contratosModificacionesContractuales/contratacion', id ], { state: { estadoCodigo } } );
        break;

      case "Novedad Contractual":
          this.routes.navigate( [ '/contratosModificacionesContractuales/modificacionContractual', id ] );
        break;
      default:
        console.log( 'No es una opcion valida.' );

    }

  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
      this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
        () => this.routes.navigate( [ '/contratosModificacionesContractuales' ] )
      );
    });
  }

  cambioEstadoRegistrado( elemento ) {
    const registro = elemento;
    if(elemento.tipoSolicitud === 'Novedad Contractual'){
      this.contratosContractualesSvc.changeStateTramiteNovedad( elemento.solicitudId )
        .subscribe(
          response => {
            this.openDialog( '', `<b>${response.message}</b>` );
          },
          err => this.openDialog( '', `<b>${err.message}</b>` )
        );
    }else{
      this.dataSource = new MatTableDataSource();
      registro.contratacion.estadoSolicitudCodigo = this.estadoCodigos.registrado;
      registro.estadoCodigo = this.estadoCodigos.registrado;

      const pContrato = new FormData();

      pContrato.append( 'contratacionId', `${ registro.contratacion.contrato[0].contratacionId }` );
      pContrato.append( 'contratoId', `${ registro.contratacion.contrato[0].contratoId }` );

      this.contratosContractualesSvc.postRegistroTramiteContrato( pContrato, this.estadoCodigos.registrado )
        .subscribe(
          response => {
            this.openDialog( '', `<b>${response.message}</b>` );
          },
          err => this.openDialog( '', `<b>${err.message}</b>` )
        );
    }
  }

}
