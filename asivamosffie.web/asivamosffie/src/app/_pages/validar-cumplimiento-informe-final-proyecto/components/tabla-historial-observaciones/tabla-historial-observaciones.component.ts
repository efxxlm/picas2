import { Component, AfterViewInit, ViewChild, OnInit, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

const ELEMENT_DATA = [
  {
    fechaRevision: '29/11/2020',
    responsable: 'Responsables de grupo de novedades y liquidaciones',
    observaciones: 'Verifique el ITEM 11 ya que deberia contar con anexo digital'
  }
];

@Component({
  selector: 'app-tabla-historial-observaciones',
  templateUrl: './tabla-historial-observaciones.component.html',
  styleUrls: ['./tabla-historial-observaciones.component.scss']
})
export class TablaHistorialObservacionesComponent implements OnInit {
  ELEMENT_DATA: any[];
  displayedColumns: string[] = ['fechaRevision', 'responsable', 'observaciones'];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(public dialog: MatDialog) {}

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
    this.paginator._intl.previousPageLabel = 'Anterior';
  }
}
