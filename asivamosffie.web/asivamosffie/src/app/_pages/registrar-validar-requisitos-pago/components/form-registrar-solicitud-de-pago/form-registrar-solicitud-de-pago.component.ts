import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-form-registrar-solicitud-de-pago',
  templateUrl: './form-registrar-solicitud-de-pago.component.html',
  styleUrls: ['./form-registrar-solicitud-de-pago.component.scss']
})
export class FormRegistrarSolicitudDePagoComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'faseContrato',
    'pagosRealizados',
    'valorFacturado',
    'porcentajeFacturado',
    'saldoPorPagar',
    'porcentajePorPagar'
  ];
  dataTable: any[] = [
    {
      faseContrato: 'Fase 1 - Preconstrucción',
      pagosRealizados: '0',
      valorFacturado: '0',
      porcentajeFacturado: '0',
      saldoPorPagar: '$30.000.000',
      porcentajePorPagar: '100%',
    },
    {
      faseContrato: 'Fase 2 - Construcción',
      pagosRealizados: '0',
      valorFacturado: '0',
      porcentajeFacturado: '0',
      saldoPorPagar: '$75.000.000',
      porcentajePorPagar: '100%',
    }
  ];
  addressForm = this.fb.group({
    fechaSolicitud: [null, Validators.required],
    numeroRadicado: [null, Validators.required],
    faseContrato: [null, Validators.required]
  });
  fasesArray = [
    { name: 'Fase 1 - Preconstrucción', value: '1' },
    { name: 'Fase 2 - Construcción', value: '2' }
  ];
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  onSubmit() {

  }
}
