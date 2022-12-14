import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadosSolicitud } from 'src/app/_interfaces/project-contracting';
import { DialogObservacionesComponent } from '../dialog-observaciones/dialog-observaciones.component';
import { EstadoSolicitudContratacion, EstadoSolicitudContratacionCodigo } from 'src/app/_interfaces/estados-solicitudContratacion.interface';


@Component({
  selector: 'app-table-solicitud-contratacion',
  templateUrl: './table-solicitud-contratacion.component.html',
  styleUrls: ['./table-solicitud-contratacion.component.scss']
})
export class TableSolicitudContratacionComponent implements OnInit {

  estadosSolicitud = EstadosSolicitud;

  displayedColumns: string[] = [
    'fecha',
    'numero',
    'opcionPorContratar',
    'estadoSolicitud',
    'estadoDelIngreso',
    'id'
  ];
  dataSource = new MatTableDataSource();
  estadosContratacion: EstadoSolicitudContratacion = EstadoSolicitudContratacionCodigo;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
    private projectContractingService: ProjectContractingService,
    public dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.projectContractingService.getListContratacion().subscribe(response => {

      response.forEach( registro => registro[ 'fechaCreacion' ] = registro[ 'fechaCreacion' ].split('T')[0].split('-').reverse().join('/') );

      this.dataSource = new MatTableDataSource(response);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por p??gina';
      this.paginator._intl.nextPageLabel = 'Siguiente';
      this.paginator._intl.previousPageLabel = 'Anterior';
    });
  }

  detallarSolicitud(id: number) {
    console.log(id);
  }

  onDelete(id: number) {
    this.openDialogSiNo('', '<b>??Est?? seguro de eliminar este registro?</b>', id);
  }

  openDialogSiNo(modalTitle: string, modalText: string, e: number) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {

      if (result === true) {
        this.eliminarSolicitud(e);
      };
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  observacionesDialog ( numeroSolicitud: string, contratacionId: number ) {
    const dialogRef = this.dialog.open(DialogObservacionesComponent, {
      width: '60em',
      data: { numeroSolicitud, contratacionId }
    });
  }

  eliminarSolicitud(id: number) {
    console.log(id);
    this.projectContractingService.eliminarContratacion(id)
      .subscribe(respuesta => {
        this.openDialog('', '<b>La informaci??n ha sido eliminada correctamente.</b>');
        this.ngOnInit();
      });
  }

  enviarSolicitud(element: any) {
    if(element.esExpensa == true){
      //Si el tipo de intervenci??n de algun proyecto asociado al contrato es expensas no pasa por el comite tecnico
      // 13 = Aprobado por el comite fiduciario
      this.projectContractingService.changeStateContratacionByIdContratacion(element.contratacionId, '13')
      .subscribe(respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`);
        this.ngOnInit();
      });
    }else{
      this.projectContractingService.changeStateContratacionByIdContratacion(element.contratacionId, this.estadosSolicitud.RechazadaPorComiteTecnico)
      .subscribe(respuesta => {
        this.openDialog('', `<b>${respuesta.message}</b>`);
        this.ngOnInit();
      });
    }
  }

}
