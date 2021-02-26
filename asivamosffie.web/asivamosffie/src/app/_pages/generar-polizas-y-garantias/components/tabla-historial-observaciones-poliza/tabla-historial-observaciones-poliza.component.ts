import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { PolizaGarantiaService } from 'src/app/core/_services/polizaGarantia/poliza-garantia.service';

@Component({
  selector: 'app-tabla-historial-observaciones-poliza',
  templateUrl: './tabla-historial-observaciones-poliza.component.html',
  styleUrls: ['./tabla-historial-observaciones-poliza.component.scss']
})
export class TablaHistorialObservacionesPolizaComponent implements OnInit {
  displayedColumns: string[] = ['fechaRevision', 'observacion', 'estadoRevisionCodigo'];
  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, {static: true} ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort: MatSort;
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  dataTable: any[] = [];
  loadTableObservaciones: Subscription;
  constructor(private polizaService: PolizaGarantiaService) { }

  ngOnInit(): void {
    this.cargarTablaDeDatos();
  }
  cargarTablaDeDatos(){
    this.loadTableObservaciones = this.polizaService.loadTableObservaciones.subscribe((loadTableObservaciones: any) => {
      console.log( loadTableObservaciones );
      if(loadTableObservaciones!=''){
        this.dataTable=loadTableObservaciones;
        this.dataSource = new MatTableDataSource(this.dataTable);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      }
    }); 
  }
}
