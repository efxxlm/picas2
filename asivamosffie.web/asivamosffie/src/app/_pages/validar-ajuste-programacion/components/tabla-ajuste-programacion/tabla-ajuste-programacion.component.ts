import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { Router } from '@angular/router';

export interface VerificacionDiaria {
  id: string;
  fechaAprobacionPoliza: string;
  numeroContrato: string;
  llaveMEN: string;
  tipoNovedad: string;
  fechaNovedad: string;
  estadoRegistro: string;
}

const ELEMENT_DATA: VerificacionDiaria[] = [
  {
    id: '1',
    fechaAprobacionPoliza: '21/06/2020',
    numeroContrato: 'C223456789',
    llaveMEN: 'LL03260326',
    tipoNovedad: 'ModProrroga',
    fechaNovedad: '04/07/2020',
    estadoRegistro: 'Sin ajustes',
  }
];

@Component({
  selector: 'app-tabla-ajuste-programacion',
  templateUrl: './tabla-ajuste-programacion.component.html',
  styleUrls: ['./tabla-ajuste-programacion.component.scss']
})
export class TablaAjusteProgramacionComponent implements AfterViewInit {

  displayedColumns: string[] = [
    'fechaAprobacionPoliza',
    'numeroContrato',
    'llaveMEN',
    'tipoNovedad',
    'fechaNovedad',
    'estadoRegistro',
    'id'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private dialog: MatDialog,
    private faseConstruccionServices: FaseUnoConstruccionService,
    private router: Router,
  ) { }

  ngAfterViewInit() {
    this.faseConstruccionServices.GetAjusteProgramacionGrid()
      .subscribe(respuesta => {
        this.dataSource = new MatTableDataSource(respuesta.filter( r => r.estadoCodigo === '3' || r.estadoCodigo === '4' || r.estadoCodigo === '5' || r.estadoCodigo === '6' ));
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        this.paginator._intl.nextPageLabel = 'Siguiente';
        this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
          return (page + 1).toString() + ' de ' + length.toString();
        };
        this.paginator._intl.previousPageLabel = 'Anterior';
      });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  Aprobar(id){
    this.faseConstruccionServices.AprobarAjusteProgramacion( id )
      .subscribe( respuesta => {
        this.openDialog('', respuesta.message);
        if (respuesta.code === "200")
        this.ngAfterViewInit();
      })
  }

}
