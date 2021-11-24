import { Component, OnInit, Input, AfterViewInit, ViewChild, Output, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { DialogObservacionesComponent } from '../dialog-observaciones/dialog-observaciones.component'
import { CargarFlujoComponent } from '../cargar-flujo/cargar-flujo.component';
import { ReprogrammingService } from 'src/app/core/_services/reprogramming/reprogramming.service';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { CommonService, Respuesta } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
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
  selector: 'app-flujo-intervencion-recursos',
  templateUrl: './flujo-intervencion-recursos.component.html',
  styleUrls: ['./flujo-intervencion-recursos.component.scss']
})
export class FlujoIntervencionRecursosComponent implements AfterViewInit, OnInit  {

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
  @Input() novedadContractualRegistroPresupuestal:any;
  @Input() plazoContratacion:any;
  @Input() valorContrato:number;
  @Output() estadoSemaforo = new EventEmitter<string>();
  existeRegistroValido = false;

  constructor(
    public dialog: MatDialog,
    private reprogrammingService : ReprogrammingService,
    private disponibilidadServices: DisponibilidadPresupuestalService,
    private commonSvc: CommonService,
    private router: Router
  ) { }

  ngOnInit(): void {
    if (this.ajusteProgramacionInfo?.ajusteProgramacionId !== 0 && this.ajusteProgramacionInfo?.ajusteProgramacionId !== undefined) {
      this.reprogrammingService.getLoadAdjustInvestmentFlowGrid(this.ajusteProgramacionInfo?.ajusteProgramacionId)
        .subscribe((response: any[]) => {
          //response = response.filter( p => p.estadoCargue == 'Válidos' )
          if(response.length > 0){
            this.dataSource = new MatTableDataSource(response);
            response.forEach(r =>{
              if(!this.existeRegistroValido){
                if(r.estadoCargue == 'Válido')
                  this.existeRegistroValido = true;
              }
            });
            if(this.existeRegistroValido == true){
              this.estadoSemaforo.emit( 'completo' );
            }else{
              this.estadoSemaforo.emit( 'sin-diligenciar' );
            }
          }
          this.dataSource = new MatTableDataSource(response);
        })
    };
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  }

  openCargarFlujo() {
    const dialogRef = this.dialog.open(CargarFlujoComponent, {
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

  onClose(): void {
    this.dialog.closeAll();
  }

  openObservaciones(dataFile: any) {
    const dialogCargarProgramacion = this.dialog.open(DialogObservacionesComponent, {
      width: '75em',
       data: { esObra: false, ajusteProgramacionInfo: this.ajusteProgramacionInfo, dataFile: dataFile}
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
      this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => this.router.navigate(['/registrarAjusteProgramacion']));
    });
  }

  ngAfterViewInit() {
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  descargarDRPBoton() {
    this.disponibilidadServices.GenerateDRP(this.novedadContractualRegistroPresupuestal?.disponibilidadPresupuestalId, true, this.novedadContractualRegistroPresupuestal?.novedadContractualRegistroPresupuestalId, false).subscribe((response: any) => {
      const documento = `${this.novedadContractualRegistroPresupuestal?.numeroDrp}.pdf`;
      const text = documento,
        blob = new Blob([response], { type: 'application/pdf' }),
        anchor = document.createElement('a');
      anchor.download = documento;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
      anchor.click();
    });

  }

  descargar(element: any) {
    this.commonSvc.getFileById(element.archivoCargueId)
      .subscribe(respuesta => {
        const documento = 'FlujoInversion.xlsx';
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
