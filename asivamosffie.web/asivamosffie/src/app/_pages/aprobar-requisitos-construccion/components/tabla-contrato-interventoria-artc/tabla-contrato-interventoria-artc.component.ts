import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { FaseDosAprobarConstruccionService } from 'src/app/core/_services/faseDosAprobarConstruccion/fase-dos-aprobar-construccion.service';

@Component({
  selector: 'app-tabla-contrato-interventoria-artc',
  templateUrl: './tabla-contrato-interventoria-artc.component.html',
  styleUrls: ['./tabla-contrato-interventoria-artc.component.scss']
})
export class TablaContratoInterventoriaArtcComponent implements OnInit {

  dataSource = new MatTableDataSource();
  tipoContratoInterventoria: string = '2';
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 
    'fechaAprobacion',
    'numeroContratoObra',
    'proyectosAsociados',
    'proyectosAprobados',
    'proyectosPendientes',
    'estadoRequisito',
    'gestion'
  ];

  constructor ( private routes: Router,
                private faseDosAprobarConstruccionSvc: FaseDosAprobarConstruccionService )
  { }

  ngOnInit(): void {
    this.faseDosAprobarConstruccionSvc.getContractsGrid( this.tipoContratoInterventoria )
      .subscribe(
        response => {
          this.dataSource                        = new MatTableDataSource( response );
          this.dataSource.paginator              = this.paginator;
          this.dataSource.sort                   = this.sort;
          this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        }
      )
  };

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  getForm ( id: number ) {
    this.routes.navigate( [ '/aprobarRequisitosTecnicosConstruccion/verificarRequisitosInicioInterventoria', id ] )
  };

  aprobarInicio ( id: number ) {
    console.log( id );
  };

  verDetalle ( id: number ) {
    this.routes.navigate( [ '/aprobarRequisitosTecnicosConstruccion/verDetalleInterventoria', id ] )
  }

}
