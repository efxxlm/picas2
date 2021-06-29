import { Component, OnInit, ViewChild } from '@angular/core'
import { MatDialog, MatDialogConfig } from '@angular/material/dialog'
import { MatPaginator } from '@angular/material/paginator'
import { MatSort } from '@angular/material/sort'
import { MatTableDataSource } from '@angular/material/table'
import { DialogCargarReportPagosRprComponent } from '../dialog-cargar-report-pagos-rpr/dialog-cargar-report-pagos-rpr.component'
import { ObservacionesReportPagoRprComponent } from '../observaciones-report-pago-rpr/observaciones-report-pago-rpr.component'
import { FaseDosPagosRendimientosService } from '../../../../core/_services/faseDosPagosRendimientos/fase-dos-pagosRendimientos.service'
import { FileDownloader } from 'src/app/_helpers/file-downloader'
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component'
import { filter } from 'rxjs/internal/operators/filter'
import { switchMap } from 'rxjs/operators'
import { Respuesta } from 'src/app/core/_services/common/common.service'

export interface Payment{
  fechaCargue: string
  totalRegistros: number
  registrosValidos: number
  registrosInvalidos: number
  estadoCargue: string
}
@Component({
  selector: 'app-tabla-registrar-pagos-rpr',
  templateUrl: './tabla-registrar-pagos-rpr.component.html',
  styleUrls: ['./tabla-registrar-pagos-rpr.component.scss']
})
export class TablaRegistrarPagosRprComponent implements OnInit {
  dataSource = new MatTableDataSource()
  tipoCargue = 'Pagos'
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator
  @ViewChild(MatSort, { static: true }) sort: MatSort
  displayedColumns: string[] = [
    'fechaCargue',
    'totalRegistros',
    'registrosValidos',
    'registrosInvalidos',
    'estadoCargue',
    'gestion'
  ]
  constructor(
    public dialog: MatDialog,
    private faseDosPagosRendimientosSvc: FaseDosPagosRendimientosService
  ) {}

  ngOnInit(): void {
    this.getData()
  }

  getData() {
    this.faseDosPagosRendimientosSvc
      .getPayments()
      .subscribe((response: any[]) => {
        if (response.length === 0) {
          return
        }
        response.sort((a, b) => {
          if (a.fechaUltimoReporte > b.fechaUltimoReporte) return 1;
          if (a.fechaUltimoReporte < b.fechaUltimoReporte) return -1;
          return 0;
        });
        response.forEach(element => {
          element.fechaCargue = element.fechaCargue
            ? element.fechaCargue.split('T')[0].split('-').reverse().join('/')
            : '';
        });
        this.dataSource = new MatTableDataSource(response)
        this.dataSource.paginator = this.paginator
        this.dataSource.sort = this.sort


        this.dataSource.sortingDataAccessor = (item: Payment , property): string | number => {
          switch (property) {
            case 'fechaCargue': return new Date(item.fechaCargue).toString();
            default: return item[property];
          }
        };

        this.paginator._intl.itemsPerPageLabel = 'Elementos por página'

        this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
          if (length === 0 || pageSize === 0) {
            return '0 de ' + length
          }
          length = Math.max(length, 0)
          const startIndex = page * pageSize
          // If the start index exceeds the list length, do not try and fix the end index to the end.
          const endIndex =
            startIndex < length
              ? Math.min(startIndex + pageSize, length)
              : startIndex + pageSize
          return startIndex + 1 + ' - ' + endIndex + ' de ' + length
        }
      })
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  cargarNuevoReportedePago() {
    const dialogConfig = new MatDialogConfig()
    dialogConfig.height = 'auto'
    dialogConfig.width = '865px'
    const dialogRef = this.dialog.open(
      DialogCargarReportPagosRprComponent,
      dialogConfig
    )

    dialogRef.afterClosed().subscribe((result) => {
      this.getData()
    })
  }

   openDialogSiNo(modalTitle: string, modalText: string, uploadPaymentId:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton:true }
    });   
    const onConfirmDelete = dialogRef.afterClosed().pipe(filter(result => result));
    
    onConfirmDelete.subscribe( X =>{
        this.faseDosPagosRendimientosSvc.deletePayment(uploadPaymentId)
          .subscribe(( isDeleted)=>{
          this.getData()
          },onError => {
            
          });
      });
  }


  deleteUploadConfirm(uploadPaymentId: number){
    this.openDialogSiNo('','<b>¿Está seguro de eliminar este registro?</b>',uploadPaymentId);    
  }

  viewDetails(uploadPaymentId: number){
    const fileRequest = {resourceId: uploadPaymentId, fileName: "Payment"}
    this.faseDosPagosRendimientosSvc.downloadPayments(uploadPaymentId)
    .subscribe((content: any)=>{
      console.log(content);
       FileDownloader.exportExcel("RegistrarPagos.xlsx", content)
    },onError => {
      
    });
    
  }

  downloadTemplate(){
    location.href ="./assets/files/TemplateRegistrarPagos.xlsx";
  }

  abrirObservaciones(carguePagoId: number, element: any) {
    const dialogConfig = new MatDialogConfig()
    dialogConfig.height = 'auto'
    dialogConfig.width = '865px';
    dialogConfig.data =  element;
    const dialogRef = this.dialog.open(
      ObservacionesReportPagoRprComponent,
      dialogConfig
    )

    dialogRef.afterClosed().pipe(
      filter(result => result !=undefined && result.data !=undefined && element.observaciones !== result.data)
      , switchMap( (result:any) => { 
         const data = {
          observaciones: result.data,
          typeFile: this.tipoCargue,
          carguePagoId: carguePagoId
        }
        return this.faseDosPagosRendimientosSvc.setObservationPayments(data)
      })
    ).subscribe((response: Respuesta) => {
        if(response.isSuccessful){
          this.openMessageInformation('', '<b>La información ha sido guardada exitosamente</b>')
          this.getData()
        }
    });
  }

  private openMessageInformation(modalTitle: string, modalText: string){
    this.dialog.open(ModalDialogComponent, {
      width: '40em',
      data: { modalTitle, modalText }
    })
  }
}
