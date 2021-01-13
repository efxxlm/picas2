import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-control-y-tabla-actuacion-reclamacion',
  templateUrl: './control-y-tabla-actuacion-reclamacion.component.html',
  styleUrls: ['./control-y-tabla-actuacion-reclamacion.component.scss']
})
export class ControlYTablaActuacionReclamacionComponent implements OnInit {

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
  //public reclamacionId = parseInt(localStorage.getItem("reclamacionID"));
  public reclamacionId = parseInt(localStorage.getItem("controversiaID"));
  constructor(private router: Router,private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.services.GetListGrillaActuacionReclamacionByActuacionID(this.reclamacionId).subscribe((data:any)=>{
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
    this.services.CambiarEstadoControversiaActuacion(id,"2").subscribe((resp:any)=>{
      this.ngOnInit();
    });
  }
  verDetalleEditarActuacion(id,actR){
    localStorage.setItem('actuacionReclamacion',actR);
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleEditarActuacionReclamacion',id]);
  }
  eliminarActuacion(id){

  }
  verDetalleActuacion(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleActuacionReclamacion',id]);
  }
}
