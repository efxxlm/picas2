import { Component, Input, OnInit, ViewChild } from '@angular/core';
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
  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 'fechaSolicitud', 'numeroSolicitud', 'tipoSolicitud', 'estadoDelRegistro', 'id' ];
  estadoCodigo: string;

  constructor ( private routes: Router ) {
  }

  ngOnInit() {
    this.$data.subscribe( resp => {
      resp.push(
        {
          "numeroSolicitud": "CO_003",
          "fechaSolicitud": "2020-08-19T22:47:16",
          "tipoSolicitud": "Modificación contractual",
          "estadoDelRegistro": "Incompleto",
          "estadoRegistro": false,
          "sesionComiteSolicitudId": 14,
          "tipoSolicitudCodigo": "2",
          "solicitudId": 6,
          "fechaCreacion": "2020-09-01T18:15:13.377",
          "usuarioCreacion": "diegoaes@yopmail.com",
          "fechaModificacion": "2020-09-04T11:29:02.81",
          "comiteTecnicoId": 18,
          "estadoCodigo": "2",
          "generaCompromiso": true,
          "cantCompromisos": 1,
          "eliminado": false,
          "requiereVotacion": true,
          "sesionSolicitudCompromiso": [],
          "sesionSolicitudObservacionProyecto": [],
          "sesionSolicitudVoto": []
        },
        {
          "numeroSolicitud": "SL_001",
          "fechaSolicitud": "2020-08-19T22:47:16",
          "tipoSolicitud": "Liquidación",
          "estadoDelRegistro": "Incompleto",
          "estadoRegistro": false,
          "sesionComiteSolicitudId": 14,
          "tipoSolicitudCodigo": "2",
          "solicitudId": 6,
          "fechaCreacion": "2020-09-01T18:15:13.377",
          "usuarioCreacion": "diegoaes@yopmail.com",
          "fechaModificacion": "2020-09-04T11:29:02.81",
          "comiteTecnicoId": 18,
          "estadoCodigo": "2",
          "generaCompromiso": true,
          "cantCompromisos": 1,
          "eliminado": false,
          "requiereVotacion": true,
          "sesionSolicitudCompromiso": [],
          "sesionSolicitudObservacionProyecto": [],
          "sesionSolicitudVoto": []
        }
      )
      console.log( resp );
      this.dataSource = new MatTableDataSource( resp );
      this.dataSource.paginator              = this.paginator;
      this.dataSource.sort                   = this.sort;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    } );
  };

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  gestionar ( tipoSolicitud: string, solicitudId: number ) {
    
    switch ( tipoSolicitud ) {

      case 'Contratación':
        this.routes.navigate( [ '/procesosContractuales/contratacion', solicitudId ] );
      break;
      case 'Modificación contractual':
        /*Ejemplo para los 3 tipos de modificaciones contractuales*/
        this.routes.navigate( [ '/procesosContractuales/modificacionContractual', solicitudId ] );
        //this.routes.navigate( [ '/procesosContractuales/modificacionContractual', solicitudId ], { state: { suspension: true, reinicio: false } } );
        //this.routes.navigate( [ '/procesosContractuales/modificacionContractual', solicitudId ], { state: { suspension: false, reinicio: true } } )
      break;
      case 'Liquidación':
        this.routes.navigate( [ '/procesosContractuales/liquidacion', solicitudId ] );
      break;
      default:
        console.log( 'No es un tipo de solicitud valido.' );

    };

  };

};