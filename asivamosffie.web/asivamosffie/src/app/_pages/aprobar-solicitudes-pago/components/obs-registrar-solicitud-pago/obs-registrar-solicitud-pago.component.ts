import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-obs-registrar-solicitud-pago',
  templateUrl: './obs-registrar-solicitud-pago.component.html',
  styleUrls: ['./obs-registrar-solicitud-pago.component.scss']
})
export class ObsRegistrarSolicitudPagoComponent implements OnInit {
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
  addressForm = this.fb.group({});
  editorStyle = {
    height: '45px',
    overflow: 'auto'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.addressForm = this.crearFormulario();
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  crearFormulario() {
    return this.fb.group({
      tieneObservaciones: [null, Validators.required],
      observaciones:[null, Validators.required],
    })
  }
  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }
  onSubmit() {
    console.log(this.addressForm.value);
  }
}
