import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-actualizar-tramite-racc',
  templateUrl: './actualizar-tramite-racc.component.html',
  styleUrls: ['./actualizar-tramite-racc.component.scss']
})
export class ActualizarTramiteRaccComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaActualizacion',
    'actuacion',
    'numeroActuacion',
    'estadoRegistro',
    'estadoActuacion',
    'gestion',
  ];
  dataTable: any[] = [
    {
      fechaActualizacion: '17/08/2020',
      actuacion: 'Actuación 1',
      numeroActuacion: 'ACT_derivada0001',
      estadoRegistro: 'Completo',
      estadoActuacion: 'Cumplida',
      gestion: 1,
    }
  ]
  constructor(private router: Router) { }
  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  irARegistro(){
    this.router.navigate(['/registrarActuacionesControversiasContractuales/registrarActuacionDerivada']);
  }
}
