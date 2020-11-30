import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ContratosModificacionesContractualesService } from '../../../../core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';

@Component({
  selector: 'app-tabla-sin-registro-contrato',
  templateUrl: './tabla-sin-registro-contrato.component.html',
  styleUrls: ['./tabla-sin-registro-contrato.component.scss']
})
export class TablaSinRegistroContratoComponent implements OnInit {

  dataTable: any[] = [];
  @Output() sinData = new EventEmitter<boolean>();
  @Output() estadoSemaforo = new EventEmitter<string>();
  dataSource                = new MatTableDataSource();
  estadoCodigo: string;
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 'fechaSolicitud', 'numeroSolicitud', 'tipoSolicitud', 'estadoRegistro', 'estadoDocumento', 'solicitudId' ];
  ELEMENT_DATA    : any[]    = [
    { titulo: 'Número de solicitud', name: 'numeroSolicitud' },
    { titulo: 'Tipo de solicitud', name: 'tipoSolicitud' }
  ];
  estadoCodigos = {
    enviadaFiduciaria: '4',
    enRevision: '2'
  }

  constructor ( private routes: Router,
                private contratosContractualesSvc: ContratosModificacionesContractualesService ) {
    this.getGrilla();
  };

  ngOnInit(): void {
  };

  getGrilla () {
    this.contratosContractualesSvc.getGrilla()
      .subscribe( ( resp: any ) => {
        let sinDiligenciar = 0;
        let enProceso = 0;
        for ( let contrataciones of resp ) {
          if ( contrataciones.estadoCodigo === this.estadoCodigos.enviadaFiduciaria ) {
            this.dataTable.push( contrataciones );
            sinDiligenciar++;
          };
          if ( contrataciones.estadoCodigo === this.estadoCodigos.enRevision ) {
            this.dataTable.push( contrataciones );
            enProceso++;
          };
        };

        if ( sinDiligenciar === this.dataTable.length ) {
          this.estadoSemaforo.emit( 'sin-diligenciar' );
        };

        if ( enProceso === this.dataTable.length ) {
          this.estadoSemaforo.emit( 'en-proceso' );
        };

        if ( ( sinDiligenciar > 0 && sinDiligenciar < this.dataTable.length ) && ( enProceso > 0 && enProceso < this.dataTable.length ) ) {
          this.estadoSemaforo.emit( 'sin-diligenciar' );
        };

        if ( this.dataTable.length === 0 ) {
          this.sinData.emit( false );
          this.estadoSemaforo.emit( 'completo' );
          return;
        };

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

};