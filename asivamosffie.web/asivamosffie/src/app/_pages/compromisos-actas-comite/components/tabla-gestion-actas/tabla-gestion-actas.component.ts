import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabla-gestion-actas',
  templateUrl: './tabla-gestion-actas.component.html',
  styleUrls: ['./tabla-gestion-actas.component.scss']
})
export class TablaGestionActasComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort: MatSort;
  data: any[] = [];
  displayedColumns: string[] = [ 'fechaComite', 'numeroComite', 'estadoActa', 'gestion' ];
  ELEMENT_DATA: any[] = [
    {titulo: 'Fecha de comité', name: 'fechaComite'},
    { titulo: 'Número de comité', name: 'numeroComite' }
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
        estadoActa: {
          sinRevision: true,
          devuelta: false,
          aprobada: false
        },
        estadoCompromiso: {
          sinAvance: false,
          enProceso: false,
          finalizado: true
        }
      }
    );

  }

  revisarActa ( acta: any ) {
    this.routes.navigate( [ '/compromisosActasComite/revisionActa' ], { state: { acta } } )
  }

}
