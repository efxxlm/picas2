import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { FaseUnoConstruccionService } from '../../../../core/_services/faseUnoConstruccion/fase-uno-construccion.service';

@Component({
  selector: 'app-tabla-requisitos-tecnicos',
  templateUrl: './tabla-requisitos-tecnicos.component.html',
  styleUrls: ['./tabla-requisitos-tecnicos.component.scss']
})
export class TablaRequisitosTecnicosComponent implements OnInit {
  dataSource = new MatTableDataSource();
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
    private faseUnoConstruccionSvc: FaseUnoConstruccionService,
    private dialog: MatDialog
  ) {
    this.cargarRegistros();
  }

  cargarRegistros() {
    this.faseUnoConstruccionSvc.getContractsGrid().subscribe(listas => {
      
      if (listas.length > 0) {
        listas.forEach(
          registro =>
            (registro.fechaAprobacion = registro.fechaAprobacion
              ? registro.fechaAprobacion.split('T')[0].split('-').reverse().join('/')
              : '')
        );
      }

      this.dataSource = new MatTableDataSource(listas);
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
        const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
        return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
      };
      this.paginator._intl.previousPageLabel = 'Anterior';
    });
  }

  ngOnInit(): void {}

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  getForm(id: number, fechaPoliza: string) {
    this.routes.navigate(['/requisitosTecnicosConstruccion/gestionarInicioContrato', id], { state: { fechaPoliza } });
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  aprobarInicio(id: number) {
    this.faseUnoConstruccionSvc.aprobarInicio(id).subscribe(
      response => {
        this.openDialog('', response.message);
        this.cargarRegistros();
      },
      err => this.openDialog('', err.message)
    );
  }

  verDetalle(id: number) {
    this.routes.navigate(['/requisitosTecnicosConstruccion/verDetalles', id]);
  }
}
