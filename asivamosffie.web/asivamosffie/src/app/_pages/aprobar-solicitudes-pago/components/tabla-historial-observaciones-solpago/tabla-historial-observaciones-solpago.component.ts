import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-historial-observaciones-solpago',
  templateUrl: './tabla-historial-observaciones-solpago.component.html',
  styleUrls: ['./tabla-historial-observaciones-solpago.component.scss']
})
export class TablaHistorialObservacionesSolpagoComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaRevision',
    'responsable',
    'observaciones'
  ];
  dataTable: any[] = [
{
  fechaRevision: '05/12/2020',
  responsable: 'Supervisor',
  observaciones: 'La forma de pago de la fase 2 construcción también es 50/50, ajuste este dato ya que una vez aprobamos este primer pago queda predeterminado para el resto de las facturas y pagos.'
}
  ];
  constructor() { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

}
