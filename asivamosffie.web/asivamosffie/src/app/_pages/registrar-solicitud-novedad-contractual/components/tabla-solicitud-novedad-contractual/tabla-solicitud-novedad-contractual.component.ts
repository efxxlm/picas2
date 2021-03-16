import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ContratosModificacionesContractualesService } from 'src/app/core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';

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
    'tipoNovedadNombre',
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
  ) { }

  ngAfterViewInit() {

    this.contractualNoveltyService.getListGrillaNovedadContractual()
      .subscribe(resp => {
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
    console.log(`Aprobar solicitud ${id}`);
  }

  rechazarSolicitud(id: string) {
    console.log(`Aprobar solicitud ${id}`);
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
