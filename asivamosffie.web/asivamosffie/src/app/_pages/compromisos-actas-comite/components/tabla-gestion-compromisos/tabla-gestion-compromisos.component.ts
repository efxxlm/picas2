import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { CompromisosActasComiteService } from '../../../../core/_services/compromisosActasComite/compromisos-actas-comite.service';

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
  displayedColumns: string[] = [ 'fechaOrdenDia', 'numeroComite', 'tarea', 'fechaCumplimiento', 'estadoCompromiso', 'gestion' ];
  ELEMENT_DATA: any[] = [
    { titulo: 'Número de comité', name: 'numeroComite' },
    { titulo: 'Compromiso', name: 'tarea' }
  ];

  constructor ( private routes: Router,
                private compromisosSvc: CompromisosActasComiteService ) { }

  ngOnInit(): void {
    this.getData();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  //getDataTabla
  getData () {

    this.compromisosSvc.getGrillaCompromisos()
      .subscribe( ( resp: any[] ) => {
        console.log( resp );
        this.dataSource = new MatTableDataSource( resp );
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      } );

  };

  //Reportar Avance
  reportProgress ( sesionComiteTecnicoCompromisoId: number ) {
    this.routes.navigate( [ '/compromisosActasComite/reporteAvanceCompromiso', sesionComiteTecnicoCompromisoId ] )
  }

}
