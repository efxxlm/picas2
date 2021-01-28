import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { FaseDosPagosRendimientosService } from 'src/app/core/_services/faseDosPagosRendimientos/fase-dos-pagosRendimientos.service';
import { FileDownloader } from 'src/app/_helpers/file-downloader';

@Component({
  selector: 'app-gestionar-rendimientos',
  templateUrl: './gestionar-rendimientos.component.html',
  styleUrls: ['./gestionar-rendimientos.component.scss']
})
export class GestionarRendimientosComponent implements OnInit {
  verAyuda = false;
  uploadType = 'Rendimientos'
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaCargue',
    'totalRegistros', //'numTotalRegistros',
    'registrosConsistentes',
    'registrosInconsistentes',
    'gestion'
  ];
  dataTable: any[] = [
    {
      fechaCargue: '10/08/2020',
      numTotalRegistros: 300,
      registrosConsistentes: '',
      registrosInconsistentes: '',
      gestion: 1
    },
    {
      fechaCargue: '10/08/2020',
      numTotalRegistros: 23,
      registrosConsistentes: '0',
      registrosInconsistentes: '23',
      gestion: 2
    },
    {
      fechaCargue: '10/08/2020',
      numTotalRegistros: 326,
      registrosConsistentes: '300',
      registrosInconsistentes: '26',
      gestion: 2
    }
  ];
  constructor(public dialog: MatDialog,
    private faseDosPagosRendimientosSvc: FaseDosPagosRendimientosService) { }

  ngOnInit(): void {
    this.loadDataSource();
  }

  loadDataSource(){
    this.faseDosPagosRendimientosSvc
    .getPaymentsPerformances(this.uploadType, "Valido")
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

  managePerformance(uploadedOrderId: number){
    this.faseDosPagosRendimientosSvc
    .managePerformance(uploadedOrderId).subscribe((result)=>{

     this.loadDataSource();
    })
  }

  sendInconsistencies(uploadedOrderId: number, order){
    this.faseDosPagosRendimientosSvc.sendInconsistencies(uploadedOrderId)
      .subscribe((result: Respuesta)=>{
      order.ShowInconsistencies = result.isSuccessful
      //this.loadDataSource();
    })
  }

  downloadInconsistencies(uploadedOrderId: number){
    this.faseDosPagosRendimientosSvc.downloadPerformancesInconsistencies(uploadedOrderId)
    .subscribe((content)=>{
      // if(result.isSuccessful){
      //   console.log("Succss")
      // }
      // if(typeof result  != "string"){
      //   console.log("typeof", typeof result)
      // }
      FileDownloader.exportExcel("file", content)
      
    })
  }

  requestApproval(uploadedOrderId: number, order){
    this.faseDosPagosRendimientosSvc
    .requestApproval(uploadedOrderId).subscribe((result)=>{
      order.pendienteAprobacion = true;
     this.loadDataSource();
    })
  }

}
