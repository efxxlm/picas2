import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { DefensaJudicialService } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
@Component({
  selector: 'app-control-tabla-proceso-defensa-judicial',
  templateUrl: './control-tabla-proceso-defensa-judicial.component.html',
  styleUrls: ['./control-tabla-proceso-defensa-judicial.component.scss']
})
export class ControlTablaProcesoDefensaJudicialComponent implements OnInit {
  displayedColumns: string[] = ['fechaRegistro', 'legitimacionPasivaActiva', 'tipoAccion', 'numeroProceso', 'estadoProceso', 'gestion'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  dataTable: any[] = [
    
  ]
  constructor(private router: Router,
    private defensaServices:DefensaJudicialService) { }

  ngOnInit(): void {
    this.defensaServices.GetListGrillaProcesosDefensaJudicial().subscribe(
      response=>{
        console.log(response);
        this.dataSource = new MatTableDataSource(response);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      }
    );
    
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  irNuevo() {
    this.router.navigate(['/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial']);
  }
  editProceso(id){
    this.router.navigate(['/gestionarProcesoDefensaJudicial/registrarNuevoProcesoJudicial',id]);
  }
  actualizarProceso(id){
    this.router.navigate(['/gestionarProcesoDefensaJudicial/actualizarProceso',id]);
  }
}
