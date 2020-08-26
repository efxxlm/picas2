import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ComiteTecnico } from 'src/app/_interfaces/technicalCommitteSession';

@Component({
  selector: 'app-tabla-otros-temas',
  templateUrl: './tabla-otros-temas.component.html',
  styleUrls: ['./tabla-otros-temas.component.scss']
})
export class TablaOtrosTemasComponent implements OnInit {

  @Input() objetoComiteTecnico: ComiteTecnico 

  displayedColumns: string[] = ['responsable', 'tiempo', 'temaDolicitud', 'id'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor() { }

  ngOnInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
  }

  cargarRegistros(){
    this.dataSource = new MatTableDataSource( this.objetoComiteTecnico.sesionComiteTema );
    console.log('s', this.objetoComiteTecnico)
  }

}
