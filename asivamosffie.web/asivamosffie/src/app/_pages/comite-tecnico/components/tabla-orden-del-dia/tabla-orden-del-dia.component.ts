import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabla-orden-del-dia',
  templateUrl: './tabla-orden-del-dia.component.html',
  styleUrls: ['./tabla-orden-del-dia.component.scss']
})
export class TablaOrdenDelDiaComponent implements OnInit {

  displayedColumns: string[] = ['fecha', 'numero', 'estado', 'id'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
                private technicalCommitteeSessionService: TechnicalCommitteSessionService,
                private router: Router,

             ) 
  {

  }

  ngOnInit(): void {

    this.technicalCommitteeSessionService.getComiteGrilla()
      .subscribe( response => {
        console.log( response );
        this.dataSource = new MatTableDataSource( response );
      })

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

  onEdit(e: number) {
    this.router.navigate(['/comiteTecnico/crearOrdenDelDia',e ,'']);
  }

  onConvocar(e: number){

  }

  OnDelete(e: number){

  }
}
