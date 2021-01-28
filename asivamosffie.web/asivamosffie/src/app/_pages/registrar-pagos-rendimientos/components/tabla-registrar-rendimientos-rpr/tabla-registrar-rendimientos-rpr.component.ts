import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import exportFromJSON from 'export-from-json';
import { FaseDosPagosRendimientosService } from 'src/app/core/_services/faseDosPagosRendimientos/fase-dos-pagosRendimientos.service';
import { FileDownloader } from 'src/app/_helpers/file-downloader';
import { DialogCargarReportRendRprComponent } from '../dialog-cargar-report-rend-rpr/dialog-cargar-report-rend-rpr.component';

@Component({
  selector: 'app-tabla-registrar-rendimientos-rpr',
  templateUrl: './tabla-registrar-rendimientos-rpr.component.html',
  styleUrls: ['./tabla-registrar-rendimientos-rpr.component.scss']
})
export class TablaRegistrarRendimientosRprComponent implements OnInit {
  uploadType = 'Rendimientos'
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaCargue',
    'totalRegistros', //'numTotalRegistros',
    'registrosValidos', // 'numRegistrosValidos',
    'registrosInvalidos',// 'numRegistrosInvalidos',
    'registrosInconsistentes',
    'estadoCargue',
    'gestion'
  ];

  constructor(public dialog: MatDialog, private faseDosPagosRendimientosSvc: FaseDosPagosRendimientosService) { }

  ngOnInit(): void {
    // this.dataSource = new MatTableDataSource(this.dataTable);
    // this.dataSource.paginator = this.paginator;
    // this.dataSource.sort = this.sort;
    // this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    // this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
    //   if (length === 0 || pageSize === 0) {
    //     return '0 de ' + length;
    //   }
    //   length = Math.max(length, 0);
    //   const startIndex = page * pageSize;
    //   // If the start index exceeds the list length, do not try and fix the end index to the end.
    //   const endIndex = startIndex < length ?
    //     Math.min(startIndex + pageSize, length) :
    //     startIndex + pageSize;
    //   return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    // };
    this.loadDataSource();
  }

  loadDataSource(){
    this.faseDosPagosRendimientosSvc
    .getPaymentsPerformances(this.uploadType)
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
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };  

  deleteUpload(uploadPaymentId: number){
    this.faseDosPagosRendimientosSvc.deletePaymentsPerformanceStatus(uploadPaymentId)
     .subscribe(( isDeleted)=>{
        console.log("Eliminado", isDeleted)
        this.loadDataSource()
    },onError => {
      
    });
  }

  downloadInconsistencies(uploadedOrderId: number){
    this.faseDosPagosRendimientosSvc.downloadPerformancesInconsistencies(uploadedOrderId)
    .subscribe((result)=>{
      console.log(result);
     FileDownloader.exportExcel("file", result)
    })
  }


  viewDetails(uploadPaymentId: number){
    const fileRequest = {resourceId: uploadPaymentId, fileName: "Rendimientos"}
    this.faseDosPagosRendimientosSvc.downloadPaymentsPerformanceStatus(fileRequest, this.uploadType)
    .subscribe((content: any)=>{
      FileDownloader.exportExcel("Rendimientos.xlsx", content)
      // const data = content.data.archivoJson;

      // const fileName = content.data.nombreArchivo
      // const exportType = 'xls'
 
      // exportFromJSON({ data, fileName, exportType, withBOM: true })
    },onError => {
       console.log("error", onError);
    });
    // FileDownloader.exportExcel("RegistrarPagos.xlsx", {})
  }

  cargarNuevoReportedeRendimiento(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '865px';
    const dialogRef = this.dialog.open(DialogCargarReportRendRprComponent, dialogConfig);

      dialogRef.afterClosed().subscribe((result) => {
        this.loadDataSource()
      })
    
  }
}
