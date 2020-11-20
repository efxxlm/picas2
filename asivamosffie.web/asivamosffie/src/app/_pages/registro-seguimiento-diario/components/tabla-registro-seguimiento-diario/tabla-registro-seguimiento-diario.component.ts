import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ProjectService } from 'src/app/core/_services/project/project.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';
import { FollowUpDailyService } from 'src/app/core/_services/dailyFollowUp/daily-follow-up.service';

@Component({
  selector: 'app-tabla-registro-seguimiento-diario',
  templateUrl: './tabla-registro-seguimiento-diario.component.html',
  styleUrls: ['./tabla-registro-seguimiento-diario.component.scss']
})
export class TablaRegistroSeguimientoDiarioComponent implements AfterViewInit {
  displayedColumns: string[] = [
    'llaveMen',
    'tipoIntervencion',
    'institucionEducativa',
    'sede',
    'fechaUltimoSeguimientoDiario',
    'seguimientoDiarioId'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private followUpDailyService: FollowUpDailyService,
    private router: Router,
  ) 
  { }

  ngAfterViewInit() {

    this.followUpDailyService.gridRegisterDailyFollowUp()
      .subscribe(respuesta => {
        this.dataSource = new MatTableDataSource(respuesta);

        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        this.paginator._intl.nextPageLabel = 'Siguiente';
        this.paginator._intl.getRangeLabel = function (page, pageSize, length) {
          return (page + 1).toString() + " de " + length.toString();
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

  Editar( proyecto ){
    this.router.navigate( [ '/registroSeguimientoDiario/registrarSeguimiento', proyecto.seguimientoDiarioId ? proyecto.seguimientoDiarioId : 0 ], { state: { proyecto } } )
  }

  RegistrarNuevo( proyecto ){
    this.router.navigate( [ '/registroSeguimientoDiario/registrarSeguimiento', 0 ], { state: { proyecto } } )
  }

  VerDetalle( proyecto ){
    this.router.navigate( [ '/registroSeguimientoDiario/verDetalle', proyecto.seguimientoDiarioId ? proyecto.seguimientoDiarioId : 0 ], { state: { proyecto } } )
  }

}
