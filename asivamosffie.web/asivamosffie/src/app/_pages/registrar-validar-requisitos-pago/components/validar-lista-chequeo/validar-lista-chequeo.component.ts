import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DialogObservacionesItemListchequeoComponent } from '../dialog-observaciones-item-listchequeo/dialog-observaciones-item-listchequeo.component';
import { DialogSubsanacionComponent } from '../dialog-subsanacion/dialog-subsanacion.component';

@Component({
  selector: 'app-validar-lista-chequeo',
  templateUrl: './validar-lista-chequeo.component.html',
  styleUrls: ['./validar-lista-chequeo.component.scss']
})
export class ValidarListaChequeoComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'item',
    'documento',
    'revTecnica',
    'observaciones'
  ];
  dataTable: any[] = [
    {
      item: 1,
      documento: 'Certificación de supervisión de aprobación de pago de interventoría suscrita por el contratista y el Supervisor en original, en el cual se evidencie el avance porcentual de obra',
      revTecnica: '',
      observaciones: ''
    },
    {
      item: 2,
      documento: 'Copia de la resolución de facturación vigente',
      revTecnica: '',
      observaciones: ''
    }
  ];
  booleanosRevisionTecnica: any[] = [
    { value: true, viewValue: 'Si cumple' },
    { value: false, viewValue: 'No cumple' }
  ]
  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  };

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  callObservaciones(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '865px';
    //dialogConfig.data = { id: id, idRol: idRol, numContrato: numContrato, fecha1Titulo: fecha1Titulo, fecha2Titulo: fecha2Titulo };
    const dialogRef = this.dialog.open(DialogObservacionesItemListchequeoComponent, dialogConfig);
    //dialogRef.afterClosed().subscribe(value => {});
  }
  callSubsanacion(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.height = 'auto';
    dialogConfig.width = '865px';
    //dialogConfig.data = { id: id, idRol: idRol, numContrato: numContrato, fecha1Titulo: fecha1Titulo, fecha2Titulo: fecha2Titulo };
    const dialogRef = this.dialog.open(DialogSubsanacionComponent, dialogConfig);
    //dialogRef.afterClosed().subscribe(value => {});
  }
}
