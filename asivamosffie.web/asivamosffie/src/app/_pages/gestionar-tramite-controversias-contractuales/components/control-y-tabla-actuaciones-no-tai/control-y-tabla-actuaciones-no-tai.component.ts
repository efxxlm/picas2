import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-control-y-tabla-actuaciones-no-tai',
  templateUrl: './control-y-tabla-actuaciones-no-tai.component.html',
  styleUrls: ['./control-y-tabla-actuaciones-no-tai.component.scss']
})
export class ControlYTablaActuacionesNoTaiComponent implements OnInit {
  public controversiaID = parseInt(localStorage.getItem("controversiaID"));
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaActuacion',
    'actuacion',
    'numeroActuacion',
    'estadoRegistro',
    'estadoActuacion',
    'gestion',
  ];
  dataTable: any[] = [];  
  constructor( private services: ContractualControversyService, private router: Router) {
   }

   ngOnInit(): void {
    this.services.GetListGrillaControversiaActuacion(this.controversiaID).subscribe(data=>{
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
  enviarComiteTecnicoTramAct(id){
    this.services.CambiarEstadoControversiaActuacion(id,"2").subscribe(response=>{
      this.ngOnInit();
    });
  }
  verDetalleEditarActuacion(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleEditarActuacionNoTai',id]);
  }
  eliminarActuacion(id){
    this.services.EliminarControversiaActuacion(id).subscribe((data0:any)=>{
      this.ngOnInit();
    });
  }
  verDetalleActuacion(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleActuacionNoTai',id]);
  }
}
