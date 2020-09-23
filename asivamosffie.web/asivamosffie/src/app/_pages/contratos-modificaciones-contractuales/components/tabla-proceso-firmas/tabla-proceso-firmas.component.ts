import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ContratosModificacionesContractualesService } from '../../../../core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';

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
  estadoCodigo: string;
  estadoCodigos = {
    enFirmaFiduciaria: '11',
    firmado: '12',
    registrado: '13'
  }

  constructor ( private routes: Router,
                private contratosContractualesSvc: ContratosModificacionesContractualesService ) { }

  ngOnInit(): void {
    this.getGrilla();
  };

  getGrilla () {
    this.contratosContractualesSvc.getGrilla()
      .subscribe( ( resp: any ) => {
        const dataTable = [];
        
        for ( let contratacion of resp ) {
          if ( contratacion.estadoCodigo === this.estadoCodigos.enFirmaFiduciaria ) {
            dataTable.push( contratacion );
            this.estadoCodigo = this.estadoCodigos.firmado;
          };
          if ( contratacion.estadoCodigo === this.estadoCodigos.firmado ) {
            dataTable.push( contratacion );
            this.estadoCodigo = this.estadoCodigos.firmado;
          };
        };

        console.log( resp );
        this.dataSource                        = new MatTableDataSource( dataTable );
        this.dataSource.paginator              = this.paginator;
        this.dataSource.sort                   = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      } );
  }

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  gestionar ( tipoSolicitud: string, id: number ) {

    switch ( tipoSolicitud ) {

      case "Contratación":
        this.routes.navigate( [ '/contratosModificacionesContractuales/contratacion', id ], { state: { estadoCodigo: this.estadoCodigo } } );
      break;

      case "Modificación contractual":
        this.routes.navigate( [ '/contratosModificacionesContractuales/modificacionContractual', id ] );
      break;
      default:
        console.log( 'No es una opcion valida.' );

    };

  };

  cambioEstadoRegistrado ( elemento ) {
    console.log( elemento, this.estadoCodigos.registrado );
  }

};