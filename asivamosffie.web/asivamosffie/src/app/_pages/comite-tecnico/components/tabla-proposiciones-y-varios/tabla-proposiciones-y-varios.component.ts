import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { VotacionSolicitudComponent } from '../votacion-solicitud/votacion-solicitud.component';
import { VotacionSolicitudMultipleComponent } from '../votacion-solicitud-multiple/votacion-solicitud-multiple.component';

export interface OrdenDelDia {
  id: number;
  responsable: string;
  tiempo: string;
  tema: string;
  votacion: boolean;
}

const ELEMENT_DATA: OrdenDelDia[] = [
  { id: 0, responsable: '23/06/2020', tiempo: 'SA0006', tema: 'Apertura de proceso de selección', votacion: false }
];

@Component({
  selector: 'app-tabla-proposiciones-y-varios',
  templateUrl: './tabla-proposiciones-y-varios.component.html',
  styleUrls: ['./tabla-proposiciones-y-varios.component.scss']
})
export class TablaProposicionesYVariosComponent implements OnInit {

  displayedColumns: string[] = ['responsable', 'tiempo', 'tema', 'votacion', 'id'];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
    public dialog: MatDialog
  ) { }

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

  openDialogValidacionSolicitudes() {
    this.dialog.open(VotacionSolicitudComponent, {
      width: '70em'
    });
  }

  openDialogValidacionSolicitudesMultiple() {
    this.dialog.open(VotacionSolicitudMultipleComponent, {
      width: '70em'
    });
  }

}
