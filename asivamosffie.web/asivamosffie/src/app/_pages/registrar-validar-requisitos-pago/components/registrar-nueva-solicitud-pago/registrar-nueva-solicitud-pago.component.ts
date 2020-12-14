import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-registrar-nueva-solicitud-pago',
  templateUrl: './registrar-nueva-solicitud-pago.component.html',
  styleUrls: ['./registrar-nueva-solicitud-pago.component.scss']
})
export class RegistrarNuevaSolicitudPagoComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'drp',
    'numDrp',
    'valor',
    'saldo'
  ];
  dataTable: any[] = [
    {
      drp: '1',
      numDrp: 'IP_00090',
      valor: '$100.000.000',
      saldo: '$100.000.000'
    },
    {
      drp: '2',
      numDrp: 'IP_00123',
      valor: '$5.000.000',
      saldo: '$5.000.000'
    }
  ];

  addressForm = this.fb.group({
    tipoSolicitud: [null, Validators.required],
    modalidadContrato: [null, Validators.required],
    numeroContrato: [null, Validators.required],
  });
  tiposSolicitudArray = [
    { name: 'Contratos de obra', value: '1' },
    { name: 'Contratos de interventoria', value: '2' },
    { name: 'Expensas', value: '3' },
    { name: 'Otros costos/servicios', value: '4' }
  ];
  modalidadContratoArray = [
    { name: 'Tipo A', value: '1' },
    { name: 'Tipo B', value: '2' },
    { name: 'Mejoramiento', value: '3' }
  ];
  contratosArray = [
    { name: 'N801801', value: '1' }
  ];
  contratoId: any;
  constructor(private fb: FormBuilder, public dialog: MatDialog) { }


  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };

  seleccionAutocomplete(id:any){
    this.addressForm.value.numeroContrato = id;
    this.contratoId = id;
  }
}
