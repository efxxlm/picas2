import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { DialogRechazarSolicitudComponent } from '../dialog-rechazar-solicitud/dialog-rechazar-solicitud.component';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';
import { DialogRechazarSolicitudInterventorComponent } from '../dialog-rechazar-solicitud-interventor/dialog-rechazar-solicitud-interventor.component';

export interface VerificacionDiaria {
  id: string;
  fecha: string;
  numeroSolicitud: string;
  numeroContrato: string;
  tipoNovedad: string;
  estadoNovedad: string;
  estadoRegistro: string;
}

const ELEMENT_DATA: VerificacionDiaria[] = [
  {
    id: '1',
    fecha: '15/10/2020',
    numeroSolicitud: 'NOV-001',
    numeroContrato: 'A887654344',
    tipoNovedad: 'Modificación de Condiciones Contractuales',
    estadoNovedad: 'Sin verificar',
    estadoRegistro: 'Incompleto'
  },
  {
    id: '2',
    fecha: '17/10/2020',
    numeroSolicitud: 'NOV-006',
    numeroContrato: 'A887654344',
    tipoNovedad: 'Suspensión',
    estadoNovedad: 'Sin verificar',
    estadoRegistro: 'Incompleto'
  }
];

@Component({
  selector: 'app-tabla-novedad-contratos-obra',
  templateUrl: './tabla-novedad-contratos-obra.component.html',
  styleUrls: ['./tabla-novedad-contratos-obra.component.scss']
})
export class TablaNovedadContratosObraComponent implements AfterViewInit {
  displayedColumns: string[] = [
    'fecha',
    'numeroSolicitud',
    'numeroContrato',
    'tipoNovedad',
    'estadoNovedad',
    'estadoRegistro',
    'id'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private contractualNoveltyService: ContractualNoveltyService, public dialog: MatDialog) {}

  ngAfterViewInit() {
    this.contractualNoveltyService.getListGrillaNovedadContractualObra().subscribe(resp => {

      resp = resp.filter( x => x.estadoCodigo !== '1' && x.estadoCodigo !== '9' && x.estadoCodigo !== '3' && x.estadoCodigo !== '2');

      resp.forEach(element => {
        element.fechaSolictud = element.fechaSolictud
          ? element.fechaSolictud.split('T')[0].split('-').reverse().join('/')
          : '';
        element.novedadesSeleccionadas = element.novedadesSeleccionadas
          ? element.novedadesSeleccionadas.slice(0, -1)
          : '';
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
