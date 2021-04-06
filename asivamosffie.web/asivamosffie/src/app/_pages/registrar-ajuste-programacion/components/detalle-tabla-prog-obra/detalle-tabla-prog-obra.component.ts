import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DialogObservacionesComponent } from 'src/app/_pages/solicitar-contratacion/components/dialog-observaciones/dialog-observaciones.component';
import { CargarProgramacionComponent } from '../cargar-programacion/cargar-programacion.component';
export interface VerificacionDiaria {
  id: string;
  fechaCargue: string;
  numeroToalRegistros: string;
  numeroRegistrosValidos: string;
  numeroRegistrosInalidos: string;
  estadoCargue: string;
}

const ELEMENT_DATA: VerificacionDiaria[] = [
  {
    id: '1',
    fechaCargue: '10/08/2020',
    numeroToalRegistros: '5',
    numeroRegistrosValidos: '3',
    numeroRegistrosInalidos: '2',
    estadoCargue: 'Fallido',
  }
];
@Component({
  selector: 'app-detalle-tabla-prog-obra',
  templateUrl: './detalle-tabla-prog-obra.component.html',
  styleUrls: ['./detalle-tabla-prog-obra.component.scss']
})
export class DetalleTablaProgObraComponent implements OnInit {
  displayedColumns: string[] = [
    'fechaCargue',
    'numeroToalRegistros',
    'numeroRegistrosValidos',
    'numeroRegistrosInalidos',
    'estadoCargue',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  @Input() ajusteProgramacionInfo: any;

  constructor(
    public dialog: MatDialog
  ) { }

  ngOnInit(): void {
  }

  openCargarProgramacion() {
    console.log(this.ajusteProgramacionInfo)
    const dialogRef = this.dialog.open(CargarProgramacionComponent, {
      width: '75em',
      data: { ajusteProgramacionInfo: this.ajusteProgramacionInfo }
    });
    dialogRef.afterClosed()
      .subscribe(response => {
        if (response) {
          console.log(response);
        };
      })
  }

  openObservaciones(id: string) {
    const dialogCargarProgramacion = this.dialog.open(DialogObservacionesComponent, {
      width: '75em',
      // data: { }
    });
    dialogCargarProgramacion.afterClosed()
      .subscribe(response => {
        if (response) {
          console.log(response);
        };
      })
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      return (page + 1).toString() + ' de ' + length.toString();
    };
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }


}
