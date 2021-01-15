import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-control-y-tabla-reclamacion-cc',
  templateUrl: './control-y-tabla-reclamacion-cc.component.html',
  styleUrls: ['./control-y-tabla-reclamacion-cc.component.scss']
})
export class ControlYTablaReclamacionCcComponent implements OnInit {
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
    this.services.GetListGrillaControversiaReclamacion(this.controversiaID).subscribe((data:any)=>{
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
  actualizarReclamacionAseguradora(id,numReclamacion){
    localStorage.setItem("reclamacionID",id);
    localStorage.setItem("codReclamacion",numReclamacion);
    this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarReclamoAseguradora']);
  }
  registrarReclamacionAseguradora(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/registrarReclamacionAseguradora',id]);
  }
  verDetalleEditarReclamacion(id,actuacion,numReclamacion){
    localStorage.setItem("actuacion",actuacion);
    localStorage.setItem("numReclamacion",numReclamacion);
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleEditarReclamacionAseguradora',id]);
  }
  enviarReclamacionComiteTecnico(id){
    this.services.CambiarEstadoActuacionSeguimiento(id,'4').subscribe((data:any)=>{
      this.ngOnInit();
    });
  }
  verDetalleReclamacion(id,actuacion,numReclamacion){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleReclamacionAseguradora',id]);
    localStorage.setItem("actuacion",actuacion);
    localStorage.setItem("numReclamacion",numReclamacion);
  }
}
