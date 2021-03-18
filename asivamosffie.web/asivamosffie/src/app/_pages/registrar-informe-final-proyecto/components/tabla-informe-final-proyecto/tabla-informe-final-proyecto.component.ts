import { Component, AfterViewInit, ViewChild, OnInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { RegistrarInformeFinalProyectoService } from 'src/app/core/_services/registrarInformeFinal/registrar-informe-final-proyecto.service';

export interface RegistrarInterface {
  fechaTerminacionObra: Date,
  fechaTerminacionInterventoria: Date,
  llaveMen: string,
  tipoIntervencion: string,
  institucionEducativa: string,
  sedeEducativa: string,
  proyectoId: number,
  registroCompleto: boolean;
  estadoInforme: string,
  estadoInformeCod: string
}

@Component({
  selector: 'app-tabla-informe-final-proyecto',
  templateUrl: './tabla-informe-final-proyecto.component.html',
  styleUrls: ['./tabla-informe-final-proyecto.component.scss']
})

export class TablaInformeFinalProyectoComponent implements OnInit, AfterViewInit {
  ELEMENT_DATA : RegistrarInterface[] = [];
  displayedColumns: string[] = [
    'fechaTerminacionInterventoria',
    'fechaTerminacionObra',
    'llaveMen',
    'tipoIntervencion',
    'institucionEducativa',
    'sedeEducativa',
    'estadoInforme',
    'registroCompleto',
    'proyectoId'
  ];
  dataSource = new MatTableDataSource<RegistrarInterface>(this.ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private registrarInformeFinalProyectoService: RegistrarInformeFinalProyectoService,
    public dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.getAllReports();
  }

  getAllReports(){
    this.registrarInformeFinalProyectoService.getListReportGrilla()
    .subscribe(report => {
      this.dataSource.data = report as RegistrarInterface[];
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
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
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

  enviarRegistroFinal(pProyectoId: number) {
    this.registrarInformeFinalProyectoService.sendFinalReportToSupervision(pProyectoId)
      .subscribe(respuesta => {
        this.openDialog('', '<b>La información ha sido guardada correctamente.</b>');
        this.ngOnInit();
      });
  }

}