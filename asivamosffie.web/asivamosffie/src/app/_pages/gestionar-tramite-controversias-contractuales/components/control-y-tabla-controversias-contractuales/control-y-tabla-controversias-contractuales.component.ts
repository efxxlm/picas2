import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-control-y-tabla-controversias-contractuales',
  templateUrl: './control-y-tabla-controversias-contractuales.component.html',
  styleUrls: ['./control-y-tabla-controversias-contractuales.component.scss']
})
export class ControlYTablaControversiasContractualesComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaSolicitud',
    'numeroSolicitud',
    'tipoControversia',
    'estadoControversia',
    'gestion',
  ];
  /*
  dataTable: any[] = [
    {
      fechaSolicitud: '20/08/2020',
      numeroSolicitud: 'CO001',
      tipoControversia: '1',
      estadoControversia: '1',
      id: 1
    },
    {
      fechaSolicitud: '10/08/2020',
      numeroSolicitud: 'CO002',
      tipoControversia: '1',
      estadoControversia: '3',
      id: 2
    },
    {
      fechaSolicitud: '10/08/2020',
      numeroSolicitud: 'CO003',
      tipoControversia: '2',
      estadoControversia: '6',
      id: 3
    }
  ];  */
  public dataTable;
  constructor(private router: Router, private services: ContractualControversyService) {
   }

  ngOnInit(): void {
    this.services.GetListGrillaTipoSolicitudControversiaContractual().subscribe(data=>{
      this.dataTable = data;
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.previousPageLabel = 'Anterior';
    });
    /*
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    */
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  verDetalleEditarTramiteButton(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleEditarControversia',id]);
  }

  actualizarTramiteButton(id){
    localStorage.setItem("controversiaID",id);
    this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarTramiteControversia']);
  }

  verDetalleButton(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleControversia',id]);
  }
}
