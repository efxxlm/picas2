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
      let dataTable = [];
      dataTable.push(
        {
          "numeroSolicitud": "PI_008",
          "fechaSolicitud": "2020-08-19T22:47:16",
          "tipoSolicitud": "Contratación",
          "estadoDelRegistro": "Completo",
          "estadoRegistro": true,
          "sesionComiteSolicitudId": 14,
          "tipoSolicitudCodigo": "2",
          "solicitudId": 6,
          "fechaCreacion": "2020-09-01T18:15:13.377",
          "usuarioCreacion": "diegoaes@yopmail.com",
          "fechaModificacion": "2020-09-04T11:29:02.81",
          "comiteTecnicoId": 18,
          "estadoCodigo": "5",
          "generaCompromiso": true,
          "cantCompromisos": 1,
          "eliminado": false,
          "requiereVotacion": true,
          "sesionSolicitudCompromiso": [],
          "sesionSolicitudObservacionProyecto": [],
          "sesionSolicitudVoto": []
        },
        {
          "numeroSolicitud": "CO_003",
          "fechaSolicitud": "2020-08-19T22:47:16",
          "tipoSolicitud": "Modificación contractual",
          "estadoDelRegistro": "Completo",
          "estadoRegistro": true,
          "sesionComiteSolicitudId": 14,
          "tipoSolicitudCodigo": "2",
          "solicitudId": 6,
          "fechaCreacion": "2020-09-01T18:15:13.377",
          "usuarioCreacion": "diegoaes@yopmail.com",
          "fechaModificacion": "2020-09-04T11:29:02.81",
          "comiteTecnicoId": 18,
          "estadoCodigo": "5",
          "generaCompromiso": true,
          "cantCompromisos": 1,
          "eliminado": false,
          "requiereVotacion": true,
          "sesionSolicitudCompromiso": [],
          "sesionSolicitudObservacionProyecto": [],
          "sesionSolicitudVoto": []
        },
        {
          "numeroSolicitud": "CO_013",
          "fechaSolicitud": "2020-08-19T22:47:16",
          "tipoSolicitud": "Modificación contractual",
          "estadoDelRegistro": "Completo",
          "estadoRegistro": true,
          "sesionComiteSolicitudId": 14,
          "tipoSolicitudCodigo": "2",
          "solicitudId": 6,
          "fechaCreacion": "2020-09-01T18:15:13.377",
          "usuarioCreacion": "diegoaes@yopmail.com",
          "fechaModificacion": "2020-09-04T11:29:02.81",
          "comiteTecnicoId": 18,
          "estadoCodigo": "5",
          "generaCompromiso": true,
          "cantCompromisos": 1,
          "eliminado": false,
          "requiereVotacion": true,
          "sesionSolicitudCompromiso": [],
          "sesionSolicitudObservacionProyecto": [],
          "sesionSolicitudVoto": []
        }
      )

      this.dataSource = new MatTableDataSource( dataTable );
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
        this.routes.navigate( [ '/procesosContractuales/contratacion', id ], { state: { verDetalle: true } } )
      break;
      case 'Modificación contractual':
        /*Ejemplo para los 3 tipos de modificaciones contractuales*/
        //this.routes.navigate( [ '/procesosContractuales/modificacionContractual', id ], { state: { verDetalle: true } } )
        //this.routes.navigate( [ '/procesosContractuales/modificacionContractual', id ], { state: { verDetalle: true } } );
        this.routes.navigate( [ '/procesosContractuales/modificacionContractual', id ], { state: { verDetalle: true } } )
      break;
      case 'Liquidación':
        this.routes.navigate( [ '/procesosContractuales/liquidacion', id ], { state: { verDetalle: true } } )
      break;
      default:
        console.log( 'No es un tipo de solicitud valido.' );

    };

  };

}
