import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-tabla-novedades-aprobadas',
  templateUrl: './tabla-novedades-aprobadas.component.html',
  styleUrls: ['./tabla-novedades-aprobadas.component.scss']
})
export class TablaNovedadesAprobadasComponent implements AfterViewInit {

  displayedColumns: string[] = [
    'fechaValidacion',
    'numeroSolicitud',
    'numeroContrato',
    'novedadesSeleccionadas',
    'estadoDescripcion',
    'registroCompletoTramiteNovedades',
    'id'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private contractualNoveltyService: ContractualNoveltyService,
    public dialog: MatDialog,
  ) { }

  ngAfterViewInit() {
    this.contractualNoveltyService.getListGrillaNovedadContractualGestionar()
      .subscribe(respuesta => {
        respuesta.forEach(element => {
          element.fechaValidacion = element.fechaValidacion
            ? element.fechaValidacion.split('T')[0].split('-').reverse().join('/')
            : '';
          element.novedadesSeleccionadas = element.novedadesSeleccionadas
            ? element.novedadesSeleccionadas.slice(0, -1)
            : '';
        });
        this.dataSource = new MatTableDataSource( respuesta );
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
          const endIndex = startIndex < length ?
            Math.min(startIndex + pageSize, length) :
            startIndex + pageSize;
          return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
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

  EnviarAComite(id){
    this.contractualNoveltyService.enviarAComite( id )
      .subscribe( respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`);
        if ( respuesta.code === '200' )
          this.ngAfterViewInit();
      })
  }

  devolverSolicitud(id){
    this.contractualNoveltyService.devolverSolicitudASupervisor( id )
      .subscribe( respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`);
        if ( respuesta.code === '200' )
          this.ngAfterViewInit();
      })
  }

  
  cancelarNovedad(id){

    this.contractualNoveltyService.CancelarNovedad( id )
      .subscribe( respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`);
        if ( respuesta.code === '200' )
          this.ngAfterViewInit();
      })
  }  

  AprobacionTecnicaJuridica(id){
    this.contractualNoveltyService.AprobacionTecnicaJuridica( id )
      .subscribe( respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`);
        if ( respuesta.code === '200' )
          this.ngAfterViewInit();
      })
  }

}
