import { Input, Output, ViewChild, EventEmitter } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DataSesion, SolicitudContractual, ColumnasTabla, DataTable } from '../../../../_interfaces/comiteFiduciario.interfaces';

@Component({
  selector: 'app-tabla-sesiones-temas',
  templateUrl: './tabla-sesiones-temas.component.html',
  styleUrls: ['./tabla-sesiones-temas.component.scss']
})
export class TablaSesionesTemasComponent implements OnInit {

  dataSource = new MatTableDataSource();
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  displayedColumns: string[] = [ 'nombreResponsable', 'tiempoIntervencion', 'tema', 'seleccionar' ];

  @Input() dataSolicitud: any;

  columnas: ColumnasTabla[] = [
    { titulo: 'Responsable', name: 'nombreResponsable' },
    { titulo: 'Tiempo', name: 'tiempoIntervencion' },
    { titulo: 'Tema solicitud', name: 'tema' },
  ];
  //EventEmitter para emitir las sesiones contractuales seleccionadas
  @Output() temasSeleccionados = new EventEmitter<DataTable>();
  solicitud: any;

  constructor() { };

  ngOnInit(): void {
    console.log(this.dataSolicitud);
    this.solicitud = this.dataSolicitud;
    this.dataSource = new MatTableDataSource( this.dataSolicitud.temas );
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  };

  //Metodo para emitir las sesiones seleccionadas
  getSesionSeleccionada ( event: boolean, sesion: any ) {
    this.dataSolicitud.temas.find(r => r.sesionTemaId == sesion.sesionTemaId).seleccionado = event;
    this.temasSeleccionados.emit( this.dataSolicitud );
  };

};
