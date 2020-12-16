import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-control-y-tabla-actuaciones-no-tai',
  templateUrl: './control-y-tabla-actuaciones-no-tai.component.html',
  styleUrls: ['./control-y-tabla-actuaciones-no-tai.component.scss']
})
export class ControlYTablaActuacionesNoTaiComponent implements OnInit {

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
      fechaActualizacion: '20/08/2020',
      actuacion: 'Actuación 1',
      numeroActuacion: '0001',
      estadoRegistro: '1',
      estadoActuacion: '1',
      id: 1
    },
    {
      fechaActualizacion: '21/08/2020',
      actuacion: 'Actuación 2',
      numeroActuacion: '0002',
      estadoRegistro: '2',
      estadoActuacion: '2',
      id: 2
    }
  ];  
  constructor(private router: Router) {
   }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  enviarComiteTecnicoTramAct(id){

  }
  verDetalleEditarActuacion(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleEditarActuacionNoTai',id]);
  }
  eliminarActuacion(id){

  }
  verDetalleActuacion(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleActuacionNoTai',id]);
  }
}
