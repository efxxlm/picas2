import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-detalle-validar-listachequeo',
  templateUrl: './tabla-detalle-validar-listachequeo.component.html',
  styleUrls: ['./tabla-detalle-validar-listachequeo.component.scss']
})
export class TablaDetalleValidarListachequeoComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'item',
    'documento',
    'revTecnica'
  ];
  dataTable: any[] = [
    {
      item: 1,
      documento: 'Certificación de supervisión de aprobación de pago de interventoría suscrita por el contratista y el Supervisor en original, en el cual se evidencie el avance porcentual de obra',
      revTecnica: 'Sí cumple'
    },
    {
      item: 2,
      documento: 'Copia de la resolución de facturación vigente',
      revTecnica: 'Sí cumple'
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
