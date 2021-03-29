import { Component, AfterViewInit, ViewChild, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ValidarInformeFinalService } from 'src/app/core/_services/validarInformeFinal/validar-informe-final.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

export interface RegistrarInterface {
  proyectoId: number;
  fechaEnvio: Date;
  llaveMen: string;
  tipoIntervencion: string;
  institucionEducativa: string;
  sedeEducativa: string;
  estadoAprobacionString: string;
}

@Component({
  selector: 'app-tabla-informe-final-proyecto',
  templateUrl: './tabla-informe-final-proyecto.component.html',
  styleUrls: ['./tabla-informe-final-proyecto.component.scss']
})
export class TablaInformeFinalProyectoComponent implements OnInit, AfterViewInit {
  ELEMENT_DATA: RegistrarInterface[] = [];

  displayedColumns: string[] = [
    'fechaEnvio',
    'llaveMen',
    'tipoIntervencion',
    'institucionEducativa',
    'sedeEducativa',
    'estadoAprobacionString',
    'proyectoId'
  ];
  dataSource = new MatTableDataSource<RegistrarInterface>(this.ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  datosTabla = [];

  constructor(public dialog: MatDialog, private validarInformeFinalService: ValidarInformeFinalService) {}

  ngOnInit(): void {
    this.getListInformeFinal();
  }

  getListInformeFinal() {
    this.validarInformeFinalService.getListInformeFinal().subscribe(report => {
      report.forEach(element => {
        this.datosTabla.push({
          fechaCreacion : element.fechaCreacion.split('T')[0].split('-').reverse().join('/'),
          llaveMen: element.proyecto.llaveMen,
          tipoIntervencionString: element.proyecto.tipoIntervencionString,
          institucionEducativa: element.proyecto.institucionEducativa.nombre,
          sede: element.proyecto.sede.nombre,
          estadoAprobacion: element.estadoAprobacion,
          estadoAprobacionString: element.estadoAprobacionString,
          proyectoId: element.proyectoId
        });
      })
      this.dataSource.data = this.datosTabla;
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
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  sendFinalReportToFinalVerification(pProyectoId: number) {
    this.validarInformeFinalService.sendFinalReportToFinalVerification(pProyectoId).subscribe(respuesta => {
      this.openDialog('', '<b>La información ha sido enviada correctamente.</b>');
      this.ngOnInit();
    });
  }

  sendFinalReportToInterventor(pProyectoId: number) {
    this.validarInformeFinalService.sendFinalReportToInterventor(pProyectoId).subscribe(respuesta => {
      this.openDialog('', '<b>La información ha sido enviada correctamente.</b>');
      this.ngOnInit();
    });
  }
}
