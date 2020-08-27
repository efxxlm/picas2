import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabla-gestion-compromisos',
  templateUrl: './tabla-gestion-compromisos.component.html',
  styleUrls: ['./tabla-gestion-compromisos.component.scss']
})
export class TablaGestionCompromisosComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort: MatSort;
  data: any[] = [];
  displayedColumns: string[] = [ 'fechaComite', 'numeroComite', 'compromiso', 'fechaLimiteCumplimiento', 'estadoCompromiso', 'gestion' ];
  ELEMENT_DATA: any[] = [
    {titulo: 'Fecha de comité', name: 'fechaComite'},
    { titulo: 'Número de comité', name: 'numeroComite' },
    { titulo: 'Compromiso', name: 'compromiso' },
    { titulo: 'Fecha limite de cumplimiento', name: 'fechaLimiteCumplimiento' }
  ];

  constructor ( private routes: Router ) { }

  ngOnInit(): void {
    this.getData();
    this.dataSource = new MatTableDataSource( this.data );
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  //getDataTabla
  getData () {

    this.data.push(
      { 
        fechaComite: '24/06/2020',
        numeroComite: 'CT_000001',
        compromiso: 'Realizar seguimiento semanal del cronograma',
        fechaLimiteCumplimiento: '30/06/2020',
        fechaRegistro: '29/06/2020',
        gestionRealizada: 'Se realiza el seguimiento semanal con avance en cada una de las tareas propuestas para estos días.',
        estadoCompromiso: {
          sinAvance: false,
          enProceso: false,
          finalizado: true
        }
      },
      { 
        fechaComite: '24/06/2020',
        numeroComite: 'CT_000001',
        compromiso: 'Cargar soportes semanales de avances',
        fechaLimiteCumplimiento: '01/07/2020',
        fechaRegistro: '29/06/2020',
        gestionRealizada: 'Se solicitaron los reportes pertinentes y se han cargado 4/6 aun nos hacen falta tener dos para completar la tarea',
        estadoCompromiso: {
          sinAvance: false,
          enProceso: true,
          finalizado: false
        }
      },
      { 
        fechaComite: '22/06/2020',
        numeroComite: 'CT_000003',
        compromiso: 'Conformación de equipo de apoyo',
        fechaLimiteCumplimiento: '03/07/2020',
        fechaRegistro: '',
        gestionRealizada: '',
        estadoCompromiso: {
          sinAvance: true,
          enProceso: false,
          finalizado: false
        }
      }
    );

  }

  //Reportar Avance
  reportProgress ( compromiso: any ) {
    this.routes.navigate( [ '/compromisosActasComite/reporteAvanceCompromiso' ], { state: { compromiso } } )
  }

}
