import { Component, Input, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ContratosModificacionesContractualesService } from '../../../../core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';
import { ProcesosContractualesService } from '../../../../core/_services/procesosContractuales/procesos-contractuales.service';

@Component({
  selector: 'app-tabla-proceso-firmas',
  templateUrl: './tabla-proceso-firmas.component.html',
  styleUrls: ['./tabla-proceso-firmas.component.scss']
})
export class TablaProcesoFirmasComponent implements OnInit {

  dataSource                = new MatTableDataSource();
  @Output() sinData = new EventEmitter<boolean>();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 'fechaSolicitud', 'numeroSolicitud', 'tipoSolicitud', 'estadoRegistro', 'estadoDocumento', 'id' ];
  ELEMENT_DATA    : any[]    = [
    { titulo: 'Número de solicitud', name: 'numeroSolicitud' },
    { titulo: 'Tipo de solicitud', name: 'tipoSolicitud' }
  ];
  dataTable: any[] = [];
  estadoCodigo: string;
  estadoCodigos = {
    enFirmaFiduciaria: '5',
    firmado: '8',
    registrado: '6'
  }

  constructor ( private routes: Router,
                private contratosContractualesSvc: ContratosModificacionesContractualesService,
                private procesosContractualesSvc: ProcesosContractualesService ) {
    this.getGrilla();
  }

  ngOnInit(): void {
  };

  getGrilla () {
    this.contratosContractualesSvc.getGrilla()
      .subscribe( ( resp: any ) => {     
        
        for ( let contrataciones of resp ) {
          if ( contrataciones.contratacion.estadoSolicitudCodigo === this.estadoCodigos.enFirmaFiduciaria ) {
            this.dataTable.push( contrataciones );
          } else if ( contrataciones.contratacion.estadoSolicitudCodigo === this.estadoCodigos.firmado ) {
            this.dataTable.push( contrataciones );
          };
        };
        if ( this.dataTable.length === 0 ) {
          this.sinData.emit( false );
        }
        console.log( resp );
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

      case "Modificación contractual":
        this.routes.navigate( [ '/contratosModificacionesContractuales/modificacionContractual', id ] );
      break;
      default:
        console.log( 'No es una opcion valida.' );

    };

  };

  cambioEstadoRegistrado ( elemento ) {
    elemento.contratacion.estadoSolicitudCodigo = this.estadoCodigos.registrado;
    elemento.estadoCodigo = this.estadoCodigos.registrado

    const pContrato = new FormData();

    pContrato.append( 'contratacionId', `${ elemento.contratacion.contrato[0].contratacionId }` );
    pContrato.append( 'contratoId', `${ elemento.contratacion.contrato[0].contratoId }` );

    this.contratosContractualesSvc.postRegistroTramiteContrato( pContrato, this.estadoCodigos.registrado )
      .subscribe( () => {
        this.dataTable = [];
        this.contratosContractualesSvc.getGrilla()
          .subscribe( ( resp: any ) => {
            
            for ( let contrataciones of resp ) {
              if ( contrataciones.contratacion.estadoSolicitudCodigo === this.estadoCodigos.enFirmaFiduciaria ) {
                this.dataTable.push( contrataciones );
              } else if ( contrataciones.contratacion.estadoSolicitudCodigo === this.estadoCodigos.firmado ) {
                this.dataTable.push( contrataciones );
              };
            };
            if ( this.dataTable.length === 0 ) {
              this.sinData.emit( false );
            };
            console.log( resp );
            this.dataSource = new MatTableDataSource( this.dataTable );

          } )
      } )
  }

};