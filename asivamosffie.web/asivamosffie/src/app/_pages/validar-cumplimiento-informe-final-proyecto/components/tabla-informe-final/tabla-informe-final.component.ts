import { Component, AfterViewInit, ViewChild, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ValidarCumplimientoInformeFinalService } from 'src/app/core/_services/validarCumplimientoInformeFinal/validar-cumplimiento-informe-final.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

export interface RegistrarInterface {
  proyectoId: number;
  fechaCreacion: Date;
  fechaAprobacion: Date;
  llaveMen: string;
  tipoIntervencion: string;
  institucionEducativa: string;
  sedeEducativa: string;
  estadoCumplimientoString: string;
  estadoCumplimiento: string;
  registroCompletoCumplimiento: boolean;
}

@Component({
  selector: 'app-tabla-informe-final',
  templateUrl: './tabla-informe-final.component.html',
  styleUrls: ['./tabla-informe-final.component.scss']
})
export class TablaInformeFinalComponent implements OnInit, AfterViewInit {
  datosTabla = [];

  ELEMENT_DATA: RegistrarInterface[] = [];
  displayedColumns: string[] = [
    'fechaCreacion',
    'fechaAprobacion',
    'llaveMen',
    'tipoIntervencion',
    'institucionEducativa',
    'sedeEducativa',
    'estadoCumplimientoString',
    'proyectoId'
  ];
  dataSource = new MatTableDataSource<RegistrarInterface>(this.ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  constructor(
    public dialog: MatDialog,
    private validarCumplimientoInformeFinalService: ValidarCumplimientoInformeFinalService,
    private routes: Router
  ) {}

  ngOnInit(): void {
    this.getListInformeFinal();
  }

  getListInformeFinal() {
    this.validarCumplimientoInformeFinalService.getListInformeFinal().subscribe(report => {
      report.forEach(element => {
        this.datosTabla.push({
          fechaEnvio: element.fechaCreacion.split('T')[0].split('-').reverse().join('/'),
          fechaAprobacion: element.fechaAprobacion.split('T')[0].split('-').reverse().join('/'),
          llaveMen: element.proyecto.llaveMen,
          tipoIntervencion: element.proyecto.tipoIntervencionString,
          institucionEducativa: element.proyecto.institucionEducativa.nombre,
          sedeEducativa: element.proyecto.sede.nombre,
          estadoVerificacion: element.estadoCumplimientoString,
          registroCompletoCumplimiento: element.registroCompletoCumplimiento,
          tieneObservacionesInterventoria: element.tieneObservacionesInterventoria,
          tieneObservacionesCumplimiento: element.tieneObservacionesCumplimiento,
          estadoCumplimiento: element.estadoCumplimiento,
          id: element.proyectoId
        });
      });
      this.dataSource.data = this.datosTabla;
      console.log(this.dataSource.data);
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  ngAfterViewInit() {
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
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  sendFinalReportToSupervision(pProyectoId: number) {
    this.validarCumplimientoInformeFinalService.sendFinalReportToSupervision(pProyectoId).subscribe(respuesta => {
      this.openDialog('', '<b>La informaci??n ha sido guardada exitosamente.</b>');
      this.routes.navigateByUrl( '/', {skipLocationChange: true} )
      .then( () => this.routes.navigate( ['/validarCumplimientoInformeFinalProyecto'] ) );
    });
  }

  approveFinalReportByFulfilment(pProyectoId: number) {
    this.validarCumplimientoInformeFinalService.approveFinalReportByFulfilment(pProyectoId).subscribe(respuesta => {
      this.openDialog('', '<b>La informaci??n ha sido guardada exitosamente.</b>');
      this.routes.navigateByUrl( '/', {skipLocationChange: true} )
      .then( () => this.routes.navigate( ['/validarCumplimientoInformeFinalProyecto'] ) );
    });
  }
}
