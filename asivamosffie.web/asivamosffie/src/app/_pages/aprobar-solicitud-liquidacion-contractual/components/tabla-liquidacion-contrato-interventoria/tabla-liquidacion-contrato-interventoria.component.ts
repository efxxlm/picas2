import { Component, AfterViewInit, ViewChild, OnInit, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { RegisterContractualLiquidationRequestService } from 'src/app/core/_services/registerContractualLiquidationRequest/register-contractual-liquidation-request.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { EstadosSolicitudLiquidacionContractual, EstadosSolicitudLiquidacionContractualCodigo, ListaMenuSolicitudLiquidacion, ListaMenuSolicitudLiquidacionId } from 'src/app/_interfaces/estados-solicitud-liquidacion-contractual';

@Component({
  selector: 'app-tabla-liquidacion-contrato-interventoria',
  templateUrl: './tabla-liquidacion-contrato-interventoria.component.html',
  styleUrls: ['./tabla-liquidacion-contrato-interventoria.component.scss']
})
export class TablaLiquidacionContratoInterventoriaComponent implements OnInit, AfterViewInit {

  ELEMENT_DATA: any[] = [];
  dataSource = new MatTableDataSource(this.ELEMENT_DATA);
  @Output() estadoSemaforo = new EventEmitter<string>();

  displayedColumns: string[] = [
    'fechaValidacionLiquidacion',
    'fechaPoliza',
    'numeroSolicitudLiquidacion',
    'numeroContrato',
    'valorSolicitud',
    'proyectosAsociados',
    'estadoAprobacionLiquidacionString',
    'contratacionId'
  ];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  datosTabla = [];
  listaMenu: ListaMenuSolicitudLiquidacion = ListaMenuSolicitudLiquidacionId;
  listaEstadoLiquidacionSolicitud: EstadosSolicitudLiquidacionContractual = EstadosSolicitudLiquidacionContractualCodigo;
  estadoCodigos = {
    enProcesoDeAprobacion: '1',
    conAprobacion: '2',
    enviadoControlSeguimiento: '3'
  }
  constructor(
    private registerContractualLiquidationRequestService: RegisterContractualLiquidationRequestService,
    private dialog: MatDialog,
    private routes: Router
    ) { }

  ngOnInit(): void {
    this.getListContractualLiquidationInterventoria(this.listaMenu.aprobarSolicitudLiquidacionContratacion);
  }

  getListContractualLiquidationInterventoria(menuId: number) {
    this.registerContractualLiquidationRequestService.getListContractualLiquidationInterventoria(menuId).subscribe(report => {
      if(report != null){
        //semaforo
        let sinDiligenciar = 0;
        let enProceso = 0;
        let completo = 0;
        report.forEach(element => {
          this.datosTabla.push({
            fechaValidacionLiquidacion : element.fechaValidacionLiquidacion.split('T')[0].split('-').reverse().join('/'),
            fechaPoliza : element.fechaPoliza.split('T')[0].split('-').reverse().join('/'),
            numeroContrato: element.numeroContrato,
            valorSolicitud: element.valorSolicitud,
            proyectosAsociados: element.proyectosAsociados,
            estadoAprobacionLiquidacionString: element.estadoAprobacionLiquidacionString,
            estadoAprobacionLiquidacionCodigo: element.estadoAprobacionLiquidacionCodigo,
            numeroSolicitudLiquidacion: element.numeroSolicitudLiquidacion,
            contratacionId: element.contratacionId
          });
          if (element.estadoAprobacionLiquidacionCodigo === this.estadoCodigos.enProcesoDeAprobacion || element.estadoAprobacionLiquidacionCodigo === this.estadoCodigos.conAprobacion ) {
            enProceso++;
          }else if(element.estadoAprobacionLiquidacionCodigo === this.estadoCodigos.enviadoControlSeguimiento){
            completo++;
          }else{
            sinDiligenciar++;
          }
        });
        //semaforo
        if ( sinDiligenciar === this.datosTabla.length ) {
          this.estadoSemaforo.emit( 'sin-diligenciar' );
        };

        if ( enProceso === this.datosTabla.length ) {
          this.estadoSemaforo.emit( 'en-proceso' );
        };

        if ( completo === this.datosTabla.length ) {
          this.estadoSemaforo.emit( 'completo' );
        };

        if ( ( sinDiligenciar > 0 && sinDiligenciar < this.datosTabla.length ) && ( enProceso > 0 && enProceso < this.datosTabla.length ) ) {
          this.estadoSemaforo.emit( 'sin-diligenciar' );
        };

        if ( this.datosTabla.length === 0 ) {
          this.estadoSemaforo.emit( 'completo' );
        }
      }
      this.dataSource.data = this.datosTabla;
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

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open( ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  SendToNovedades( pContratacionId: number ) {
    const pContratacion = {
        contratacionId: pContratacionId,
        estadoAprobacionLiquidacionCodigo: this.listaEstadoLiquidacionSolicitud.enviadoControlSeguimiento
    };

    this.registerContractualLiquidationRequestService.changeStatusLiquidacionContratacion( pContratacion, this.listaMenu.aprobarSolicitudLiquidacionContratacion )
        .subscribe(
            response => {
                this.openDialog( '', `<b>${ response.message }</b>` );
                this.routes.navigateByUrl( '/', {skipLocationChange: true} )
                .then( () => this.routes.navigate( ['/aprobarSolicitudLiquidacionContractual'] ) );
            }, err => this.openDialog( '', `<b>${ err.message }</b>` )
        );
    }


}