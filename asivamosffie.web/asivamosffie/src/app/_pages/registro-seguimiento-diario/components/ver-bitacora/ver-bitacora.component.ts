import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ProjectService } from 'src/app/core/_services/project/project.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DatePipe } from '@angular/common';


export interface SeguimientoDiario {
  id: string;
  fechaRegistro: string;
  fechaValidacion: string;
  productividad: string;
  estadoSeguimiento: string;
}

const ELEMENT_DATA: SeguimientoDiario[] = [
  {
    id: '1',
    fechaRegistro: '05/07/2020',
    fechaValidacion: 'Sin seguimiento ',
    productividad: 'Alta',
    estadoSeguimiento: 'Seguimiento diario enviado'
  }
];

@Component({
  selector: 'app-ver-bitacora',
  templateUrl: './ver-bitacora.component.html',
  styleUrls: ['./ver-bitacora.component.scss']
})
export class VerBitacoraComponent implements AfterViewInit {

  displayedColumns: string[] = [
    'fechaRegistro',
    'fechaValidacion',
    'productividad',
    'estadoSeguimiento',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(

  ) { }

  ngAfterViewInit() {

    this.followUpDailyService.gridRegisterDailyFollowUp()
      .subscribe(respuesta => {
        this.dataSource = new MatTableDataSource(respuesta);

        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        this.paginator._intl.nextPageLabel = 'Siguiente';
        this.paginator._intl.getRangeLabel = (page, pageSize, length) => (page + 1).toString() + ' de ' + length.toString();
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
}
