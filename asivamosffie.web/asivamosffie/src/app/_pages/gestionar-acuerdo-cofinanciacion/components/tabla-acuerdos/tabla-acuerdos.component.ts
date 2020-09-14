import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CofinanciacionService, Cofinanciacion } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';
import { Router } from '@angular/router';

// export interface TableElement {
//   id: number;
//   fechaCreacion: string;
//   numeroAcuerdo: string;
//   vigenciaAcuerdo: number;
//   valorTotal: number;
//   estadoRegistro: string;
// }

// const ELEMENT_DATA: TableElement[] = [
//   {id: 1, fechaCreacion: '26/05/2020', numeroAcuerdo: '000001', vigenciaAcuerdo: 2020, valorTotal: 85000000, estadoRegistro: 'Completo'},
//   {id: 2, fechaCreacion: '26/05/2020', numeroAcuerdo: '000001', vigenciaAcuerdo: 2020, valorTotal: 85000000, estadoRegistro: 'Completo'},
//   {id: 3, fechaCreacion: '26/05/2020', numeroAcuerdo: '000001', vigenciaAcuerdo: 2020, valorTotal: 85000000, estadoRegistro: 'Completo'},
// ];

@Component({
  selector: 'app-tabla-acuerdos',
  templateUrl: './tabla-acuerdos.component.html',
  styleUrls: ['./tabla-acuerdos.component.scss']
})
export class TablaAcuerdosComponent implements OnInit {

  displayedColumns: string[] = ['fechaCreacion', 'numeroAcuerdo', 'vigenciaAcuerdo', 'valorTotal', 'estadoRegistro', 'id'];
  dataSource = new MatTableDataSource();
  listaCofinanciacion: Cofinanciacion[] = [];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor( private cofinanciacionService: CofinanciacionService,
               private router: Router ) { }

  ngOnInit(): void {

    this.cofinanciacionService.listaAcuerdosCofinanciacion().subscribe( cof => 
      {
         this.listaCofinanciacion = cof; 
         this.dataSource.data = this.listaCofinanciacion;
      } );

    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  editarAcuerdo(e: number) {
    this.router.navigate([`/gestionarAcueros/resgistrarAcuerdos`,{ id: e }]);
  }
  eliminarAcuerdo(e: number) {
  }

}
