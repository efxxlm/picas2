import { Component, Input, OnInit, ViewChild } from '@angular/core';
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
  @Input() controversiaID;
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
    this.services.GetListGrillMesasByControversiaId(this.controversiaID).subscribe((data:any)=>{
      for (let mesas of data){
        if(mesas.requiereMesaTrabajo==true && mesas.estadoActuacionCodigoGeneral!='1'){
          this.dataTable.push(mesas);
        }
      }
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
    this.router.navigate(['/gestionarTramiteControversiasContractuales/registrarNuevaMesaTrabajo',this.controversiaID,id]);
  }
  verDetalleEditar(id,codeMT,idMesa){
    localStorage.setItem("idMesa",idMesa);
    localStorage.setItem("nomMesaTrabajo",codeMT);
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleEditarMesaTrabajo',this.controversiaID,id]);
  }
  finalizarMesaTrabajo(id){
    this.services.FinalizarMesa(id).subscribe((data:any)=>{
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(
        () => this.router.navigate(['gestionarTramiteControversiasContractuales/actualizarTramiteControversia',this.controversiaID])
      );
    });
  }
  verDetalleMesaTrabajo(id,codeMT,idMesa){
    localStorage.setItem("idMesa",idMesa);
    localStorage.setItem("nomMesaTrabajo",codeMT);
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleMesaTrabajo',this.controversiaID,id]);
  }
  actualizarMesaTrabajo(id,codeMT,idMesa){
    localStorage.setItem("idMesaTrabajo",id);
    localStorage.setItem("nomMesaTrabajo",codeMT);
    localStorage.setItem("idMesa",idMesa);
    this.router.navigate(['/gestionarTramiteControversiasContractuales/actualizarMesaTrabajo',this.controversiaID,id]);
  } 
}
