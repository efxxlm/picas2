import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';


@Component({
  selector: 'app-tabla-con-poliza-observada-y-devuelta',
  templateUrl: './tabla-con-poliza-observada-y-devuelta.component.html',
  styleUrls: ['./tabla-con-poliza-observada-y-devuelta.component.scss']
})
export class TablaConPolizaObservadaYDevueltaComponent implements OnInit {
  displayedColumns: string[] = ['fechaFirma', 'numeroContrato', 'tipoSolicitud', 'estadoPoliza', 'contratoId'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  public dataTable;
  constructor(private polizaService: PolizaGarantiaService) { }

  ngOnInit(): void {
    this.polizaService.GetListGrillaContratoGarantiaPoliza().subscribe(data=>{
      this.dataTable = data;
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
      this.applyFilter("Con póliza observada y devuelta");
    });    
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue;
  }

}
