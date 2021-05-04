import { ActivatedRoute } from '@angular/router';
import { AutenticacionService } from './../../../../core/_services/autenticacion/autenticacion.service';
import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DialogRechazarSolicitudInterventorComponent } from '../dialog-rechazar-solicitud-interventor/dialog-rechazar-solicitud-interventor.component';
import { DialogDevolverSolicitudInterventorComponent } from '../dialog-devolver-solicitud-interventor/dialog-devolver-solicitud-interventor.component';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { DialogRechazarSolicitudComponent } from '../dialog-rechazar-solicitud/dialog-rechazar-solicitud.component';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

export interface VerificacionDiaria {
  id: string;
  fechaSolicitud: string;
  numeroSolicitud: string;
  tipoNovedad: string;
  estadoNovedad: string;
  estadoRegistro: string;
}

const ELEMENT_DATA: VerificacionDiaria[] = [
  {
    id: '1',
    fechaSolicitud: '15/10/2020',
    numeroSolicitud: 'NOV-001',
    tipoNovedad: 'Modificación de Condiciones Contractuales',
    estadoNovedad: 'En proceso de registro',
    estadoRegistro: 'Completo'
  }
];

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
    private activatedRoute: ActivatedRoute
  ) {
    console.log(this.activatedRoute.snapshot.data);
  }

  ngAfterViewInit() {
    this.contractualNoveltyService.getListGrillaNovedadContractualInterventoria().subscribe(resp => {

      resp = resp.filter( x => x.estadoCodigo !== '1');

      resp.forEach(element => {
        element.fechaSolictud = element.fechaSolictud
          ? element.fechaSolictud.split('T')[0].split('-').reverse().join('/')
          : '';
      });
      this.dataSource = new MatTableDataSource(resp);

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

  rechazarSolicitud(id: number, numeroSolicitud, tipoNovedad) {
    const dialogCargarProgramacion = this.dialog.open(DialogRechazarSolicitudInterventorComponent, {
      width: '75em',
      data: { numeroSolicitud, tipoNovedad }
    });
    dialogCargarProgramacion.afterClosed().subscribe(response => {
      if (response) {
        let novedad: NovedadContractual = {
          novedadContractualId: id,
          causaRechazo: response.causaRechazo
        };

        this.contractualNoveltyService.rechazarPorSupervisor(novedad).subscribe(respuesta => {
          this.openDialog('', `<b>${respuesta.message}</b>`);
          if (respuesta.code === '200') this.ngAfterViewInit();
        });
      }
    });
  }

  devolverSolicitud(id: string) {
    const dialogCargarProgramacion = this.dialog.open(DialogDevolverSolicitudInterventorComponent, {
      width: '75em'
      // data: { }
    });
    dialogCargarProgramacion.afterClosed().subscribe(response => {
      if (response) {
        console.log(response);
      }
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  tramitar(id) {
    this.contractualNoveltyService.tramitarSolicitud(id).subscribe(respuesta => {
      this.openDialog('', `<b>${respuesta.message}</b>`);
      if (respuesta.code === '200') this.ngAfterViewInit();
    });
  }

  devolver(id) {
    this.contractualNoveltyService.devolverSolicitud(id).subscribe(respuesta => {
      this.openDialog('', `<b>${respuesta.message}</b>`);
      if (respuesta.code === '200') this.ngAfterViewInit();
    });
  }
}
