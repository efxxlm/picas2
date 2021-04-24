import { ActivatedRoute } from '@angular/router';
import { AutenticacionService } from './../../../../core/_services/autenticacion/autenticacion.service';
import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ContratosModificacionesContractualesService } from 'src/app/core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { map } from 'rxjs/operators';
import { DialogRechazarSolicitudInterventorComponent } from 'src/app/_pages/validar-solicitud-novedades/components/dialog-rechazar-solicitud-interventor/dialog-rechazar-solicitud-interventor.component';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

export interface VerificacionDiaria {
  id: string;
  fechaSolicitud: string;
  numeroSolicitud: string;
  tipoNovedad: string;
  estadoNovedad: string;
  estadoRegistro: string;
}

@Component({
  selector: 'app-tabla-solicitud-novedad-contractual',
  templateUrl: './tabla-solicitud-novedad-contractual.component.html',
  styleUrls: ['./tabla-solicitud-novedad-contractual.component.scss']
})

export class TablaSolicitudNovedadContractualComponent implements AfterViewInit {

  displayedColumns: string[] = [
    'fechaSolictud',
    'numeroSolicitud',
    'novedadesSeleccionadas',
    'estadoNovedadNombre',
    'registroCompleto',
    'novedadContractualId'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private contractualNoveltyService: ContractualNoveltyService,
    public dialog: MatDialog,
    private activatedRoute: ActivatedRoute,

  ) {
    // console.log( this.activatedRoute.snapshot.data );
  }

  ngAfterViewInit() {

    this.contractualNoveltyService.getListGrillaNovedadContractualObra()
      .subscribe(resp => {
        resp.sort((a, b) => {
          if (a.fechaSolictud < b.fechaSolictud) return 1;
          if (a.fechaSolictud > b.fechaSolictud) return -1;
          return 0;
        });
        resp.forEach(element => {
          element.fechaSolictud = element.fechaSolictud
            ? element.fechaSolictud.split('T')[0].split('-').reverse().join('/')
            : '';
          element.novedadesSeleccionadas = element.novedadesSeleccionadas.slice(0, -1);
        });
        this.dataSource = new MatTableDataSource(resp);

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

  aprobarSolicitud(id: string) {
    this.contractualNoveltyService.aprobarSolicitud( id )
      .subscribe( respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`);
        if ( respuesta.code === '200' )
          this.ngAfterViewInit();
      });
    // console.log(`Aprobar solicitud ${id}`);
  }

  rechazarSolicitud(id: number, numeroSolicitud, tipoNovedad) {
    const dialogCargarProgramacion = this.dialog.open(DialogRechazarSolicitudInterventorComponent, {
      width: '75em',
       data: { numeroSolicitud, tipoNovedad }
    });
    dialogCargarProgramacion.afterClosed()
      .subscribe(response => {

        if (response) {
          let novedad : NovedadContractual = {
            novedadContractualId: id,
            causaRechazo: response.causaRechazo
          };
  
          this.contractualNoveltyService.rechazarPorInterventor( novedad )
             .subscribe( respuesta => {
              this.openDialog('', `<b>${respuesta.message}</b>`);
              if ( respuesta.code === '200' )
                this.ngAfterViewInit();
             });
        }
        
      })
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  openDialogSiNo(modalTitle: string, modalText: string, e: number) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result===true) {
        this.eliminarSolicitud(e);
      }
    });
  }

  eliminarSolicitud(id: number) {
    this.contractualNoveltyService.eliminarNovedadContractual( id )
      .subscribe(respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`);
        this.ngAfterViewInit();
      });
  }

}
