import { Component, OnInit, Input, AfterViewInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CargarProgramacionComponent } from '../cargar-programacion/cargar-programacion.component';
import { DialogObservacionesComponent } from '../dialog-observaciones/dialog-observaciones.component'
import { ReprogrammingService } from 'src/app/core/_services/reprogramming/reprogramming.service';
import { CommonService, Respuesta } from 'src/app/core/_services/common/common.service';
import { Router } from '@angular/router';

export interface VerificacionDiaria {
  id: string;
  fechaCargue: string;
  numeroToalRegistros: string;
  numeroRegistrosValidos: string;
  numeroRegistrosInalidos: string;
  estadoCargue: string;
}

const ELEMENT_DATA: VerificacionDiaria[] = [];

@Component({
  selector: 'app-programacion-de-obra',
  templateUrl: './programacion-de-obra.component.html',
  styleUrls: ['./programacion-de-obra.component.scss']
})
export class ProgramacionDeObraComponent implements AfterViewInit, OnInit  {

  displayedColumns: string[] = [
    'fechaCreacion',
    'cantidadRegistros',
    'cantidadRegistrosValidos',
    'cantidadRegistrosInvalidos',
    'estadoCargue',
    'id'
  ];
  dataSource = new MatTableDataSource(ELEMENT_DATA);

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  @Input() ajusteProgramacionInfo:any;

  constructor(
    public dialog: MatDialog,
    private reprogrammingService : ReprogrammingService,
    private commonSvc: CommonService,
    private router: Router,
  ) { }

  ngOnInit(): void {
    if (this.ajusteProgramacionInfo?.ajusteProgramacionId !== 0 && this.ajusteProgramacionInfo?.ajusteProgramacionId !== undefined) {
      this.reprogrammingService.getLoadAdjustProgrammingGrid(this.ajusteProgramacionInfo?.ajusteProgramacionId)
        .subscribe((response: any[]) => {
          //response = response.filter( p => p.estadoCargue == 'Válidos' )
          this.dataSource = new MatTableDataSource(response);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
          this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        })
    };
  }

  openCargarProgramacion() {
    const dialogRef = this.dialog.open(CargarProgramacionComponent, {
      width: '75em',
       data: { ajusteProgramacionInfo: this.ajusteProgramacionInfo }
    });
    dialogRef.afterClosed()
    .subscribe(response => {
      if (response) {
        console.log(response);
      };
    })
  }

  openObservaciones(dataFile: any) {
    const dialogCargarProgramacion = this.dialog.open(DialogObservacionesComponent, {
      width: '75em',
       data: { esObra: true, ajusteProgramacionInfo: this.ajusteProgramacionInfo, dataFile: dataFile}
    });
    dialogCargarProgramacion.afterClosed()
      .subscribe(response => {
        if (response) {
          console.log(response);
        };
      })
  }

  openDialogTrueFalse(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });

    return dialogRef.afterClosed();
  }


  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
     this.router.navigate(['/registrarAjusteProgramacion'], {});
    });
  }

  ngAfterViewInit() {
  }

  onClose(): void {
    this.dialog.closeAll();
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  descargar(element: any) {
    this.commonSvc.getFileById(element.archivoCargueId)
      .subscribe(respuesta => {
        const documento = 'ProgramacionObra.xlsx';
        const  blob = new Blob([respuesta], { type: 'application/octet-stream' });
        const  anchor = document.createElement('a');
        anchor.download = documento;
        anchor.href = window.URL.createObjectURL(blob);
        anchor.dataset.downloadurl = ['application/octet-stream', anchor.download, anchor.href].join(':');
        anchor.click();
      });
  }

  eliminar(element: any) {
    this.openDialogTrueFalse('', '<b>¿Está seguro de eliminar esta información?</b>').subscribe(value => {
      if (value === true) {
        this.reprogrammingService.deleteAdjustProgrammingOrInvestmentFlow(element.archivoCargueId, this.ajusteProgramacionInfo.ajusteProgramacionId)
        .subscribe((respuesta: Respuesta) => {
          this.onClose();
          this.openDialog('', respuesta.message);
          return;
        })
      }
    });
  }

}
