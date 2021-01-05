import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-control-y-tabla-mesas-trabajo-cc',
  templateUrl: './control-y-tabla-mesas-trabajo-cc.component.html',
  styleUrls: ['./control-y-tabla-mesas-trabajo-cc.component.scss']
})
export class ControlYTablaMesasTrabajoCcComponent implements OnInit {
  public controversiaID = parseInt(localStorage.getItem("controversiaID"));
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
  dataTable: any[] = [];
  constructor(private router: Router, private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.services.GetMesasByControversiaActuacionId(this.controversiaID).subscribe((data:any)=>{
      this.dataTable = data;
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    });
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
