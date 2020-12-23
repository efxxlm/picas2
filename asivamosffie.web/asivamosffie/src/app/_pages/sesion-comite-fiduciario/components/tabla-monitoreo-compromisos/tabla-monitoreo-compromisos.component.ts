import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { EstadosComite } from 'src/app/_interfaces/technicalCommitteSession';

export interface OrdenDelDia {
  id: number;
  fecha: string;
  numero: string;
  numeroCompromisos: number;
  nivelCumplimiento: string;
}

const ELEMENT_DATA: OrdenDelDia[] = [
  { id: 0, fecha: '09/07/2020', numero: 'CF_00001', numeroCompromisos: 1, nivelCumplimiento: '0' }
];

@Component({
  selector: 'app-tabla-monitoreo-compromisos',
  templateUrl: './tabla-monitoreo-compromisos.component.html',
  styleUrls: ['./tabla-monitoreo-compromisos.component.scss']
})
export class TablaMonitoreoCompromisosComponent implements OnInit {

  displayedColumns: string[] = ['fechaOrdenDia', 'numero', 'cantidadCompromisos', 'nivelCumplimiento', 'id'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  constructor(
                private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,
                private technicalCommitteeSessionService: TechnicalCommitteSessionService
             ) 
  {

  }

  ngOnInit(): void {

      this.technicalCommitteeSessionService.getListComite( 'True' )
        .subscribe( response => {
          console.log( response );
          this.dataSource = new MatTableDataSource( response );
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
      })

  }

}
