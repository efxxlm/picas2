import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { SolicitudesContractuales } from 'src/app/_interfaces/technicalCommitteSession'

@Component({
  selector: 'app-tabla-solicitudes-contractuales',
  templateUrl: './tabla-solicitudes-contractuales.component.html',
  styleUrls: ['./tabla-solicitudes-contractuales.component.scss']
})
export class TablaSolicitudesContractualesComponent implements OnInit {

  @Input() solicitudesContractuales: SolicitudesContractuales[];

  displayedColumns: string[] = ['fecha', 'numero', 'tipo'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  constructor(
                private techicalCommitteeSessionService: TechnicalCommitteSessionService,

             ) 
  {

  }

  ngOnInit(): void {

        this.dataSource = new MatTableDataSource( this.solicitudesContractuales );

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

}
