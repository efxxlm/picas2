import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';

@Component({
  selector: 'app-tabla-contratos-interventoria-vrtc',
  templateUrl: './tabla-contratos-interventoria-vrtc.component.html',
  styleUrls: ['./tabla-contratos-interventoria-vrtc.component.scss']
})
export class TablaContratosInterventoriaVrtcComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort          : MatSort;
  displayedColumns: string[] = [ 
    'fechaAprobacion',
    'numeroContrato',
    'cantidadProyectosAsociados',
    'cantidadProyectosRequisitosAprobados',
    'cantidadProyectosRequisitosPendientes',
    'estadoNombre',
    'gestion'
  ];
  
  constructor ( private routes: Router,
                private faseUnoConstruccionSvc: FaseUnoConstruccionService  ) 
    { }

  ngOnInit(): void {
    this.faseUnoConstruccionSvc.getContractsGridApoyoInterventoria().subscribe( respuesta => {
      this.dataSource                        = new MatTableDataSource( respuesta );
      this.dataSource.paginator              = this.paginator;
      this.dataSource.sort                   = this.sort;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    })
    
    
  };

  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  getForm ( id: number, fechaPoliza: string ) {
    this.routes.navigate( [ '/verificarRequisitosTecnicosConstruccion/verificarRequisitosInicioInterventoria', id ], { state: { fechaPoliza } } )
  };

  aprobarInicio ( id: number ) {
    console.log( id );
  };

  verDetalle ( id: number ) {
    this.routes.navigate( [ '/verificarRequisitosTecnicosConstruccion/verDetalleInterventoria', id ] )
  }

}
