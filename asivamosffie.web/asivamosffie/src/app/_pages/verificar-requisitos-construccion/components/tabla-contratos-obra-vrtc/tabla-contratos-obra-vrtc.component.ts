import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';

@Component({
  selector: 'app-tabla-contratos-obra-vrtc',
  templateUrl: './tabla-contratos-obra-vrtc.component.html',
  styleUrls: ['./tabla-contratos-obra-vrtc.component.scss']
})
export class TablaContratosObraVrtcComponent implements OnInit {
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
  dataTable: any [] = [];

  constructor ( private routes: Router,
                private faseUnoConstruccionSvc: FaseUnoConstruccionService,
                private dialog : MatDialog ) { }

  ngOnInit(): void {
    this.faseUnoConstruccionSvc.GetContractsGridApoyoObra().subscribe( respuesta => {
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
    this.routes.navigate( [ '/verificarRequisitosTecnicosConstruccion/verificarRequisitosInicio', id ], { state: { fechaPoliza } } )
  };

  aprobarInicio ( id: number ) {
    console.log( id );
  };

  verDetalle ( id: number, fechaPoliza: string  ) {
    this.routes.navigate( [ '/verificarRequisitosTecnicosConstruccion/verDetalleObra', id ], { state: { fechaPoliza } } )
  }
}
