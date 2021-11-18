import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';
import { ReprogrammingService } from 'src/app/core/_services/reprogramming/reprogramming.service';

export interface VerificacionDiaria {
  id: string;
  fechaAprobacionPoliza: string;
  numeroContrato: string;
  llaveMEN: string;
  tipoNovedad: string;
  fechaNovedad: string;
  estadoRegistro: string;
}


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
    private reprogrammingServices: ReprogrammingService,
    private router: Router,

  )
  {


   }

  ngAfterViewInit() {

    this.reprogrammingServices.GetAjusteProgramacionGrid()
      .subscribe(respuesta => {
        this.dataSource = new MatTableDataSource( respuesta );
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

  RegistrarNuevo( ajusteProgramacion ){
    console.log(ajusteProgramacion)
    this.router.navigate( [ '/registrarAjusteProgramacion/registrarAjusteProgramacion', 0 ], { state: { ajusteProgramacion } } )
  }

  openDialog (modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  };

  EnviarASupervisor( ajuste ){
    console.log( ajuste.ajusteProgramacionId )

    this.reprogrammingServices.EnviarAlSupervisorAjusteProgramacion( ajuste.ajusteProgramacionId )
      .subscribe( respuesta => {
        this.openDialog('', respuesta.message)
        if ( respuesta.code == "200" )
          this.ngAfterViewInit()
      })
  }
  verDetalle(ajusteProgramacion){
    this.router.navigate( [ '/registrarAjusteProgramacion/verHistorial', 0 ], { state: { ajusteProgramacion } } )
  }
}
