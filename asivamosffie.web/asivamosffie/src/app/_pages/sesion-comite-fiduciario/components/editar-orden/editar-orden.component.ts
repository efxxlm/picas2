import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { ColumnasTabla, ComiteFiduciario, DataTable, SolicitudContractual, Temas } from '../../../../_interfaces/comiteFiduciario.interfaces';

@Component({
  selector: 'app-editar-orden',
  templateUrl: './editar-orden.component.html',
  styleUrls: ['./editar-orden.component.scss']
})
export class EditarOrdenComponent implements OnInit {

  dataSesion: ComiteFiduciario;
  tipoDeTemas: FormControl = new FormControl();
  temas: string[] = [ 'Solicitudes contractuales', 'Tema nuevo' ];
  //Values Booleanos para habilitar/deshabilitar "Solicitudes contractuales/Temas nuevos"
  solicitudBoolean: boolean = false;
  temaBoolean: boolean = false;
  //Data que sera recibida del servicio
  dataSolicitudContractual: SolicitudContractual[] = [];
  //Data que sera enviada a la tabla
  displayedColumns: string[] = [ 'fechaSolicitud', 'numeroSolicitud', 'tipoSolicitud', 'seleccionar' ];
  columnas: ColumnasTabla[] = [
    { titulo: 'Fecha de la solicitud', name: 'fechaSolicitud' },
    { titulo: 'Número de solicitud', name: 'numeroSolicitud' },
    { titulo: 'Tipo de solicitud', name: 'tipoSolicitud' },
  ];
  //Listado de los responsables del tema a tratar
  listadoResponsables: string[] = [];
  //Contador de temas creados
  temaContador: number = 0;

  constructor( private routes: Router ) {
    this.getDataSesion();
    console.log( this.dataSesion );
  }

  ngOnInit(): void {
    this.getListadoResponsables();
  };

  //Obtener listado responsable
  getListadoResponsables () {
    this.listadoResponsables = [
      'Dirección administrativa',
      'Dirección técnica'
    ];
  };

  getDataSesion () {

    if ( this.routes.getCurrentNavigation().extras.replaceUrl ) {
      this.routes.navigateByUrl( '/comiteFiduciario' );
      return;
    };
    this.dataSesion = this.routes.getCurrentNavigation().extras.state.sesion;

    if ( this.dataSesion.solicitudesSeleccionadas.length > 0 ) {
      this.solicitudBoolean = true;
      this.dataSolicitudContractual = this.dataSesion.solicitudesSeleccionadas;
    } else {
      this.solicitudBoolean = false;
    }
    if ( this.dataSesion.temas.length > 0 ) {
      this.temaBoolean = true;
    } else {
      this.temaBoolean = false;
    };

  };

  //Contador solicitudes seleccionadas
  totalSolicitudes () {

    let contador = 0;
  
    for ( let solicitud of this.dataSesion.solicitudesSeleccionadas ) {
  
      contador += solicitud.data.length;
  
    };
  
    return contador;
  
  };

  //Agregar tema a tratar en el comite
  agregarTema () {
    this.temaContador++;
    this.dataSesion.temas.push({
      numeroTema: this.temaContador,
      tiempoIntervencion: null,
      responsable: '',
      urlSoporte: '',
      temaTratar: ''
    });
  };
  
  //Eliminar tema a tratar en el comite
  eliminarTema ( tema: Temas ) {
    const index = this.dataSesion.temas.findIndex( data => data.numeroTema === tema.numeroTema );
    this.dataSesion.temas.splice( index, 1 );
  };

  //Metodo para recibir las solicitudes contractuales
  getSesionesSeleccionada ( event: DataTable ) {

    if ( event.estado ) {

      const index = this.dataSesion.solicitudesSeleccionadas.findIndex( value => value.nombreSesion === event.solicitud.nombreSesion );

      if ( index === -1 ) {
        this.dataSesion.solicitudesSeleccionadas.push( event.solicitud );
      } else {
        this.dataSesion.solicitudesSeleccionadas.splice( index, 1, event.solicitud );
      }


    } else {

      if ( event.solicitud.data.length === 0 ) {

        const index = this.dataSesion.solicitudesSeleccionadas.findIndex( value => value.nombreSesion === event.solicitud.nombreSesion );

        this.dataSesion.solicitudesSeleccionadas.splice( index, 1 );

      } else {

        const index = this.dataSesion.solicitudesSeleccionadas.findIndex( value => value.nombreSesion === event.solicitud.nombreSesion );
        this.dataSesion.solicitudesSeleccionadas.splice( index, 1, event.solicitud );

      };
      
    };

  };

  //Enviar data del comite fiduciario
  enviarSesion () {
    console.log( this.dataSesion );
  };

};