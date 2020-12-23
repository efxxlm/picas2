import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-control-y-tabla-mesas-trabajo-cc',
  templateUrl: './control-y-tabla-mesas-trabajo-cc.component.html',
  styleUrls: ['./control-y-tabla-mesas-trabajo-cc.component.scss']
})
export class ControlYTablaMesasTrabajoCcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaActualizacion',
    'actuacion',
    'numeroActuacion',
    'numeroMesaTrabajo',
    'estadoMesaTrabajo',
    'gestion',
  ];
  dataTable: any[] = [
    {
      fechaActualizacion: '20/08/2020',
      actuacion: 'Actuación 1',
      numeroActuacion: '0001',
      numeroMesaTrabajo: '-----',
      estadoMesaTrabajo: '1',
      id: 1
    },
    {
      fechaActualizacion: '21/08/2020',
      actuacion: 'Actuación 2',
      numeroActuacion: '0002',
      numeroMesaTrabajo: 'MT_002',
      estadoMesaTrabajo: '2',
      id: 2
    },
    {
      fechaActualizacion: '21/08/2020',
      actuacion: 'Actuación 2',
      numeroActuacion: '0003',
      numeroMesaTrabajo: 'MT_003',
      estadoMesaTrabajo: '3',
      id: 3
    }
  ];
  constructor(private router: Router) { }

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
  registrarNuevaMesa(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/registrarNuevaMesaTrabajo',id]);
  }
  verDetalleEditar(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleEditarMesaTrabajo',id]);
  }
  finalizarMesaTrabajo(id){

  }
  verDetalleMesaTrabajo(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleMesaTrabajo',id]);
  }
  actualizarMesaTrabajo(id){
    localStorage.setItem("idMesaTrabajo",id);
    this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarMesaTrabajo']);
  } 
}
