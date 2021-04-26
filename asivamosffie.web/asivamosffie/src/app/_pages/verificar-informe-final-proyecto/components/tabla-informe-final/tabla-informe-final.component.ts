import { Component, AfterViewInit, ViewChild, OnInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { VerificarInformeFinalService } from 'src/app/core/_services/verificarInformeFinal/verificar-informe-final.service';
import { Router } from '@angular/router';

export interface RegistrarInterface {
  fechaCreacion: Date;
  llaveMen: string;
  tipoIntervencion: string;
  institucionEducativa: string;
  sedeEducativa: string;
  proyectoId: number;
  registroCompletoValidacion: boolean;
  estadoValidacion: string;
  estadoValidacionString: string;
}

@Component({
  selector: 'app-tabla-informe-final',
  templateUrl: './tabla-informe-final.component.html',
  styleUrls: ['./tabla-informe-final.component.scss']
})
export class TablaInformeFinalComponent implements OnInit, AfterViewInit {
  ELEMENT_DATA: RegistrarInterface[] = [];
  displayedColumns: string[] = [
    'fechaCreacion',
    'llaveMen',
    'tipoIntervencion',
    'institucionEducativa',
    'sedeEducativa',
    'estadoValidacionString',
    'registroCompletoValidacion',
    'proyectoId'
  ];
  dataSource = new MatTableDataSource<RegistrarInterface>(this.ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  datosTabla = [];
  constructor(private verificarInformeFinalProyectoService: VerificarInformeFinalService, public dialog: MatDialog, private routes: Router ) {}

  ngOnInit(): void {
    this.getListInformeFinal();
  }

  getListInformeFinal() {
    this.verificarInformeFinalProyectoService.getListInformeFinal().subscribe(report => {
      report.forEach(element => {
        this.datosTabla.push({
          fechaCreacion: element.fechaCreacion.split('T')[0].split('-').reverse().join('/'),
          llaveMen: element.proyecto.llaveMen,
          tipoIntervencionString: element.proyecto.tipoIntervencionString,
          institucionEducativa: element.proyecto.institucionEducativa.nombre,
          sede: element.proyecto.sede.nombre,
          estadoInformeCod: element.estadoInformeCod,
          estadoValidacion: element.estadoValidacion,
          estadoValidacionString: element.estadoValidacionString,
          registroCompletoValidacion: element.registroCompletoValidacion ? 'Completo' : 'Incompleto',
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
  enviarRegistroFinal(pProyectoId: number) {
    // console.log("Antes: ",pProyectoId);
    this.verificarInformeFinalProyectoService.sendFinalReportToSupervision(pProyectoId).subscribe(respuesta => {
      this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
      this.routes.navigateByUrl( '/', {skipLocationChange: true} )
      .then( () => this.routes.navigate( ['/verificarInformeFinalProyecto'] ) );
    });
  }
}
