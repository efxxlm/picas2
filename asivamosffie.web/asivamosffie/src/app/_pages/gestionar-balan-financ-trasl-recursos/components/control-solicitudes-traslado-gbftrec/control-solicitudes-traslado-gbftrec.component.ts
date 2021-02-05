import { Component, OnInit, ViewChild } from '@angular/core';
import { Validators, FormControl, FormBuilder } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-control-solicitudes-traslado-gbftrec',
  templateUrl: './control-solicitudes-traslado-gbftrec.component.html',
  styleUrls: ['./control-solicitudes-traslado-gbftrec.component.scss']
})
export class ControlSolicitudesTrasladoGbftrecComponent implements OnInit {
  addressForm = this.fb.group({
    tipoSolicitud: [ null, Validators.compose( [ Validators.minLength(3), Validators.maxLength(100) ] ) ],
    numeroOrdenGiro: [ null, Validators.compose( [ Validators.minLength(3), Validators.maxLength(100) ] ) ],
  });
  myFilter = new FormControl();
  myFilter2 = new FormControl();
  var1: any;
  var2: any;
  idContrato:any = null;
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  displayedColumns: string[] = [
    'tipoSolicitudGiro',
    'fechaAprobacionFiduciaria',
    'fechaPagoFiduciaria',
    'numeroOrdendeGiro',
    'modalidadContrato',
    'numeroContrato',
    'seleccion',
  ];
  dataTable: any[] = [
    {
      tipoSolicitudGiro: 'Obra',
      fechaAprobacionFiduciaria: '08/11/2020',
      fechaPagoFiduciaria: '17/11/2020',
      numeroOrdendeGiro: 'ODG_Obra_001',
      modalidadContrato: 'Modalidad 1',
      numeroContrato: 'N801801',
      contratacionProyectoId: 1
    },
    {
      tipoSolicitudGiro: 'Obra',
      fechaAprobacionFiduciaria: '10/11/2020',
      fechaPagoFiduciaria: '20/11/2020',
      numeroOrdendeGiro: 'ODG_Obra_326',
      modalidadContrato: 'Modalidad 1',
      numeroContrato: 'N801801',
      contratacionProyectoId: 2
    },
    {
      tipoSolicitudGiro: 'Obra',
      fechaAprobacionFiduciaria: '15/11/2020',
      fechaPagoFiduciaria: '22/11/2020',
      numeroOrdendeGiro: 'ODG_Obra_326',
      modalidadContrato: 'Modalidad 1',
      numeroContrato: 'N801801',
      contratacionProyectoId: 3
    },
  ];
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.loadDataSource();
  }
  loadDataSource() {
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
  }
  
  addProject(idproyecto:any)
  {
    this.idContrato = idproyecto;
  }
}
