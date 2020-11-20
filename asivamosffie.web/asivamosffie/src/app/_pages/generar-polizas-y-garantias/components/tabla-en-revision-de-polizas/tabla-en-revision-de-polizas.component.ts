import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';

@Component({
  selector: 'app-tabla-en-revision-de-polizas',
  templateUrl: './tabla-en-revision-de-polizas.component.html',
  styleUrls: ['./tabla-en-revision-de-polizas.component.scss']
})
export class TablaEnRevisionDePolizasComponent implements OnInit {
  @Output() estadoSemaforo1 = new EventEmitter<string>();
  displayedColumns: string[] = ['fechaFirma', 'numeroContrato', 'tipoSolicitud', 'estadoPoliza', 'contratoId'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  dataTable: any[] = [];
  loadDataItems: Subscription;
  constructor(private polizaService: PolizaGarantiaService) { }

  ngOnInit(): void {
    /*
    this.loadDataItems = this.polizaService.loadDataItems.subscribe((loadDataItems: any) => {
      if(loadDataItems!=''){
      this.dataTable=loadDataItems;
      }
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
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
    }); */
    this.polizaService.GetListGrillaContratoGarantiaPoliza().subscribe((resp: any) => {
      let enrevisionInc = 0;
      let enrevisionC  = 0;
      for (let polizas of resp) {
        if (polizas.estadoPoliza === 'En revisión de pólizas' && polizas.registroCompletoPolizaNombre=='Incompleto') {
          this.dataTable.push(polizas);
          enrevisionInc++;
        };
        if (polizas.estadoPoliza === 'En revisión de pólizas' && polizas.registroCompletoPolizaNombre=='Completo') {
          this.dataTable.push(polizas);
          enrevisionC++;
        };
      };
      if (enrevisionInc === this.dataTable.length && enrevisionC==0) {
        this.estadoSemaforo1.emit('sin-diligenciar');
      };
      if (enrevisionC === this.dataTable.length && enrevisionInc==0) {
        this.estadoSemaforo1.emit('completo');
      };
      if (enrevisionC > 0 && enrevisionInc > 0) {
        this.estadoSemaforo1.emit('en-proceso');
      };
      if(this.dataTable.length == 0){
        this.estadoSemaforo1.emit('completo');
      };
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
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
    });
  }
  applyFilter ( event: Event ) {
    const filterValue      = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  aprobarPoliza(id){
    this.polizaService.CambiarEstadoPolizaByContratoId("4",id).subscribe(resp1=>{
      if(resp1.isSuccessful==true){
        this.polizaService.GetListGrillaContratoGarantiaPoliza().subscribe(data0=>{
          this.polizaService.loadDataItems.next(data0);
        });
        this.polizaService.AprobarContratoByIdContrato(id).subscribe(data1=>{
          console.log("envió correo");
        });
      }
    });
  }
}
