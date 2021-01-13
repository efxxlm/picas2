import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-control-y-tabla-actuacion-mt',
  templateUrl: './control-y-tabla-actuacion-mt.component.html',
  styleUrls: ['./control-y-tabla-actuacion-mt.component.scss']
})
export class ControlYTablaActuacionMtComponent implements OnInit {

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
  dataTable: any[] = [];  
  public idMesaTrabajo = parseInt(localStorage.getItem("idMesaTrabajo"));
  constructor(private router: Router,private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.services.GetListGrillaControversiaActuacion(this.idMesaTrabajo).subscribe((data:any)=>{
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
  finalizarMesaDeTrabajo(id){

  }
  verDetalleEditarMTActuacion(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleEditarMesaTrabajoAct',id]);
  }
  eliminarMTActuacion(id){

  }
  verDetalleMTActuacion(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleMesaTrabajoAct',id]);
  }
}
