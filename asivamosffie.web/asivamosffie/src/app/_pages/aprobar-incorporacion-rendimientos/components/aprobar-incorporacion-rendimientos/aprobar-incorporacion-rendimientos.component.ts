import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { FaseDosPagosRendimientosService } from 'src/app/core/_services/faseDosPagosRendimientos/fase-dos-pagosRendimientos.service';
import { FileDownloader } from 'src/app/_helpers/file-downloader';
import { DialogCargarActaFirmadaAirComponent } from '../dialog-cargar-acta-firmada-air/dialog-cargar-acta-firmada-air.component';


@Component({
  selector: 'app-aprobar-incorporacion-rendimientos',
  templateUrl: './aprobar-incorporacion-rendimientos.component.html',
  styleUrls: ['./aprobar-incorporacion-rendimientos.component.scss']
})
export class AprobarIncorporacionRendimientosComponent implements OnInit {
  verAyuda = false;
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaCargue',
    'registrosIncorporados',
    'gestion'
  ];
  
  constructor(public dialog: MatDialog,
    private faseDosPagosRendimientosSvc: FaseDosPagosRendimientosService) { }

  ngOnInit(): void {
    this.loadDataSource();
  }

  loadDataSource(){
    this.faseDosPagosRendimientosSvc.getRequestedApprovalPerformances()
    .subscribe((data)=>{
      this.setDataSource(data);
    })
  }

  setDataSource(data: any[]){
    this.dataSource = new MatTableDataSource(data);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
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
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  
  cargarActaFirmada(uploadedOrderId: number){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '50%';
    dialogConfig.data = uploadedOrderId
    const dialogRef = this.dialog.open(DialogCargarActaFirmadaAirComponent, dialogConfig);
  }

  includePerformances(uploadedOrder: any){
    this.faseDosPagosRendimientosSvc.includePerformances(uploadedOrder.registerId)
      .subscribe((response: Respuesta)=>{
        if(response.isSuccessful){
          uploadedOrder.registrosIncorporados = response.data;
        }
      });
  }

  downloadConsistencies(uploadedOrderId: number){
    this.faseDosPagosRendimientosSvc.downloadManagedPerformances(uploadedOrderId, true)
    .subscribe((content)=>{
      FileDownloader.exportExcel("Consistencias.xlsx", content)
    });
  }

  viewAddedRegister(uploadedOrderId: number){
    this.faseDosPagosRendimientosSvc.downloadApprovedIncorporatedPerformances(uploadedOrderId)
    .subscribe((content)=>{
      FileDownloader.exportExcel("RendimientosIncorporados.xlsx", content)
    });
  }
  generateMinute(uploadedOrderId: number){
    this.faseDosPagosRendimientosSvc.generateMinute(uploadedOrderId)
    .subscribe((content)=>{
      console.log(content)
      FileDownloader.exportPDF("ActaRendimientos.pdf", content)
    });
  }

  uploadSignedMinutes(){

  }

  downloadTemplateMinutes(){

  }



}
