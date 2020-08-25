import { Input, Output, ViewChild, EventEmitter } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { DataSesion, SolicitudContractual, ColumnasTabla, DataTable } from '../../../../_interfaces/comiteFiduciario.interfaces';

@Component({
  selector: 'app-tabla-sesiones',
  templateUrl: './tabla-sesiones.component.html',
  styleUrls: ['./tabla-sesiones.component.scss']
})
export class TablaSesionesComponent implements OnInit {

  dataSource = new MatTableDataSource();
  //Decoradores ViewChild para controlar "MatPaginator" y "MatSort" del componente
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  //Inputs para recibir la data de la tabla
  displayedColumns: string[] = [ 'fechaSolicitud', 'numeroSolicitud', 'tipoSolicitud', 'seleccionar' ];
  @Input() dataSolicitud: SolicitudContractual;
  columnas: ColumnasTabla[] = [
    { titulo: 'Fecha de la solicitud', name: 'fechaSolicitud' },
    { titulo: 'Número de solicitud', name: 'numeroSolicitud' },
    { titulo: 'Tipo de solicitud', name: 'tipoSolicitud' },
  ];
  //EventEmitter para emitir las sesiones contractuales seleccionadas
  @Output() sesionesSeleccionadas = new EventEmitter<DataTable>();
  solicitud: SolicitudContractual;

  constructor() { };

  ngOnInit(): void {
    this.solicitud = {
      nombreSesion: this.dataSolicitud.nombreSesion,
      fecha: this.dataSolicitud.fecha,
      data: []
    };
    this.dataSource = new MatTableDataSource( this.dataSolicitud.data );
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  };

  //Metodo para emitir las sesiones seleccionadas
  getSesionSeleccionada ( event: boolean, sesion: DataSesion ) {

    if ( event ) {
      this.solicitud.data.push( sesion )
    } else {
      const index = this.solicitud.data.findIndex( data => data.numeroSolicitud === sesion.numeroSolicitud );
      this.solicitud.data.splice( index, 1 );
    }

    const data = { estado: event, solicitud: this.solicitud };

    event ? this.sesionesSeleccionadas.emit( data ) : this.sesionesSeleccionadas.emit( data );

  };

};