import { Component, AfterViewInit, ViewChild, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { RegisterProjectEtcService } from 'src/app/core/_services/registerProjectETC/register-project-etc.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

export interface RegistrarInterface {
  informeFinalId: number,
  fechaSuscripcion: Date,
  llaveMen: string,
  tipoIntervencion: string,
  institucionEducativa: string,
  sedeEducativa: string,
  estadoEntregaETCString: string,
  registroCompletoEntregaETC:boolean,
}

@Component({
  selector: 'app-tabla-proyectos',
  templateUrl: './tabla-proyectos.component.html',
  styleUrls: ['./tabla-proyectos.component.scss']
})
export class TablaProyectosComponent implements OnInit, AfterViewInit {
  datosTabla = [];

  ELEMENT_DATA : RegistrarInterface[] = [];
  displayedColumns: string[] = [
    'fechaSuscripcion',
    'llaveMen',
    'tipoIntervencion',
    'institucionEducativa',
    'sedeEducativa',
    'estadoEntregaETCString',
    'registroCompletoEntregaETC',
    'informeFinalId'
  ];
  dataSource = new MatTableDataSource<RegistrarInterface>(this.ELEMENT_DATA);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  constructor(
    public dialog: MatDialog,
    private registerProjectETCService: RegisterProjectEtcService,
    private routes: Router
  ) { }

  ngOnInit(): void {
    this.getListInformeFinal();
  }

  getListInformeFinal(){
    this.registerProjectETCService.getListInformeFinal()
    .subscribe(report => {
      report.forEach(element => {
        this.datosTabla.push({
          fechaSuscripcion : element.fechaSuscripcion.split('T')[0].split('-').reverse().join('/'),
          llaveMen: element.proyecto.llaveMen,
          tipoIntervencionString: element.proyecto.tipoIntervencionString,
          institucionEducativa: element.proyecto.institucionEducativa.nombre,
          sede: element.proyecto.sede.nombre,
          estadoEntregaEtc: element.estadoEntregaEtc,
          estadoEntregaETCString: element.estadoEntregaETCString,
          registroCompletoEntregaEtc: element.registroCompletoEntregaEtc ? 'Completo' : 'Incompleto',
          informeFinalId: element.informeFinalId
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
    this.paginator._intl.itemsPerPageLabel = 'Elementos por p??gina';
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

  enviarProyectoAEtc(informeFinalId: number) {
    this.registerProjectETCService.sendProjectToEtc(informeFinalId)
      .subscribe(respuesta => {
        this.openDialog('', '<b>La informaci??n ha sido guardada exitosamente.</b>');
        this.routes.navigateByUrl( '/', {skipLocationChange: true} )
        .then( () => this.routes.navigate( ['/registrarTransferenciaProyectosETC'] ) );
      });
  }

}