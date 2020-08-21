import { DatePipe } from '@angular/common';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ColumnasTabla } from 'src/app/_interfaces/comiteFiduciario.interfaces';
import { ComiteFiduciario } from '../../../../_interfaces/comiteFiduciario.interfaces';


@Component({
  selector: 'app-sesiones-convocadas',
  templateUrl: './sesiones-convocadas.component.html',
  styleUrls: ['./sesiones-convocadas.component.scss']
})
export class SesionesConvocadasComponent implements OnInit {

  dataSource = new MatTableDataSource();
  //Decoradores ViewChild para controlar "MatPaginator" y "MatSort" del componente
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true }) sort: MatSort;
  //Inputs para recibir la data de la tabla
  displayedColumns: string[] = [ 'fechaComite', 'numeroComite', 'estadoComite', 'gestion' ];
  columnas: ColumnasTabla[] = [
    { titulo: 'Fecha de comité', name: 'fechaComite' },
    { titulo: 'Número de comité', name: 'numeroComite' }
  ];

  constructor ( private routes: Router,
                private datepipe: DatePipe ) { }

  ngOnInit(): void {
    this.getOrden();
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  };

  getOrden (  ) {
    this.dataSource = new MatTableDataSource( [
      {
        solicitudesSeleccionadas: [
          {
            nombreSesion: 'CT_00001',
            fecha: '24/06/2020',
            data: [
              {
                fechaSolicitud: this.datepipe.transform( new Date(), 'dd-MM-yyyy'),
                numeroSolicitud: 'SC0005',
                tipoSolicitud: 'Evaluación de proceso de selección'
              }
            ]
          }
        ],
        numeroComite: 'CF_00001',
        estadoComite: true,
        proposiciones: '',
        temas: [
          {
            numeroTema: 1,
            tiempoIntervencion: 45,
            responsable: 'Dirección técnica',
            urlSoporte: 'DestoktopFFIE/archivosgenerales/jmgarzon/comite/temas/revisiondeterminostecnicos',
            temaTratar: 'Revisión de terminos técnicos'
          }
        ],
        fechaComite: this.datepipe.transform( new Date(), 'dd-MM-yyyy')
      },
      {
        solicitudesSeleccionadas: [],
        numeroComite: 'CF_00002',
        estadoComite: true,
        proposiciones: '',
        temas: [],
        fechaComite: this.datepipe.transform( new Date(), 'dd-MM-yyyy')
      }
    ] );
  }

  applyFilter ( event ) {
    console.log( event );
  };

  registrarSesion ( sesion: ComiteFiduciario ) {
    this.routes.navigate( ['/comiteFiduciario/registrarSesion'], { state: { sesion } } );
  };


}
