import { Component, OnInit, ViewChild } from '@angular/core'
import { MatDialog, MatDialogConfig } from '@angular/material/dialog'
import { MatPaginator } from '@angular/material/paginator'
import { MatSort } from '@angular/material/sort'
import { MatTableDataSource } from '@angular/material/table'
import { DialogCargarReportPagosRprComponent } from '../dialog-cargar-report-pagos-rpr/dialog-cargar-report-pagos-rpr.component'
import { ObservacionesReportPagoRprComponent } from '../observaciones-report-pago-rpr/observaciones-report-pago-rpr.component'
import { FaseDosPagosRendimientosService } from '../../../../core/_services/faseDosPagosRendimientos/fase-dos-pagosRendimientos.service'
import { FileDownloader } from 'src/app/_helpers/file-downloader'
import exportFromJSON from 'export-from-json'

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
      .getPaymentsPerformances(this.tipoCargue)
      .subscribe((response: any[]) => {
        if (response.length === 0) {
          return
        }
        this.dataSource = new MatTableDataSource(response)
        this.dataSource.paginator = this.paginator
        this.dataSource.sort = this.sort
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
    const filterValue = (event.target as HTMLInputElement).value
    this.dataSource.filter = filterValue.trim().toLowerCase()
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

  deleteUpload(uploadPaymentId: number){
    this.faseDosPagosRendimientosSvc.setPaymentsPerformanceStatus(uploadPaymentId)
     .subscribe(( isDeleted)=>{
        console.log("Eliminado", isDeleted)
        this.getData()
    },onError => {
      
    });
  }

  viewDetails(uploadPaymentId: number){
    this.faseDosPagosRendimientosSvc.downlaodPaymentsPerformanceStatus(uploadPaymentId)
    .subscribe((content: any)=>{
      const data = content.data.archivoJson;

      const fileName = content.data.nombreArchivo
      const exportType = 'xls'
 
      exportFromJSON({ data, fileName, exportType, withBOM: true })
    },onError => {
      
    });
    // FileDownloader.exportExcel("RegistrarPagos.xlsx", {})
  }

  abrirObservaciones(cargaPagosRendimientosId: number) {
    const dialogConfig = new MatDialogConfig()
    dialogConfig.height = 'auto'
    dialogConfig.width = '865px'
    const dialogRef = this.dialog.open(
      ObservacionesReportPagoRprComponent,
      dialogConfig
    )

    dialogRef.afterClosed().subscribe((result) => {
      let data = {
        observaciones: result.data,
        typeFile: this.tipoCargue,
        cargaPagosRendimientosId: cargaPagosRendimientosId
      }

      this.faseDosPagosRendimientosSvc
        .setObservationPaymentsPerformances(data)
        .subscribe((response: any[]) => {
          this.getData()
        })
    })
  }
}
