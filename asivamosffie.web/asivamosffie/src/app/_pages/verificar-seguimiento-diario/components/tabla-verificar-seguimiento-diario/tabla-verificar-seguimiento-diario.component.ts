import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { FollowUpDailyService } from 'src/app/core/_services/dailyFollowUp/daily-follow-up.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabla-verificar-seguimiento-diario',
  templateUrl: './tabla-verificar-seguimiento-diario.component.html',
  styleUrls: ['./tabla-verificar-seguimiento-diario.component.scss']
})
export class TablaVerificarSeguimientoDiarioComponent implements AfterViewInit {

  displayedColumns: string[] = [
    'fechaUltimoSeguimientoDiario',
    'llaveMen',
    'tipoIntervencion',
    'institucionEducativa',
    'sede',
    'alertas',
    'estadoCodigo',
    'seguimientoDiarioId'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private followUpDailyService: FollowUpDailyService,
    private router: Router,
    private dialog: MatDialog,
  ) 
  { }

  ngAfterViewInit() {

    this.followUpDailyService.gridVerifyDailyFollowUp()
      .subscribe( respuesta => {
        this.dataSource = new MatTableDataSource(respuesta);

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

  Verificar( proyecto ){
    this.router.navigate( [ '/verificarSeguimientoDiario/verificarSeguimiento', proyecto.seguimientoDiarioId ? proyecto.seguimientoDiarioId : 0 ], { state: { proyecto } } )
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  Enviar( proyecto ){
    this.followUpDailyService.sendToSupervision( proyecto.seguimientoDiarioId )
      .subscribe( respuesta => {
        this.openDialog( '', respuesta.message)
        if ( respuesta.code == "200" )
          this.ngAfterViewInit()
      })
  }

}
