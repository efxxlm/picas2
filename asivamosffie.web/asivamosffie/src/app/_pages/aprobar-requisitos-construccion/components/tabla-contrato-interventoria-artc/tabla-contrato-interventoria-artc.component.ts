import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { FaseDosAprobarConstruccionService } from 'src/app/core/_services/faseDosAprobarConstruccion/fase-dos-aprobar-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-tabla-contrato-interventoria-artc',
  templateUrl: './tabla-contrato-interventoria-artc.component.html',
  styleUrls: ['./tabla-contrato-interventoria-artc.component.scss']
})
export class TablaContratoInterventoriaArtcComponent implements OnInit {
  dataSource = new MatTableDataSource();
  estadosConstruccionInterventoria: any;
  tipoContratoInterventoria = '2';
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaAprobacion',
    'numeroContrato',
    'cantidadProyectosAsociados',
    'cantidadProyectosRequisitosAprobados',
    'cantidadProyectosRequisitosPendientes',
    'estadoNombre',
    'gestion'
  ];

  constructor(
    private routes: Router,
    private dialog: MatDialog,
    private faseDosAprobarConstruccionSvc: FaseDosAprobarConstruccionService
  ) {}

  ngOnInit(): void {
    this.faseDosAprobarConstruccionSvc.listaEstadosAprobarConstruccion('interventoria').subscribe(response => {
      this.estadosConstruccionInterventoria = response;
      console.log(this.estadosConstruccionInterventoria);
      this.faseDosAprobarConstruccionSvc.getContractsGrid(this.tipoContratoInterventoria).subscribe(listas => {
        const dataTable = [];
        listas.forEach(lista => {
          if (
            Number(lista['estadoCodigo']) >= Number(this.estadosConstruccionInterventoria.enviadoAlSupervisor.codigo) &&
            lista['tipoContratoCodigo'] === this.tipoContratoInterventoria
          ) {
            dataTable.push(lista);
          }
          if (
            lista['estaDevuelto'] === true &&
            Number(lista['estadoCodigo']) < Number(this.estadosConstruccionInterventoria.enviadoAlSupervisor.codigo) &&
            lista['tipoContratoCodigo'] === this.tipoContratoInterventoria
          ) {
            dataTable.push(lista);
          }
        });
        console.log(dataTable);
        dataTable.forEach(element => {
          element.fechaAprobacion = element.fechaAprobacion
            ? element.fechaAprobacion.split('T')[0].split('-').reverse().join('/')
            : '';
        });
        this.dataSource = new MatTableDataSource(dataTable);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por p??gina';
        this.paginator._intl.nextPageLabel = 'Siguiente';
        this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
          if (length === 0 || pageSize === 0) {
            return '0 de ' + length;
          }
          length = Math.max(length, 0);
          const startIndex = page * pageSize;
          // If the start index exceeds the list length, do not try and fix the end index to the end.
          const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
          return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
        };
        this.paginator._intl.previousPageLabel = 'Anterior';
      });
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  getForm(id: number) {
    this.routes.navigate(['/aprobarRequisitosTecnicosConstruccion/verificarRequisitosInicioInterventoria', id]);
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  aprobarInicio(contratoId: number, pEstado: string) {
    this.faseDosAprobarConstruccionSvc
      .cambiarEstadoContratoEstadoVerificacionConstruccionCodigo(contratoId, pEstado)
      .subscribe(response => {
        this.openDialog('', `<b>${response.message}</b>`);
        this.routes
          .navigateByUrl('/', { skipLocationChange: true })
          .then(() => this.routes.navigate(['/aprobarRequisitosTecnicosConstruccion']));
      });
  }

  verDetalle(id: number) {
    this.routes.navigate(['/aprobarRequisitosTecnicosConstruccion/verDetalleInterventoria', id]);
  }
}
