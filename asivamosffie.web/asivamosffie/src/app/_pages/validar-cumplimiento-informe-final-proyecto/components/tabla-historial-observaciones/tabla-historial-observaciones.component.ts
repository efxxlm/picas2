import { Component, AfterViewInit, ViewChild, OnInit, Input } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { InformeFinalObservaciones, Report } from 'src/app/_interfaces/proyecto-final.model';

@Component({
  selector: 'app-tabla-historial-observaciones',
  templateUrl: './tabla-historial-observaciones.component.html',
  styleUrls: ['./tabla-historial-observaciones.component.scss']
})
export class TablaHistorialObservacionesComponent implements OnInit {
  @Input() data: Report;
  ELEMENT_DATA : InformeFinalObservaciones[] = [];
  anexos: any[];
  dataSource = new MatTableDataSource<InformeFinalObservaciones>(this.ELEMENT_DATA);

  displayedColumns: string[] = ['fechaCreacion', 'responsable', 'observaciones'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(public dialog: MatDialog) {}

  ngOnInit(): void {
    if(this.data != null){
      if(this.data.proyecto.informeFinal[0].historialObsInformeFinalInterventoriaNovedades.length > 0){
        this.dataSource.data = this.data.proyecto.informeFinal[0].historialObsInformeFinalInterventoriaNovedades as InformeFinalObservaciones[];
      }      
    }
  }

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
