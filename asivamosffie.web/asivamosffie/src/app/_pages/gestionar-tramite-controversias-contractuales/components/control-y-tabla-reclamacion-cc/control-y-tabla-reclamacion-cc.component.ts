import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-control-y-tabla-reclamacion-cc',
  templateUrl: './control-y-tabla-reclamacion-cc.component.html',
  styleUrls: ['./control-y-tabla-reclamacion-cc.component.scss']
})
export class ControlYTablaReclamacionCcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaActualizacion',
    'actuacion',
    'numActuacion',
    'numReclamacion',
    'estadoReclamacion',
    'gestion'
  ];
  dataTable: any[] = [
    {
      fechaActualizacion: '10/08/2020',
      actuacion: "Actuacion 1",
      numActuacion: "ACT Controversia 0001",
      numReclamacion: "---",
      estadoReclamacion: 'Sin registro',
      id: 1
    },
    {
      fechaActualizacion: '10/08/2020',
      actuacion: "Actuacion 2",
      numActuacion: "ACT Controversia 0002",
      numReclamacion: "REC 002",
      estadoReclamacion: 'Enviado a comité técnico',
      id: 2
    },
    {
      fechaActualizacion: '10/08/2020',
      actuacion: "Actuacion 3",
      numActuacion: "ACT Controversia 0003",
      numReclamacion: "REC 003",
      estadoReclamacion: 'Aprobada por comité técnico',
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
  actualizarReclamacionAseguradora(id){
    localStorage.setItem("reclamacionID",id);
    this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarReclamoAseguradora']);
  }
  registrarReclamacionAseguradora(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/registrarReclamacionAseguradora',id]);
  }
  verDetalleEditarReclamacion(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/registrarReclamacionAseguradora',id]);
  }
  enviarReclamacionComiteTecnico(id){

  }
  verDetalleReclamacion(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleReclamacionAseguradora',id]);
  }
}
