import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FuenteFinanciacion, FuenteFinanciacionService } from 'src/app/core/_services/fuenteFinanciacion/fuente-financiacion.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';


@Component({
  selector: 'app-tabla-fuentes',
  templateUrl: './tabla-fuentes.component.html',
  styleUrls: ['./tabla-fuentes.component.scss']
})
export class TablaFuentesComponent implements OnInit {

  displayedColumns: string[] = [ 'fechaCreacion', 'tipoAportante', 'aportante', 'vigencia', 'fuenteDeRecursos', 'valorAporteFuenteDeRecursos', 'valorAporteEnCuenta', 'estado', 'id'];
  dataSource = new MatTableDataSource();

  listaFF: FuenteFinanciacion[] = [];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
                private fuenteFinanciacionService: FuenteFinanciacionService,
                private router: Router
  ) { }

  ngOnInit(): void {
    
    this.fuenteFinanciacionService.listaFuenteFinanciacion().subscribe( ff => {
      this.dataSource = new MatTableDataSource(ff);
    })

    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  editarFuente(e: number, idTipo: number) {
    console.log(e);
    this.router.navigate(['/registrarFuentes',e,idTipo]);
  }
  eliminarFuente(e: number, idTipo: number) {
    console.log(e);
  }
  controlRecursosFuente(e: number, idTipo: number) {
    console.log(e);
  }

}
