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
    private reprogrammingService: ReprogrammingService,
    private router: Router,
  ) { }

  ngAfterViewInit() {
    this.reprogrammingService.getAjusteProgramacionGrid()
      .subscribe(respuesta => {
        respuesta.forEach(element => {
          element.novedadesSeleccionadas = element.novedadesSeleccionadas
            ? element.novedadesSeleccionadas.slice(0, -1)
            : '';
        });
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
    this.reprogrammingService.aprobarAjusteProgramacion( id )
      .subscribe( respuesta => {
        this.openDialog('', respuesta.message);
        if (respuesta.code === "200")
        this.ngAfterViewInit();
      })
  }

  enviarInterventor(id){
    this.reprogrammingService.enviarAlInterventor( id )
      .subscribe( respuesta => {
        this.openDialog('', respuesta.message);
        if (respuesta.code === "200")
        this.ngAfterViewInit();
      })
  }



  validar( ajusteProgramacion , esRegistroNuevo){
    if(esRegistroNuevo){
      this.router.navigate( [ '/validarAjusteProgramacion/validar', ajusteProgramacion.ajusteProgramacionId], { state: { ajusteProgramacion } } )
    }else{
      this.router.navigate( [ '/validarAjusteProgramacion/verDetalleEditar', ajusteProgramacion.ajusteProgramacionId], { state: { ajusteProgramacion } } )
    }
  }

  verDetalle( ajusteProgramacion){
      this.router.navigate( [ '/validarAjusteProgramacion/verDetalle', ajusteProgramacion.ajusteProgramacionId], { state: { ajusteProgramacion } } )
  }

}
